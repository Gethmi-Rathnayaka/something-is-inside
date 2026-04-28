using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages screen effects like blood splashes, flashes, vignettes, etc.
/// Attach to a Canvas or persistent UI GameObject.
/// </summary>
public class ScreenEffectsManager : MonoBehaviour
{
    public static ScreenEffectsManager Instance;

    [Header("Screen Effect References")]
    [SerializeField] private Image screenOverlay;      // Full-screen overlay (assign a panel with Image)
    [SerializeField] private CanvasGroup overlayGroup; // For fading effects

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
        if (screenOverlay == null)
            screenOverlay = GetComponent<Image>();
        if (overlayGroup == null)
            overlayGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Play a blood splash effect (red flash + fade).
    /// </summary>
    public void PlayBloodSplashEffect(float duration = 2f)
    {
        if (screenOverlay == null) return;

        StartCoroutine(BloodSplashCoroutine(duration));
    }

    private IEnumerator BloodSplashCoroutine(float duration)
    {
        // Flash red
        screenOverlay.color = new Color(1f, 0f, 0f, 0.3f);
        screenOverlay.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        // Fade to transparent
        float elapsed = 0f;
        while (elapsed < duration - 0.3f)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0.3f, 0f, elapsed / (duration - 0.3f));
            screenOverlay.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }

        screenOverlay.gameObject.SetActive(false);
    }

    /// <summary>
    /// Play a vignette/darkness effect (creepy atmosphere).
    /// </summary>
    public void PlayVignetteEffect(float intensity = 0.4f, float duration = 1f)
    {
        if (screenOverlay == null) return;

        StartCoroutine(VignetteCoroutine(intensity, duration));
    }

    private IEnumerator VignetteCoroutine(float intensity, float duration)
    {
        screenOverlay.color = new Color(0f, 0f, 0f, 0f);
        screenOverlay.gameObject.SetActive(true);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, intensity, elapsed / duration);
            screenOverlay.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        // Fade back out
        elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(intensity, 0f, elapsed / 1f);
            screenOverlay.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        screenOverlay.gameObject.SetActive(false);
    }

    /// <summary>
    /// Screen flash (white or colored).
    /// </summary>
    public void PlayFlash(Color color, float duration = 0.5f)
    {
        if (screenOverlay == null) return;

        StartCoroutine(FlashCoroutine(color, duration));
    }

    private IEnumerator FlashCoroutine(Color color, float duration)
    {
        screenOverlay.color = new Color(color.r, color.g, color.b, 0.6f);
        screenOverlay.gameObject.SetActive(true);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0.6f, 0f, elapsed / duration);
            screenOverlay.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        screenOverlay.gameObject.SetActive(false);
    }
}
