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
    private bool loadingFromSave = false;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Initialize doll stats
        dollManager.InitializeDolls();

        // Check if we should load from a saved game
        if (SaveManager.Instance != null && SaveManager.Instance.HasGameState())
        {
            loadingFromSave = true;
            SaveManager.Instance.LoadGameState();

            // Restore game state
            var state = SaveManager.Instance.gameState;
            day = state.currentDay;
            interactionsLeft = state.interactionsLeft;

            // Apply doll states
            state.elizabeth.ApplyTo(dollManager.elizabeth.state);
            state.oliver.ApplyTo(dollManager.oliver.state);
            state.marie.ApplyTo(dollManager.marie.state);

            // Update UI
            uiManager.UpdateDay(day);

            // If continuing mid-day, DON'T replay intro dialogue
            if (interactionsLeft < 3)
            {
                // Resume gameplay directly
                uiManager.StartDollSelection(interactionsLeft);
            }
            else
            {
                dayEventManager.ShowDayEvent(day);
                SaveCurrentGameState();
            }
        }
        else
        {
            // New game
            StartDay();
        }
        SaveCurrentGameState();
    }

    // ── Day Flow ────────────────────────────────────────────────────────────────

    public void StartDay()
    {
        if (day > 10)
        {
            DetermineFinalEnding();
            return;
        }

        // Track day reached
        if (SaveManager.Instance != null)
            SaveManager.Instance.TrackDayReached(day);

        interactionsLeft = 3;
        nightmareFlag = false;

        uiManager.UpdateDay(day);
        dayEventManager.ShowDayEvent(day);      // shows observe text + sets up choices
        SaveCurrentGameState();
    }

    /// <summary>Called by InteractionManager after every successful interaction.</summary>
    public void UseInteraction()
    {
        interactionsLeft--;

        uiManager.UpdateInteractionHeader(interactionsLeft);
        SaveCurrentGameState();

        if (interactionsLeft <= 0)
            EndDay();
    }

    /// <summary>Save the current game state to disk.</summary>
    public void SaveCurrentGameState()
    {
        if (SaveManager.Instance != null)
        {

            SaveManager.Instance.SaveGameState(
                day,
                interactionsLeft,
                dollManager.elizabeth,
                dollManager.oliver,
                dollManager.marie
            );
        }
    }


    public void EndDay()
    {
        // Keep end-of-day pacing but without night overlays.
        uiManager.HideInteractionUI();
        uiManager.HideChoices();

        uiManager.StartDialogue(new string[] { "Day is over." }, () =>
        {
            // Run per-day processing and move to the next day immediately.
            dollManager.RunNightForAllDolls();

            if (CheckImmediateBadEnd())
                return;

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
            if (SaveManager.Instance != null)
                SaveManager.Instance.TrackEnding("bad_ribbon");
            uiManager.ShowEnding("The ribbon unravels completely. The room goes dark.\n— BAD END —", EndingType.Bad);
            return true;
        }

        // Elizabeth blood not cleaned (Day 8+)
        if (elizabeth.state.bloodNotCleanedFlag)
        {
            if (SaveManager.Instance != null)
                SaveManager.Instance.TrackEnding("bad_blood");
            uiManager.ShowEnding("Something spreads from Elizabeth's dress.\nYou should have cleaned it.\n— BAD END —", EndingType.Bad);
            return true;
        }

        // Oliver 3 days no comfort
        if (oliver.state.oliverBadEndFlag)
        {
            if (SaveManager.Instance != null)
                SaveManager.Instance.TrackEnding("bad_oliver");
            uiManager.ShowEnding("Oliver's crying fills the house.\nThere is nothing left to comfort.\n— BAD END —", EndingType.Bad);
            return true;
        }

        // World collapse: 2 dolls corruption > 70
        int highCorruption = 0;
        foreach (var doll in dollManager.allDolls)
            if (doll.state.corruption >= 70) highCorruption++;

        if (highCorruption >= 2)
        {
            if (SaveManager.Instance != null)
                SaveManager.Instance.TrackEnding("bad_corruption");
            uiManager.ShowEnding("Something is very wrong.\nThe dolls are no longer just dolls.\n— BAD END —", EndingType.Bad);
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
            if (SaveManager.Instance != null)
                SaveManager.Instance.TrackEnding("good");
            uiManager.ShowEnding("The dolls smile at you.\nYou are welcome to stay.\n— GOOD END —", EndingType.Good);
            return;
        }

        // Neutral end (fallthrough)
        if (SaveManager.Instance != null)
            SaveManager.Instance.TrackEnding("neutral");
        uiManager.ShowEnding("The dolls stare.\nNeither acceptance nor rejection.\n— NEUTRAL END —", EndingType.Neutral);
    }
}