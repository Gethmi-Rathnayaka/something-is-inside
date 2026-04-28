using UnityEngine;

/// <summary>
/// Handles visual updates for a doll based on its state.
/// Attach this script to the same GameObject as a DollBase child class.
/// </summary>
public class DollVisuals : MonoBehaviour
{
    [Header("Visual References")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Sprite Variants")]
    [SerializeField] private SpriteVariants spriteVariants;

    // Tracks special visual states for this doll
    [HideInInspector] public DollSpriteState spriteState = new DollSpriteState();

    private void OnEnable()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Updates the doll's visuals based on its current state and sprite state.
    /// This is called after interactions or state changes.
    /// </summary>
    public void UpdateVisuals(DollState state)
    {
        if (state == null)
            return;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        // Get appropriate sprite based on state and special flags
        if (spriteVariants != null)
        {
            Sprite newSprite = spriteVariants.GetAppropriateSprite(state, spriteState);
            if (newSprite != null)
            {
                spriteRenderer.sprite = newSprite;
            }
        }

        // You can add more visual updates here in the future:
        // - Color tint based on cleanliness
        // - Animation triggers
        // - Particle effects for corruption
    }

    /// <summary>
    /// Change a specific sprite state flag (like adding blood or wrapping ribbon).
    /// This is called by special event handlers.
    /// </summary>
    public void SetSpriteFlag(string flagName, bool value)
    {
        switch (flagName)
        {
            case "hasBlood":
                spriteState.hasBlood = value;
                break;
            case "hasLongHair":
                spriteState.hasLongHair = value;
                break;
            case "isWrappedInRibbon":
                spriteState.isWrappedInRibbon = value;
                break;
            case "isWet":
                spriteState.isWet = value;
                break;
            case "hasDistortedFace":
                spriteState.hasDistortedFace = value;
                break;
        }
    }

    /// <summary>
    /// Get the current sprite state (for debugging or external checks).
    /// </summary>
    public DollSpriteState GetSpriteState()
    {
        return spriteState;
    }

    /// <summary>
    /// Get the sprite variants for this doll (used by UI to display sprite).
    /// </summary>
    public SpriteVariants GetSpriteVariants()
    {
        return spriteVariants;
    }
}
