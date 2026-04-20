using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int day = 1;
    public int interactionsLeft = 3;

    private DollBase lastInteractedDoll;

    public DollManager dollManager;
    public UIManager uiManager;
    public DayEventManager dayEventManager;
    public InteractionManager interactionManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartDay();
    }

    public void StartDay()
    {
        // Check if we've reached day 10 (final judgment)
        if (day > 10)
        {
            DetermineFinalEnding();
            return;
        }

        interactionsLeft = 3;
        lastInteractedDoll = null;
        dayEventManager.SetDay(day);

        uiManager.UpdateDay(day);

        DayEvent dayEvent = dayEventManager.GetCurrentDayEvent();
        if (dayEvent != null)
        {
            uiManager.ShowMessage(dayEvent.eventTitle + "\n\n" + dayEvent.eventDescription);
            // Present the 3 choices to the player
            uiManager.DisplayChoices(dayEvent.choices);
        }
    }

    public void UseInteraction(DollBase targetDoll)
    {
        lastInteractedDoll = targetDoll;
        interactionsLeft--;

        if (interactionsLeft <= 0)
        {
            StartNight();
        }
    }

    public void StartNight()
    {
        // Night evaluation: dolls that weren't interacted with become neglected
        dollManager.EvaluateDolls(lastInteractedDoll);
        uiManager.StartNightSequence();

        day++;

        Invoke(nameof(StartDay), 2f);
    }

    private void DetermineFinalEnding()
    {
        // Evaluate which dolls are happy/corrupted based on player's 10 days of choices
        int happyDolls = 0;
        int corruptedDolls = 0;

        DollBase[] allDolls = dollManager.allDolls;
        foreach (DollBase doll in allDolls)
        {
            if (doll.state.mood >= 60)
                happyDolls++;
            else if (doll.state.corruption >= 70)
                corruptedDolls++;
        }

        // Determine ending
        if (happyDolls >= 2)
        {
            uiManager.ShowMessage("The dolls smile at you. You are welcome to stay.");
            // GOOD ENDING
        }
        else if (corruptedDolls >= 2)
        {
            uiManager.ShowMessage("The dolls' eyes turn black. You must LEAVE THIS HOUSE.");
            // BAD ENDING
        }
        else
        {
            uiManager.ShowMessage("The dolls stare. Neither acceptance nor rejection. Ambiguous ending.");
            // NEUTRAL ENDING
        }

        // Game ends here
    }
}

