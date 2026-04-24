using UnityEngine;

/// <summary>
/// Attach to: its own "DayEventManager" GameObject.
/// Assign in Inspector: uiManager, interactionManager, dollManager.
///
/// This script controls:
///  - The "observe" text shown at the start of each day
///  - Which choice buttons appear (by calling UIManager.SetChoices)
///  - Any special mid-day flags (blood splash, ribbon warning, etc.)
/// </summary>
public class DayEventManager : MonoBehaviour
{
    public UIManager         uiManager;
    public InteractionManager interactionManager;
    public DollManager       dollManager;

    // ── Entry point called by GameManager.StartDay() ────────────────────────────

    public void ShowDayEvent(int day)
    {
        switch (day)
        {
            case 1:  Day1();  break;
            case 2:  Day2();  break;
            case 3:  Day3();  break;
            case 4:  Day4();  break;
            case 5:  Day5();  break;
            case 6:  Day6();  break;
            case 7:  Day7();  break;
            case 8:  Day8();  break;
            case 9:  Day9();  break;
            case 10: Day10(); break;
        }
    }

    // ── Day 1 ────────────────────────────────────────────────────────────────────
    // Intro day. Dusty house, player finds the box.

    private void Day1()
    {
        uiManager.ShowMessage(
            "It smells of dust in here...\n" +
            "What's this huge box? Oh — a note fell!\n" +
            "\"Sweetie, take care of them. They were my closest companions.\"\n" +
            "Hmm. Grandma took good care of these."
        );

        ShowStandardChoices();
    }

    // ── Day 2 ────────────────────────────────────────────────────────────────────

    private void Day2()
    {
        var eli   = dollManager.elizabeth;
        var oliver = dollManager.oliver;
        var marie  = dollManager.marie;

        string observeText = "Hmm. Why is Oliver's face wet? Just like tears.";

        if (eli.state.mood < 50)
            observeText += "\nElizabeth's expression looks... tighter than yesterday.";

        if (oliver.state.cleanliness < 50 || eli.state.cleanliness < 50 || marie.state.cleanliness < 50)
            observeText += "\nThere's dust gathering on the shelf.";

        if (marie.state.corruption > 50)
            observeText += "\nMarie's ribbon twitches. Did it... move?";

        // Nightmare flag check (from Elizabeth ignoring on Day 1)
        if (GameManager.Instance.GetNightmareFlag())
            observeText += "\n...Elizabeth was in your dreams last night. She looked angry.";

        uiManager.ShowMessage(observeText);
        ShowStandardChoices();
    }

    // ── Day 3 ────────────────────────────────────────────────────────────────────

    private void Day3()
    {
        var eli    = dollManager.elizabeth;
        var oliver = dollManager.oliver;

        string observeText = "";

        if (oliver.state.mood < 30)
            observeText += "Oliver is... definitely crying. There are tiny wet streaks on his cheeks.\n";

        if (eli.state.mood < 50)
            observeText += "Elizabeth's face looks distorted. Like a smile that's too wide.\n";

        if (string.IsNullOrEmpty(observeText))
            observeText = "The morning is quiet. The dolls sit on the shelf.";

        uiManager.ShowMessage(observeText);
        ShowStandardChoices();
    }

    // ── Day 4 ────────────────────────────────────────────────────────────────────

    private void Day4()
    {
        uiManager.ShowMessage(
            "They're watching me.\n" +
            "I walked across the room and — their eyes followed.\n" +
            "Left. Right. Left again.\n" +
            "...They're just dolls."
        );
        ShowStandardChoices();
    }

    // ── Day 5 ────────────────────────────────────────────────────────────────────
    // Special: ribbon inspection moment.

    private void Day5()
    {
        uiManager.ShowMessage(
            "Marie's ribbon is dark red today. Wet.\n" +
            "It smells... rusty. Like blood.\n" +
            "Should I remove it?"
        );

        // Override choices: ribbon decision comes FIRST before standard interactions
        uiManager.SetChoices(
            new (string label, System.Action action)[]
            {
                ("Remove the ribbon",  () => interactionManager.RemoveMariesRibbon()),
                ("Leave it alone",     () => interactionManager.LeaveMariesRibbon()),
                ("Not today",          () => interactionManager.NotToday())
            }
        );
    }

    // ── Day 6 ────────────────────────────────────────────────────────────────────

