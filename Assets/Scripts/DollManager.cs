using UnityEngine;

public class DollManager : MonoBehaviour
{
    public DollBase[] allDolls; // Drag all doll gameobjects here in Inspector

    public UIManager uiManager;

    public void EvaluateDolls(DollBase targetDoll = null)
    {
        // First pass: Apply neglect penalty to dolls that WEREN'T interacted with
        if (targetDoll != null)
        {
            foreach (DollBase doll in allDolls)
            {
                if (doll != targetDoll)
                {
                    // This doll was neglected tonight
                    doll.state.neglectCounter++;
                    uiManager.ShowMessage(doll.state.dollName + " feels abandoned...");
                }
            }
        }

        // Second pass: Process each doll's unique night event logic
        foreach (DollBase doll in allDolls)
        {
            // Increment neglect for all dolls at start of night
            doll.state.neglectCounter++;

            // Trigger the unique logic for that specific doll type
            doll.ProcessNightEvent();

            // Finally, update the visual representation
            doll.state.UpdateVisuals();
        }
    }
}
