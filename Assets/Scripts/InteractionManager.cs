using UnityEngine;

/// <summary>
/// Attach to: same GameObject as GameManager or its own object.
/// Assign in Inspector: dollManager, uiManager.
///
/// UIManager's choice buttons call HandleInteraction() with the right parameters.
/// You no longer need a DayEvent.Choice data class — choices are built per day
/// in DayEventManager and fed directly as button labels + lambdas.
/// </summary>
public class InteractionManager : MonoBehaviour
{
    public DollManager dollManager;
    public UIManager uiManager;

    // ── Public entry points (called by UIManager button lambdas) ───────────────

    public void CleanDoll(string dollName)
    {
        var doll = dollManager.GetDollByName(dollName);
        if (doll == null) return;

        string feedback = doll.Clean();
        FinishInteraction(feedback);
    }

    public void BrushHair()
    {
        // Only Elizabeth has brush hair
        string feedback = dollManager.elizabeth.BrushHair();
        FinishInteraction(feedback);
    }

    public void ComfortOliver()
    {
        string feedback = dollManager.oliver.Comfort();
        FinishInteraction(feedback);
    }

    public void GiftOliver(string item)
    {
        string feedback = dollManager.oliver.GiftItem(item);
        FinishInteraction(feedback);
    }

    public void GiftElizabeth(string item)
    {
        // Elizabeth gets +2 mood for any gift
        dollManager.elizabeth.state.ModifyMood(2);
        string feedback = $"Elizabeth receives the {item}. +2 Mood";
        FinishInteraction(feedback);
    }

    public void GiftMarie(string item)
    {
        // Marie gets +2 mood for any gift
        dollManager.marie.state.ModifyMood(2);
        string feedback = $"Marie receives the {item}. +2 Mood";
        FinishInteraction(feedback);
    }

    /// <summary>Player ignores a specific doll.</summary>
    public void IgnoreDoll(string dollName)
    {
        var doll = dollManager.GetDollByName(dollName);
        if (doll == null) return;

        string feedback = doll.Ignore();
        FinishInteraction(feedback);
    }

    /// <summary>
    /// Day 5 special: player tries to remove Marie's ribbon.
    /// </summary>
    public void RemoveMariesRibbon()
    {
        string feedback = dollManager.marie.RemoveRibbon();
        FinishInteraction(feedback);
    }

    /// <summary>
    /// Day 5 special: player leaves ribbon alone.
    /// </summary>
    public void LeaveMariesRibbon()
    {
        string feedback = dollManager.marie.LeaveRibbon();
        FinishInteraction(feedback);
    }

    /// <summary>
    /// Day 8 special: player cleans blood off Elizabeth.
    /// bloodSplashed flag must be set first by DayEventManager.
    /// </summary>
    public void CleanBloodFromElizabeth()
    {
        dollManager.elizabeth.bloodSplashed = false;
        
        // Remove blood sprite
        if (dollManager.elizabeth.visuals != null)
        {
            dollManager.elizabeth.visuals.SetSpriteFlag("hasBlood", false);
            dollManager.elizabeth.visuals.UpdateVisuals(dollManager.elizabeth.state);
        }
        
        string feedback = dollManager.elizabeth.Clean();
        FinishInteraction(feedback);
    }

    /// <summary>
    /// Day 8: player ignores the blood.
    /// </summary>
    public void IgnoreBloodOnElizabeth()
    {
        // Just pass the day — DollBase.NightProcess will set bloodNotCleanedFlag
        string feedback = "You look away. It's probably nothing.";
        FinishInteraction(feedback);
    }

    // ── "Not today" / skip interaction ─────────────────────────────────────────

    /// <summary>
    /// Spends an interaction slot without doing anything to a doll.
    /// </summary>
    public void NotToday()
    {
        FinishInteraction("You leave the dolls alone for now.");
    }

    // ── Internal ───────────────────────────────────────────────────────────────

    private void FinishInteraction(string feedbackText)
    {
        uiManager.ShowMessage(feedbackText);
        uiManager.UpdateStatsDisplay(dollManager.allDolls);
        GameManager.Instance.UseInteraction();
    }
}