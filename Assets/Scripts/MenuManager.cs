using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    private void PlayClickSfx()
    {
        if (AudioManager.Instance != null && AudioManager.Instance.click != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
        }
    }

    [Header("UI References")]
    public GameObject quitConfirmationPanel;  // Assign the quit confirmation popup in Inspector
    public GameObject creditsPanel;           // Assign the credits popup in Inspector (optional)
    public UnityEngine.UI.Button continueButton;      // Continue button (will be hidden if no save)

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Start a new game (clears any existing save).
    /// </summary>
    public void StartNewGame()
    {
        PlayClickSfx();

        if (SaveManager.Instance != null)
            SaveManager.Instance.ClearGameState();

        SceneManager.LoadScene("game");
    }

    /// <summary>
    /// Continue from the last saved game.
    /// </summary>
    public void ContinueGame()
    {
        PlayClickSfx();

        SceneManager.LoadScene("game");
    }

    /// <summary>
    /// Load the game scene to start playing (legacy name - use StartNewGame or ContinueGame).
    /// </summary>
    public void GoToGame()
    {
        PlayClickSfx();

        StartNewGame();
    }

    /// <summary>
    /// Load the save/achievements scene.
    /// </summary>
    public void GoToSave()
    {
        PlayClickSfx();

        SceneManager.LoadScene("save");
    }

    /// <summary>
    /// Show the quit confirmation popup (local to boot scene).
    /// </summary>
    public void ShowQuitConfirmation()
    {
        PlayClickSfx();

        if (quitConfirmationPanel != null)
        {
            quitConfirmationPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Hide the quit confirmation popup.
    /// </summary>
    public void HideQuitConfirmation()
    {
        PlayClickSfx();

        if (quitConfirmationPanel != null)
        {
            quitConfirmationPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Actually quit the game.
    /// </summary>
    public void QuitGame()
    {
        PlayClickSfx();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    /// <summary>
    /// Show the credits popup (local to boot scene).
    /// </summary>
    public void ShowCredits()
    {
        PlayClickSfx();

        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Hide the credits popup.
    /// </summary>
    public void HideCredits()
    {
        PlayClickSfx();

        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
    }
}