    private void Day6()
    {
        var eli   = dollManager.elizabeth;
        var oliver = dollManager.oliver;
        var marie  = dollManager.marie;

        // Check if 2+ dolls have corruption > 50
        int highCorrupt = 0;
        if (eli.state.corruption   > 50) highCorrupt++;
        if (oliver.state.corruption > 50) highCorrupt++;
        if (marie.state.corruption  > 50) highCorrupt++;

        string observeText = "";

        if (highCorrupt >= 2)
        {
            observeText =
                "They've switched places.\n" +
                "Elizabeth is where Oliver was. Oliver where Marie sat.\n" +
                "I didn't touch them.";
            uiManager.ShowMessage(observeText);

            uiManager.SetChoices(
                new (string label, System.Action action)[]
                {
                    ("Switch them back", () =>
                    {
                        // Switching back adds +5 corruption to both swapped dolls
                        eli.state.ModifyCorruption(5);
                        oliver.state.ModifyCorruption(5);
                        interactionManager.NotToday();
                        uiManager.ShowMessage("You move them back. They feel heavier than they should.");
                    }),
                    ("Leave them", () =>
                    {
                        interactionManager.NotToday();
                        uiManager.ShowMessage("You leave them. Maybe they prefer it this way.");
                    }),
                    ("Interact with them",  () => ShowStandardChoices())
                }
            );
        }
        else
        {
            observeText = "A quieter day. The shelf looks the same as you left it.";
            uiManager.ShowMessage(observeText);
            ShowStandardChoices();
        }
    }

    // ── Day 7 ────────────────────────────────────────────────────────────────────

    private void Day7()
    {
        var eli = dollManager.elizabeth;

        string observeText = "";

        if (eli.state.mood < 40 || eli.state.cleanliness < 40)
        {
            observeText = "Elizabeth's hair is longer.\n" +
                          "That's impossible. But it is.\n" +
                          "It trails over the shelf edge now.";
        }
        else
        {
            observeText = "Day 7. More than halfway through grandma's \"10 days\".";
        }

        uiManager.ShowMessage(observeText);
        ShowStandardChoices();
    }

    // ── Day 8 ────────────────────────────────────────────────────────────────────
    // Special: blood splash event.

    private void Day8()
    {
        // Trigger blood event
        dollManager.elizabeth.bloodSplashed = true;

        uiManager.ShowMessage(
            "I reached for something on the shelf and—\n" +
            "A paper cut. My hand is bleeding.\n" +
            "The blood drops hit Elizabeth's dress."
        );

        uiManager.SetChoices(
            new (string label, System.Action action)[]
            {
                ("Clean Elizabeth immediately", () => interactionManager.CleanBloodFromElizabeth()),
                ("It's fine, ignore it",        () => interactionManager.IgnoreBloodOnElizabeth()),
                ("Interact with others",        () => ShowStandardChoices())
            }
        );
    }

    // ── Day 9 ────────────────────────────────────────────────────────────────────

    private void Day9()
    {
        var eli    = dollManager.elizabeth;
        var oliver = dollManager.oliver;
        var marie  = dollManager.marie;

        string observeText = "The house is quiet. Too quiet.";

        // Corruption-dependent intensification
        int avgCorrupt = (eli.state.corruption + oliver.state.corruption + marie.state.corruption) / 3;
        if (avgCorrupt > 50)
        {
            observeText += "\nThey're all staring at me.\n" +
                           "I can feel it even when my back is turned.";
        }

        uiManager.ShowMessage(observeText);
        ShowStandardChoices();
    }

    // ── Day 10 ───────────────────────────────────────────────────────────────────

    private void Day10()
    {
        uiManager.ShowMessage(
            "Day 10. Grandma's note said \"10 days\".\n\n" +
            "The dolls are all looking at me at once.\n" +
            "Whatever happens today — it ends today."
        );

        // Final day: player gets one last chance to care for them
        ShowStandardChoices();
    }

    // ── Helpers ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Standard 3-choice layout: choose a doll → choose action.
    /// For simplicity this shows a flat list of the most common actions.
    /// You can expand this into a two-step menu (pick doll first, then action).
    /// </summary>
    private void ShowStandardChoices()
    {
        uiManager.SetChoices(
            new (string label, System.Action action)[]
            {
                ("Care for Elizabeth", () => ShowElizabethActions()),
                ("Care for Oliver",    () => ShowOliverActions()),
                ("Care for Marie",     () => ShowMarieActions())
                // You can add a "Not today" row here if you want to skip all dolls
            }
        );
    }

    private void ShowElizabethActions()
    {
        uiManager.SetChoices(
            new (string label, System.Action action)[]
            {
                ("Clean Elizabeth",      () => interactionManager.CleanDoll("elizabeth")),
                ("Brush Elizabeth's hair", () => interactionManager.BrushHair()),
                ("Not today",            () => interactionManager.IgnoreDoll("elizabeth"))
            }
        );
    }

    private void ShowOliverActions()
    {
        uiManager.SetChoices(
            new (string label, System.Action action)[]
            {
                ("Comfort Oliver",       () => interactionManager.ComfortOliver()),
                ("Gift: Ribbon",         () => interactionManager.GiftOliver("ribbon")),
                ("Gift: Clover",         () => interactionManager.GiftOliver("clover")),
                ("Gift: Wrong item",     () => interactionManager.GiftOliver("rock"))    // example wrong gift
            }
        );
    }

    private void ShowMarieActions()
    {
        uiManager.SetChoices(
            new (string label, System.Action action)[]
            {
                ("Clean Marie",  () => interactionManager.CleanDoll("marie")),
                ("Ignore Marie", () => interactionManager.IgnoreDoll("marie"))
            }
        );
    }
}