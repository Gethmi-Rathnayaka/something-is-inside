using UnityEngine;

/// <summary>
/// Handles player interactions with dolls.
/// Called when a player selects a choice (e.g., "Clean Angry Doll", "Comfort Weeping Doll").
/// </summary>
public class InteractionManager : MonoBehaviour
{
    public DollManager dollManager;
    public DayEventManager dayEventManager;
    public UIManager uiManager;

    // References to actual doll GameObjects with DollBase scripts
    public DollBase angryDoll;
    public DollBase weepingDoll;
    public DollBase ribbonDoll;

    /// <summary>
    /// Called when player makes a choice during the day.
    /// </summary>
    public void HandleChoice(int choiceIndex)
    {
        DayEvent dayEvent = dayEventManager.GetCurrentDayEvent();
        if (dayEvent == null) return;

        DayEvent.Choice choice = dayEvent.choices[choiceIndex];

        // Determine which doll was affected
        DollBase affectedDoll = GetDollByName(choice.affectedDoll);

        // Apply the choice's effect
        ApplyChoiceEffect(choice, affectedDoll);

        // Notify GameManager that an interaction happened
        GameManager.Instance.UseInteraction(affectedDoll);

        // Show feedback
        if (uiManager != null)
            uiManager.ShowMessage(choice.choiceText);
    }

    private void ApplyChoiceEffect(DayEvent.Choice choice, DollBase affectedDoll)
    {
        if (affectedDoll == null) return;

        // Apply mood/corruption changes based on choice
        if (choice.moodImpact != 0)
        {
            affectedDoll.state.ModifyMood(choice.moodImpact);
        }

        // Apply visual effects based on the choice
        // These directly affect which sprite shows
        switch (choice.visualEffect)
        {
            case "clean":
                affectedDoll.state.ModifyMood(10);
                // Sprite will update to show clean doll
                break;
            case "comforted_ribbon":
            case "comforted_clover":
            case "comforted":
                affectedDoll.state.ModifyMood(15);
                break;
            case "ignored_crying":
            case "ignored":
            case "fear_grow":
                affectedDoll.state.ModifyMood(-10);
                break;
            case "ribbon_removed":
                // BAD CHOICE - corrupts the doll severely
                affectedDoll.state.corruption += 50;
                affectedDoll.state.ModifyMood(-20);
                break;
            case "ribbon_cleaned":
                affectedDoll.state.corruption = Mathf.Max(0, affectedDoll.state.corruption - 10);
                affectedDoll.state.ModifyMood(5);
                break;
            case "brushed":
                affectedDoll.state.ModifyMood(8);
                break;
            case "dress_repaired":
                affectedDoll.state.ModifyMood(10);
                break;
            case "separated":
                // Separating all dolls has consequences
                affectedDoll.state.ModifyMood(-15);
                break;
            case "played_with":
                affectedDoll.state.ModifyMood(12);
                break;
            case "repositioned":
                affectedDoll.state.ModifyMood(-5); // Doll doesn't like being moved
                break;
            default:
                // No special effect
                break;
        }

        // Update the visual representation
        affectedDoll.state.UpdateVisuals();
    }

    private DollBase GetDollByName(string dollName)
    {
        switch (dollName)
        {
            case "Angry":
                return angryDoll;
            case "Weeping":
                return weepingDoll;
            case "Ribbon":
                return ribbonDoll;
            default:
                return null;
        }
    }
}
