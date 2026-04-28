using UnityEngine;

/// <summary>
/// Holds sprite variants for a single doll's different visual states.
/// Assign sprites in the Inspector for each state you want to support.
/// </summary>
[System.Serializable]
public class SpriteVariants
{
    [Header("Base Sprites")]
    [SerializeField] public Sprite defaultSprite;
    [SerializeField] public Sprite sadSprite;          // Low mood
    [SerializeField] public Sprite corruptedSprite;    // High corruption
    [SerializeField] public Sprite dirtySprite;        // Low cleanliness

    [Header("Special State Sprites (Optional)")]
    [SerializeField] public Sprite withBloodSprite;           // Elizabeth - blood on dress
    [SerializeField] public Sprite longHairSprite;            // Elizabeth - longer hair
    [SerializeField] public Sprite wrappedInRibbonSprite;     // Marie - ribbon wrapped around her
    [SerializeField] public Sprite wetSprite;                 // Oliver - wet patches from crying
    [SerializeField] public Sprite distortedFaceSprite;       // Elizabeth - distorted smile

    /// <summary>
    /// Get the appropriate sprite based on doll state and special flags.
    /// Priority: special states first, then mood/corruption/cleanliness.
    /// </summary>
    public Sprite GetAppropriateSprite(DollState state, DollSpriteState spriteState)
    {
        // Special states have highest priority
        if (spriteState.hasBlood && withBloodSprite != null)
            return withBloodSprite;

        if (spriteState.hasLongHair && longHairSprite != null)
            return longHairSprite;

        if (spriteState.isWrappedInRibbon && wrappedInRibbonSprite != null)
            return wrappedInRibbonSprite;

        if (spriteState.isWet && wetSprite != null)
            return wetSprite;

        if (spriteState.hasDistortedFace && distortedFaceSprite != null)
            return distortedFaceSprite;

        // Fall back to state-based sprites
        if (state.corruption > 60 && corruptedSprite != null)
            return corruptedSprite;

        if (state.mood < 30 && sadSprite != null)
            return sadSprite;

        if (state.cleanliness < 30 && dirtySprite != null)
            return dirtySprite;

        // Default
        return defaultSprite != null ? defaultSprite : null;
    }
}

/// <summary>
/// Tracks special visual states for a doll beyond what DollState tracks.
/// This is attached to a doll and modified by events/interactions.
/// </summary>
[System.Serializable]
public class DollSpriteState
{
    public bool hasBlood = false;           // Elizabeth on Day 8
    public bool hasLongHair = false;        // Elizabeth on Day 7+
    public bool isWrappedInRibbon = false;  // Marie when corruption is very high
    public bool isWet = false;              // Oliver when he's been crying
    public bool hasDistortedFace = false;   // Elizabeth when mood is very low

    /// <summary>
    /// Quick check: is this doll in any special visual state?
    /// </summary>
    public bool HasAnySpecialState => hasBlood || hasLongHair || isWrappedInRibbon ||
                                     isWet || hasDistortedFace;

    /// <summary>
    /// Clear all special states (useful for reset/cleanup).
    /// </summary>
    public void ClearAllStates()
    {
        hasBlood = false;
        hasLongHair = false;
        isWrappedInRibbon = false;
        isWet = false;
        hasDistortedFace = false;
    }
}
