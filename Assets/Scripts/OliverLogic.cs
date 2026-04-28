using UnityEngine;

/// <summary>
/// Attach to: Oliver (Weeping Doll) GameObject.
/// Also needs: DollVisuals on the same GameObject.
/// </summary>
public class OliverLogic : DollBase
{
    // Correct gift items that give +20 mood
    private readonly string[] correctGifts = { "ribbon", "clover" };

    // Tracks consecutive days without Comfort action
    private int daysWithoutComfort = 0;

    // ── Interaction overrides ──────────────────────────────────────────────────

    public override string Comfort()
    {
        daysWithoutComfort = 0;             // reset streak
        state.ModifyMood(15);
        state.ModifyCorruption(-10);
        ApplyCareBonus();
        interactedToday = true;
        visuals?.UpdateVisuals(state);
        return "Oliver's trembling slows. He clutches the comfort quietly.";
    }

    public override string Clean()
    {
        // Remove wet sprite when cleaned
        if (visuals != null)
            visuals.SetSpriteFlag("isWet", false);

        state.ModifyCleanliness(30);
        state.ModifyMood(5);
        state.ModifyCorruption(-5);
        ApplyCareBonus();
        interactedToday = true;
        visuals?.UpdateVisuals(state);
        return "Oliver looks cleaner. His tears have dried.";
    }

    public override string GiftItem(string item)
    {
        bool correct = System.Array.Exists(correctGifts,
            g => g.Equals(item, System.StringComparison.OrdinalIgnoreCase));

        if (correct)
        {
            state.ModifyMood(20);
            ApplyCareBonus();
            interactedToday = true;
            visuals?.UpdateVisuals(state);
            return $"Oliver holds the {item} tightly. His tears stop for a moment.";
        }
        else
        {
            state.ModifyMood(-10);
            interactedToday = true;         // still counts as interaction
            visuals?.UpdateVisuals(state);
            return $"Oliver recoils from the {item}. That wasn't right.";
        }
    }

    public override string Ignore()
    {
        state.ModifyMood(-10);
        daysWithoutComfort++;
        visuals?.UpdateVisuals(state);
        return "Oliver's face is wet again. You say nothing.";
    }

    // ── Night logic ────────────────────────────────────────────────────────────

    protected override void ProcessNightEvent()
    {
        // Track if player didn't use Comfort today
        // Comfort() resets counter to 0, so if counter is still > 0, player didn't comfort
        if (interactedToday)
        {
            // Player did something with Oliver today
            // Comfort() already reset counter, so if daysWithoutComfort > 0, player used wrong action
            if (daysWithoutComfort > 0)
                daysWithoutComfort++;  // wrong action doesn't reset the streak
            // else: Comfort was used, counter stays 0
        }
        else
        {
            // Player didn't interact with Oliver at all today
            daysWithoutComfort++;
        }

        // ── 3 consecutive days no comfort → BAD END FLAG ──
        if (daysWithoutComfort >= 3)
        {
            state.oliverBadEndFlag = true;
            Debug.Log("[Oliver] 3 days no comfort — BAD END FLAG.");
        }
    }
}