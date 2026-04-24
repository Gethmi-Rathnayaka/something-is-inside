using UnityEngine;

/// <summary>
/// Handles visual updates for a doll based on its state.
/// Attach this script to the same GameObject as a DollBase child class.
/// </summary>
public class DollVisuals : MonoBehaviour
{
    [Header("Visual References")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    // You can add more visual references here (animations, particle effects, etc.)

    private void OnEnable()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Updates the doll's visuals based on its current state.
    /// </summary>
    public void UpdateVisuals(DollState state)
    {
        if (state == null)
            return;

        // TODO: Implement visual updates based on state
        // Examples:
        // - Change sprite based on mood/corruption
        // - Adjust color tint based on cleanliness
        // - Play animations based on state changes
        // - Update UI elements reflecting doll condition

        Debug.Log($"Updating visuals for {state.dollName}: Mood={state.mood}, Corruption={state.corruption}");
    }
}
