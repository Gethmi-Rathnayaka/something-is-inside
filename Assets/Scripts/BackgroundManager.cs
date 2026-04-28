using UnityEngine;

/// <summary>
/// Manages background/environment sprite changes.
/// Attach to the background GameObject or a manager.
/// </summary>
public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;

    [Header("Background Sprites")]
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
    [SerializeField] private Sprite normalBackground;
    [SerializeField] private Sprite creekyBackground;      // Darker, more ominous
    [SerializeField] private Sprite bloodStainedBackground; // For very bad end
    [SerializeField] private Sprite finalDayBackground;     // Day 10 special

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        if (backgroundSpriteRenderer == null)
            backgroundSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Switch to normal background.
    /// </summary>
    public void SetNormalBackground()
    {
        if (backgroundSpriteRenderer != null && normalBackground != null)
            backgroundSpriteRenderer.sprite = normalBackground;
    }

    /// <summary>
    /// Switch to creepy background (darker, unsettling).
    /// Call this when corruption is high or bad events are accumulating.
    /// </summary>
    public void SetCreekyBackground()
    {
        if (backgroundSpriteRenderer != null && creekyBackground != null)
            backgroundSpriteRenderer.sprite = creekyBackground;
    }

    /// <summary>
    /// Switch to blood-stained background (worst case).
    /// </summary>
    public void SetBloodStainedBackground()
    {
        if (backgroundSpriteRenderer != null && bloodStainedBackground != null)
            backgroundSpriteRenderer.sprite = bloodStainedBackground;
    }

    /// <summary>
    /// Switch to Day 10 special background.
    /// </summary>
    public void SetFinalDayBackground()
    {
        if (backgroundSpriteRenderer != null && finalDayBackground != null)
            backgroundSpriteRenderer.sprite = finalDayBackground;
    }

    /// <summary>
    /// Get the appropriate background based on overall corruption level.
    /// Call this during day transitions to auto-update atmosphere.
    /// </summary>
    public void UpdateBackgroundBasedOnCorruption(DollManager dollManager)
    {
        if (dollManager == null)
            return;

        float avgCorruption = (dollManager.elizabeth.state.corruption +
                              dollManager.oliver.state.corruption +
                              dollManager.marie.state.corruption) / 3f;

        if (avgCorruption > 75)
            SetBloodStainedBackground();
        else if (avgCorruption > 55)
            SetCreekyBackground();
        else
            SetNormalBackground();
    }
}
