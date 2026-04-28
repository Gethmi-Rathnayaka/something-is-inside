using UnityEngine;
using System.Collections.Generic;

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
    public UIManager uiManager;
    public InteractionManager interactionManager;
    public DollManager dollManager;

    private BackgroundManager backgroundManager;
    private ScreenEffectsManager screenEffectsManager;

    private void OnEnable()
    {
        backgroundManager = BackgroundManager.Instance;
        screenEffectsManager = ScreenEffectsManager.Instance;
    }

    // ── Entry point called by GameManager.StartDay() ────────────────────────────

    public void ShowDayEvent(int day)
    {
        switch (day)
        {
            case 1: Day1(); break;
            case 2: Day2(); break;
            case 3: Day3(); break;
            case 4: Day4(); break;
            case 5: Day5(); break;
            case 6: Day6(); break;
            case 7: Day7(); break;
            case 8: Day8(); break;
            case 9: Day9(); break;
            case 10: Day10(); break;
        }
    }

    // ── Day 1 ────────────────────────────────────────────────────────────────────
    // Intro day. Dusty house, player finds the box.

    private void Day1()
    {
        uiManager.StartDialogue(new string[]
        {
            "It smells of dust in here...",
            "What's this huge box? Oh — a note fell!",
            "\"Sweetie, take care of them. They were my closest companions.\"",
            "Hmm. Grandma took good care of these."
        },
        () =>
        {
            ShowStandardChoices();
        });
    }

    // ── Day 2 ────────────────────────────────────────────────────────────────────

    private void Day2()
    {
        var eli = dollManager.elizabeth;
        var oliver = dollManager.oliver;
        var marie = dollManager.marie;

        var lines = new System.Collections.Generic.List<string>
        {
            "Hmm. Why is Oliver's face wet? Just like tears."
        };

        // Oliver is always wet on Day 2 (matches the dialogue)
        if (oliver.visuals != null)
        {
            oliver.visuals.SetSpriteFlag("isWet", true);
            oliver.visuals.UpdateVisuals(oliver.state);
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.TrackEvent("day2_OliverCried");
                SaveManager.Instance.TrackSpriteUnlocked("oliver_Wet");
            }
        }

        if (eli.state.mood < 50)
            lines.Add("Elizabeth's expression looks... tighter than yesterday.");

        if (oliver.state.cleanliness < 50 || eli.state.cleanliness < 50 || marie.state.cleanliness < 50)
            lines.Add("There's dust gathering on the shelf.");

        if (marie.state.corruption > 50)
            lines.Add("Marie's ribbon twitches. Did it... move?");

        uiManager.StartDialogue(lines.ToArray(), () =>
        {
            ShowStandardChoices();
        });
    }

    // ── Day 3 ────────────────────────────────────────────────────────────────────

    private void Day3()
    {
        var eli = dollManager.elizabeth;
        var oliver = dollManager.oliver;

        var lines = new System.Collections.Generic.List<string>();

        if (oliver.state.mood < 30)
            lines.Add("Oliver is... definitely crying. There are tiny wet streaks on his cheeks.");

        if (eli.state.mood < 50)
        {
            lines.Add("Elizabeth's expression looks... distorted. Like there's bloody vines on her face.");

            // Update sprite for distorted face
            if (eli.visuals != null)
            {
                eli.visuals.SetSpriteFlag("hasDistortedFace", true);
                eli.visuals.UpdateVisuals(eli.state);
                if (SaveManager.Instance != null)
                {
                    SaveManager.Instance.TrackEvent("day3_ElizabethDistorted");
                    SaveManager.Instance.TrackSpriteUnlocked("elizabeth_DistortedFace");
                }
            }
        }

        if (lines.Count == 0)
            lines.Add("The morning is quiet. The dolls sit on the shelf.");

        uiManager.StartDialogue(lines.ToArray(), () =>
        {
            ShowStandardChoices();
        });
    }

    // ── Day 4 ────────────────────────────────────────────────────────────────────

    private void Day4()
    {
        uiManager.StartDialogue(new string[]
        {
            "They're watching me.",
            "I walked across the room and — their eyes followed.",
            "Left. Right. Left again.",
            "...They're just dolls right? They're not alive. They're not watching me. They're just dolls."
        },
        () =>
        {
            ShowStandardChoices();
        });
    }

    // ── Day 5 ────────────────────────────────────────────────────────────────────
    // Special: ribbon inspection moment.

    private void Day5()
    {
        var marie = dollManager.marie;

        if (marie.visuals != null)
        {
            marie.visuals.SetSpriteFlag("isWrappedInRibbon", false);
            int originalCorruption = marie.state.corruption;
            if (originalCorruption <= 60)
                marie.state.corruption = 61;
            marie.visuals.UpdateVisuals(marie.state);
            marie.state.corruption = originalCorruption;
        }

        uiManager.StartDialogue(new string[]
        {
            "Marie's ribbon is dark red today. Wet.",
            "It smells... rusty. Like blood.",
            "Should I remove it?"
        },
        () =>
        {
            if (SaveManager.Instance != null)
                SaveManager.Instance.TrackEvent("day5_RibbonEvent");

            // Show special event panel for ribbon decision
            uiManager.ShowSpecialEventPanel(
                "Marie's Ribbon",
                "The ribbon is wet and looks... wrong.\nShould you remove it?",
                new (string label, System.Action action)[]
                {
                    ("Remove the ribbon",  () =>
                    {
                        if (SaveManager.Instance != null)
                            SaveManager.Instance.TrackEvent("day5_RibbonRemoved");
                        marie.state.ribbonRemovedFlag = true;
                        uiManager.ShowMessage("The ribbon unravels. Something in the air changes.");
                    }),
                    ("Leave it alone",     () =>
                    {
                        // Visual-only outcome; no stat changes.
                        if (marie.visuals != null)
                        {
                            marie.visuals.SetSpriteFlag("isWrappedInRibbon", false);
                            marie.visuals.UpdateVisuals(marie.state);
                        }
                        if (SaveManager.Instance != null)
                            SaveManager.Instance.TrackEvent("day5_RibbonEvent");
                        uiManager.ShowMessage("You leave the ribbon alone.");
                    }),
                    ("Not today",          () => uiManager.ShowMessage("You leave the ribbon alone for now."))
                }
            );
        });
    }

    // ── Day 6 ────────────────────────────────────────────────────────────────────

    private void Day6()
    {
        var eli = dollManager.elizabeth;
        var oliver = dollManager.oliver;
        var marie = dollManager.marie;

        if (marie.visuals != null)
        {
            marie.visuals.SetSpriteFlag("isWrappedInRibbon", false);
            marie.visuals.UpdateVisuals(marie.state);
        }

        // Track which dolls have corruption > 50
        bool eliCorrupt = eli.state.corruption > 50;
        bool oliverCorrupt = oliver.state.corruption > 50;
        bool marieCorrupt = marie.state.corruption > 50;

        int highCorrupt = (eliCorrupt ? 1 : 0) + (oliverCorrupt ? 1 : 0) + (marieCorrupt ? 1 : 0);

        if (highCorrupt >= 2)
        {
            // Build dialogue describing which dolls look corrupted
            List<string> corruptLines = new List<string>();
            corruptLines.Add("Something is different about them today.");

            if (eliCorrupt)
                corruptLines.Add("Elizabeth Her face looks... wrong.");
            if (oliverCorrupt)
                corruptLines.Add("Oliver's eyes are dripping blood!");
            if (marieCorrupt)
                corruptLines.Add("Marie's ribbon looks wet. Something is dripping from it.");

            corruptLines.Add("They've all changed. What do I do?");

            uiManager.StartDialogue(corruptLines.ToArray(),
            () =>
            {
                uiManager.ShowSpecialEventPanel(
                    "Something Wrong",
                    "The dolls look worse than before.\nWhat will you do?",
                    new (string label, System.Action action)[]
                    {
                        ("Comfort them", () =>
                        {
                            uiManager.ShowMessage("You try to comfort them. They feel cold.");
                        }),
                        ("Leave them alone", () =>
                        {
                            uiManager.ShowMessage("You step back. Maybe you shouldn't touch them right now.");
                        }),
                        ("Interact with them",  () => ShowStandardChoices())
                    }
                );
                if (SaveManager.Instance != null)
                    SaveManager.Instance.TrackEvent("day6_DollsCorrupted");
            });
        }
        else
        {
            uiManager.StartDialogue(new string[]
            {
                "A quieter day. The shelf looks the same as you left it."
            },
            () =>
            {
                ShowStandardChoices();
            });
        }
    }

    // ── Day 7 ────────────────────────────────────────────────────────────────────

    private void Day7()
    {
        var eli = dollManager.elizabeth;

        var lines = new System.Collections.Generic.List<string>();

        if (eli.state.mood < 40 || eli.state.cleanliness < 40)
        {
            lines.Add("Elizabeth's hair is longer.");
            lines.Add("That's impossible. But it is.");
            lines.Add("It trails over the shelf edge now.");

            // Update sprite to show longer hair
            if (eli.visuals != null)
            {
                eli.visuals.SetSpriteFlag("hasLongHair", true);
                eli.visuals.UpdateVisuals(eli.state);
                if (SaveManager.Instance != null)
                {
                    SaveManager.Instance.TrackSpriteUnlocked("elizabeth_LongHair");
                }
            }
        }
        else
        {
            lines.Add("Day 7. More than halfway through grandma's \"10 days\".");
        }

        uiManager.StartDialogue(lines.ToArray(), () =>
        {
             if (SaveManager.Instance != null)
                SaveManager.Instance.TrackEvent("day7_ElizabethHair");
            ShowStandardChoices();
        });
    }

    // ── Day 8 ────────────────────────────────────────────────────────────────────
    // Special: blood splash event.

    private void Day8()
    {
        // Trigger blood event
        dollManager.elizabeth.bloodSplashed = true;

        // Update Elizabeth's sprite to show blood
        if (dollManager.elizabeth.visuals != null)
        {
            dollManager.elizabeth.visuals.SetSpriteFlag("hasBlood", true);
            dollManager.elizabeth.visuals.UpdateVisuals(dollManager.elizabeth.state);
            if (SaveManager.Instance != null)
                SaveManager.Instance.TrackEvent("day8_BloodSplash");
        }

        uiManager.StartDialogue(new string[]
        {
            "I reached for something on the shelf and—",
            "A paper cut. My hand is bleeding.",
            "The blood drops hit Elizabeth's dress."
        },
        () =>
        {
            // Play blood splash screen effect
            if (screenEffectsManager != null)
            {
                screenEffectsManager.PlayBloodSplashEffect(2f);
            }

            uiManager.ShowSpecialEventPanel(
                "Blood on Elizabeth",
                "Your blood dripped onto Elizabeth's dress.\nIt's spreading...",
                new (string label, System.Action action)[]
                {
                    ("Clean Elizabeth immediately", () =>
                    {
                        dollManager.elizabeth.bloodSplashed = false;
                        dollManager.elizabeth.state.bloodNotCleanedFlag = false;

                        if (dollManager.elizabeth.visuals != null)
                        {
                            dollManager.elizabeth.visuals.SetSpriteFlag("hasBlood", false);
                            dollManager.elizabeth.visuals.UpdateVisuals(dollManager.elizabeth.state);
                        }

                        uiManager.ShowMessage("You clean the blood from Elizabeth's dress.");
                    }),
                    ("It's fine, ignore it",        () =>
                    {
                        if (SaveManager.Instance != null)
                            SaveManager.Instance.TrackEvent("day8_BloodIgnored");
                        dollManager.elizabeth.bloodSplashed = true;
                        dollManager.elizabeth.state.bloodNotCleanedFlag = true;
                        uiManager.ShowMessage("You look away. It's probably nothing.");
                    }),
                    ("Interact with others",        () => uiManager.ShowMessage("You ignore the blood for now."))
                }
            );
        });
    }

    // ── Day 9 ────────────────────────────────────────────────────────────────────

    private void Day9()
    {
        var eli = dollManager.elizabeth;
        var oliver = dollManager.oliver;
        var marie = dollManager.marie;

        var lines = new System.Collections.Generic.List<string>
        {
            "The house is quiet. Too quiet."
        };

        // Corruption-dependent intensification
        int avgCorrupt = (eli.state.corruption + oliver.state.corruption + marie.state.corruption) / 3;
        if (avgCorrupt > 50)
        {
            lines.Add("They're all staring at me.");
            lines.Add("I can feel it even when my back is turned.");
            if (SaveManager.Instance != null)
                SaveManager.Instance.TrackEvent("day9_CorruptionIntensified");
        }

        uiManager.StartDialogue(lines.ToArray(), () =>
        {
            ShowStandardChoices();
        });
    }

    // ── Day 10 ───────────────────────────────────────────────────────────────────

    private void Day10()
    {
        uiManager.StartDialogue(new string[]
        {
            "Day 10. Grandma's note said \"10 days\".",
            "The dolls are all looking at me at once.",
            "Whatever happens today — it ends today."
        },
        () =>
        {
            ShowStandardChoices();
        });
    }

    // ── Helpers ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Standard interaction mode: player selects a doll to interact with.
    /// After doll click → action → interaction completes.
    /// </summary>
    private void ShowStandardChoices()
    {
        uiManager.StartDollSelection(GameManager.Instance.interactionsLeft);
    }

    // ── Helper Methods for Sprite/Effect Management ─────────────────────────────

    /// <summary>
    /// Update background based on overall game state (corruption levels, day, etc).
    /// Call this from GameManager at the start of each day.
    /// </summary>
    public void UpdateEnvironmentForDay(int day)
    {
        if (backgroundManager == null)
            return;

        // Day 10: final day atmosphere
        if (day == 10)
        {
            backgroundManager.SetFinalDayBackground();
            return;
        }

        // Check overall corruption to decide environment
        backgroundManager.UpdateBackgroundBasedOnCorruption(dollManager);
    }

    /// <summary>
    /// Helper: trigger a creepy vignette effect for bad events.
    /// </summary>
    public void PlayCreepyEffect()
    {
        if (screenEffectsManager != null)
        {
            screenEffectsManager.PlayVignetteEffect(0.3f, 1.5f);
        }
    }

    /// <summary>
    /// Helper: clear all special sprite states (for resets or specific transitions).
    /// </summary>
    public void ClearAllDollSpriteStates()
    {
        if (dollManager.elizabeth.visuals != null)
            dollManager.elizabeth.visuals.GetSpriteState().ClearAllStates();
        if (dollManager.oliver.visuals != null)
            dollManager.oliver.visuals.GetSpriteState().ClearAllStates();
        if (dollManager.marie.visuals != null)
            dollManager.marie.visuals.GetSpriteState().ClearAllStates();
    }
}