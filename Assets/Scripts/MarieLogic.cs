using UnityEngine;

/// <summary>
/// Attach to: Marie (Ribbon Doll) GameObject.
/// Also needs: DollVisuals on the same GameObject.
/// </summary>
public class MarieLogic : DollBase
{
    // Tracks consecutive days Marie was fully ignored (no interaction at all)
    private int daysFullyIgnored = 0;

    private void ClearRibbonSprite()
    {
        if (visuals == null)
            return;

        visuals.SetSpriteFlag("isWrappedInRibbon", false);
        visuals.UpdateVisuals(state);
    }

    // ── Interaction overrides ──────────────────────────────────────────────────

    public override string Clean()
    {
        state.ModifyCleanliness(30);
        state.ModifyMood(5);
        ApplyCareBonus();
        interactedToday = true;
        daysFullyIgnored = 0;
        ClearRibbonSprite();
        visuals?.UpdateVisuals(state);
        return "Marie sits perfectly still as you clean her.";
    }

    public override string BrushHair()
    {
        // Brushing Marie is more like carefully tending the ribbon and smoothing her appearance.
        state.ModifyMood(5);
        state.ModifyCorruption(-3);
        interactedToday = true;
        daysFullyIgnored = 0;
        ClearRibbonSprite();
        visuals?.UpdateVisuals(state);
        return "You carefully smooth Marie's hair and ribbon. She feels colder afterward.";
    }

    /// <summary>
    /// Call this when player tries to remove the ribbon (Day 5 prompt).
    /// Sets bad end flag immediately.
    /// </summary>
    public string RemoveRibbon()
    {
        state.ribbonRemovedFlag = true;
        state.ModifyCorruption(50);
        state.ModifyMood(-20);
        interactedToday = true;
        visuals?.UpdateVisuals(state);
        Debug.Log("[Marie] Ribbon removed — BAD END FLAG.");
        return "The ribbon unravels. Something in the air changes.";
    }

    /// <summary>
    /// Called when player leaves the ribbon alone (Day 5 choice).
    /// </summary>
    public string LeaveRibbon()
    {
        interactedToday = true;
        return "You leave the ribbon. It looks... almost grateful.";
    }

    // Marie has no Comfort or BrushHair actions
    public override string Ignore()
    {
        // Marie: no immediate mood change on ignore, but neglect is tracked at night.
        visuals?.UpdateVisuals(state);
        return "Marie stares straight ahead. You don't touch her.";
    }

    // ── Night logic ────────────────────────────────────────────────────────────

    protected override void ProcessNightEvent()
    {
        // Track fully-ignored days
        if (!interactedToday)
        {
            daysFullyIgnored++;

            // Faster corruption growth when Marie is left alone.
            state.ModifyCorruption(5);
            Debug.Log($"[Marie] Ignored day #{daysFullyIgnored} — +5 corruption.");
        }
        else
        {
            daysFullyIgnored = 0;
        }

        // ── After 3 consecutive ignored days → extra corruption gain ──
        if (daysFullyIgnored >= 3)
        {
            state.ModifyCorruption(10);
            Debug.Log("[Marie] 3+ ignored days — +10 bonus corruption added.");
        }

        // ── Ribbon tightening visual cue if corruption > 60 ──
        if (state.corruption > 60)
        {
            Debug.Log("[Marie] Corruption > 60 — ribbon tightening.");
        }
    }
}