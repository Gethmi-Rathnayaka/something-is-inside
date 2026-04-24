using UnityEngine;

/// <summary>
/// Attach to: Elizabeth (Angry Doll) GameObject.
/// Also needs: DollVisuals on the same GameObject.
/// </summary>
public class ElizabethLogic : DollBase
{
    // Tracks how many days in a row she hasn't been cleaned
    private int daysNotCleaned = 0;

    // Set by GameManager on Day 8 blood event
    [HideInInspector] public bool bloodSplashed = false;

    // ── Interaction overrides ──────────────────────────────────────────────────

    public override string Clean()
    {
        daysNotCleaned = 0;
        state.bloodNotCleanedFlag = false;  // cleaning removes blood-flag danger

        state.ModifyCleanliness(30);
        state.ModifyMood(5);
        state.ModifyCorruption(-5);
        ApplyCareBonus();
        interactedToday = true;
        visuals?.UpdateVisuals(state);
        return "Elizabeth gleams. She looks... pleased.";
    }

    public override string BrushHair()
    {
        // Brush: +20 mood, -10 corruption (plus base -5 care bonus)
        state.ModifyMood(20);
        state.ModifyCorruption(-10);
        ApplyCareBonus();
        interactedToday = true;
        visuals?.UpdateVisuals(state);
        return "Elizabeth's hair flows perfectly. Her expression softens.";
    }

    public override string Ignore()
    {
        state.ModifyMood(-15);      // Elizabeth ignores hit is -15, not -10
        state.consecutiveIgnoreCount++;
        visuals?.UpdateVisuals(state);
        return "Elizabeth stares. Her jaw tightens.";
    }

    // ── Night logic ────────────────────────────────────────────────────────────

    protected override void ProcessNightEvent()
    {
        // Track uncleaned days
        if (state.cleanliness < 80)      // wasn't cleaned today
            daysNotCleaned++;
        else
            daysNotCleaned = 0;

        // ── Nightmare flag: 2 days without cleaning ──
        if (daysNotCleaned >= 2)
        {
            // Tell GameManager to trigger nightmare UI next day
            GameManager.Instance.SetNightmareFlag(true);
            Debug.Log("[Elizabeth] Nightmare flag SET — 2 days uncleaned.");
        }

        // ── Day 8 blood check ──
        // If blood was splashed and NOT cleaned before night → bad end flag
        if (bloodSplashed && state.cleanliness < 80)
        {
            state.bloodNotCleanedFlag = true;
            Debug.Log("[Elizabeth] Blood not cleaned — BAD END FLAG.");
        }

        // ── Hair grows longer on Day 7 if mood/cleanliness low ──
        // DayEventManager checks Day 7 separately; we just let stats reflect it.
    }
}