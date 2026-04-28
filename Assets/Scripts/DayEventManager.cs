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

        // If Oliver cried, update his sprite
        if (oliver.state.mood < 50 && oliver.visuals != null)
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

        if (GameManager.Instance.GetNightmareFlag())
            lines.Add("...Elizabeth was in your dreams last night. She looked angry.");

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
            lines.Add("Elizabeth's expression looks... distorted. Like a smile that's too wide.");

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
            "...They're just dolls."
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
                        interactionManager.RemoveMariesRibbon();
                    }),
                    ("Leave it alone",     () => 
                    {
                        // If corruption is very high, ribbon wraps around her
                        if (marie.state.corruption > 70 && marie.visuals != null)
                        {
                            marie.visuals.SetSpriteFlag("isWrappedInRibbon", true);
                            marie.visuals.UpdateVisuals(marie.state);
                            if (SaveManager.Instance != null)
                            {
                                SaveManager.Instance.TrackEvent("day5_RibbonWrapped");
                                SaveManager.Instance.TrackSpriteUnlocked("marie_WrappedInRibbon");
                            }
                            uiManager.ShowMessage("The ribbon coils tighter around Marie...");
                        }
                        interactionManager.LeaveMariesRibbon();
                    }),
                    ("Not today",          () => interactionManager.NotToday())
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

        // Check if 2+ dolls have corruption > 50
        int highCorrupt = 0;
        if (eli.state.corruption > 50) highCorrupt++;
        if (oliver.state.corruption > 50) highCorrupt++;
        if (marie.state.corruption > 50) highCorrupt++;

        if (highCorrupt >= 2)
        {
            uiManager.StartDialogue(new string[]
            {
                "They've switched places.",
                "Elizabeth is where Oliver was. Oliver where Marie sat.",
                "I didn't touch them."
            },
            () =>
            {
                uiManager.ShowSpecialEventPanel(
                    "The Dolls Have Moved",
                    "They've switched places on the shelf.\nWhat will you do?",
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
                if (SaveManager.Instance != null)
                    SaveManager.Instance.TrackEvent("day6_DollsMoved");
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
                    ("Clean Elizabeth immediately", () => interactionManager.CleanBloodFromElizabeth()),
                    ("It's fine, ignore it",        () => 
                    {
                        if (SaveManager.Instance != null)
                            SaveManager.Instance.TrackEvent("day8_BloodIgnored");
                        interactionManager.IgnoreBloodOnElizabeth();
                    }),
                    ("Interact with others",        () => ShowStandardChoices())
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