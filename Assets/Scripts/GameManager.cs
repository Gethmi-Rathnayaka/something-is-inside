using UnityEngine;

/// <summary>
/// Attach to: a persistent "GameManager" GameObject in your main scene.
/// Assign in Inspector: dollManager, uiManager, dayEventManager, interactionManager.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Day Tracking")]
    public int day = 1;
    public int interactionsLeft = 3;

    [Header("Scene References")]
    public DollManager dollManager;
    public UIManager uiManager;
    public DayEventManager dayEventManager;
    public InteractionManager interactionManager;

    // ── Internal flags ──────────────────────────────────────────────────────────
    private bool nightmareFlag = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Set starting stats (called once)
        dollManager.InitializeDolls();
        StartDay();
    }

    // ── Day Flow ────────────────────────────────────────────────────────────────

    public void StartDay()
    {
        if (day > 10)
        {
            DetermineFinalEnding();
            return;
        }

        interactionsLeft = 3;
        nightmareFlag = false;

        uiManager.UpdateDay(day);
        dayEventManager.ShowDayEvent(day);      // shows observe text + sets up choices
    }

    /// <summary>Called by InteractionManager after every successful interaction.</summary>
    public void UseInteraction()
    {
        interactionsLeft--;

        uiManager.UpdateInteractionHeader(interactionsLeft);

        if (interactionsLeft <= 0)
            StartNight();
    }

    public void StartNight()
    {
        // Hide all interaction UI and show "Day is over" dialogue
        uiManager.HideInteractionUI();
        uiManager.HideChoices();

        uiManager.StartDialogue(new string[] { "Day is over." }, () =>
        {
            RunNightProcessing();
        });
    }

    private void RunNightProcessing()
    {
        // Run night processing for all dolls
        dollManager.RunNightForAllDolls();

        // Check bad end conditions immediately
        if (CheckImmediateBadEnd())
            return;

        uiManager.StartNightSequence(() =>
        {
            day++;
            StartDay();
        });
    }

    public void SetNightmareFlag(bool value) => nightmareFlag = value;
    public bool GetNightmareFlag() => nightmareFlag;


    /// <summary>
    /// Checks conditions that cause an immediate bad end mid-game.
    /// Returns true if a bad end was triggered (caller should stop day flow).
    /// </summary>
    private bool CheckImmediateBadEnd()
    {
        var elizabeth = dollManager.elizabeth;
        var oliver = dollManager.oliver;
        var marie = dollManager.marie;

        // Marie ribbon removed
        if (marie.state.ribbonRemovedFlag)
        {
            uiManager.ShowEnding("The ribbon unravels completely. The room goes dark.\n— BAD END —");
            return true;
        }

        // Elizabeth blood not cleaned (Day 8+)
        if (elizabeth.state.bloodNotCleanedFlag)
        {
            uiManager.ShowEnding("Something spreads from Elizabeth's dress.\nYou should have cleaned it.\n— BAD END —");
            return true;
        }

        // Oliver 3 days no comfort
        if (oliver.state.oliverBadEndFlag)
        {
            uiManager.ShowEnding("Oliver's crying fills the house.\nThere is nothing left to comfort.\n— BAD END —");
            return true;
        }

        // World collapse: 2 dolls corruption > 70
        int highCorruption = 0;
        foreach (var doll in dollManager.allDolls)
            if (doll.state.corruption >= 70) highCorruption++;

        if (highCorruption >= 2)
        {
            uiManager.ShowEnding("Something is very wrong.\nThe dolls are no longer just dolls.\n— BAD END —");
            return true;
        }

        return false;
    }

    /// <summary>Called after Day 10 night processing.</summary>
    private void DetermineFinalEnding()
    {
        // Check bad flags first
        if (CheckImmediateBadEnd()) return;

        // Good end: all three dolls mood >= 60 AND corruption < 40
        bool goodEnd = true;
        foreach (var doll in dollManager.allDolls)
        {
            if (doll.state.mood < 60 || doll.state.corruption >= 40)
            {
                goodEnd = false;
                break;
            }
        }

        if (goodEnd)
        {
            uiManager.ShowEnding("The dolls smile at you.\nYou are welcome to stay.\n— GOOD END —");
            return;
        }

        // Neutral end (fallthrough)
        uiManager.ShowEnding("The dolls stare.\nNeither acceptance nor rejection.\n— NEUTRAL END —");
    }
}