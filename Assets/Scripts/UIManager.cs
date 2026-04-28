using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

/// <summary>
/// Attach to: a "UIManager" GameObject in your Canvas.
/// Assign all Text / Button / Panel references in the Inspector.
///
/// Key difference from old version:
///  - SetChoices() takes (label, Action) tuples — no DayEvent.Choice needed.
///  - StartNightSequence() takes a callback so GameManager.StartDay runs
///    AFTER the fade, not via Invoke.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [Header("Text")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI dayText;

    [Header("Stat Display (optional — assign if you have stat bars/labels)")]
    public TextMeshProUGUI elizabethStatsText;
    public TextMeshProUGUI oliverStatsText;
    public TextMeshProUGUI marieStatsText;

    [Header("Choice Buttons (assign up to 4)")]
    public Button[] choiceButtons;     // drag buttons in order; label is child Text

    [Header("Overlays")]
    public GameObject nightOverlay;
    public GameObject fadePanel;
    public GameObject endingPanel;
    public TextMeshProUGUI endingText;

    [Header("Dialogue Sequence")]
    public GameObject dialoguePanel;
    public Button nextButton;

    [Header("Doll Interaction Panel")]
    public GameObject dollInteractionPanel;      // Panel with doll stats + action buttons
    public TextMeshProUGUI dollNameText;          // Doll name display
    public TextMeshProUGUI dollStatsText;         // Doll stats display
    public Image dollSpriteImage;                 // Display doll's default sprite
    public Button[] dollActionButtons;            // up to 4 buttons (Clean/Brush/Gift/Comfort, etc.)
    public Button closeDollPanelButton;           // Close button

    public GameObject interactionHeaderPanel;
    public TextMeshProUGUI interactionHeaderText; // "Interact with dolls today (x/3)"

    [Header("Special Event Panel")]
    public GameObject specialEventPanel;          // Panel for Day 5, 8, 6 events
    public TextMeshProUGUI specialEventTitleText; // Event title
    public TextMeshProUGUI specialEventDescText;  // Event description
    public Button[] specialEventButtons;          // up to 3 buttons for event choices
    public Button closeSpecialEventButton;        // Close button

    private string[] currentLines;
    private int currentLineIndex;
    private Action onDialogueComplete;

    // ── Dialogue Sequence ──────────────────────────────────────────────────────────

    public void StartDialogue(string[] lines, Action onComplete = null)
    {
        currentLines = lines;
        currentLineIndex = 0;
        onDialogueComplete = onComplete;

        // Hide interaction UI during dialogue
        HideInteractionUI();

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(true);
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(NextLine);
        }

        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (currentLineIndex < currentLines.Length && dialogueText != null)
        {
            dialogueText.text = currentLines[currentLineIndex];
        }
    }

    private void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex >= currentLines.Length)
        {
            if (dialoguePanel != null)
                dialoguePanel.SetActive(false);

            if (nextButton != null)
                nextButton.gameObject.SetActive(false);

            onDialogueComplete?.Invoke();
            return;
        }

        ShowCurrentLine();
    }

    /// <summary>
    /// Shows an interaction prompt with remaining interaction slots.
    /// </summary>
    public void ShowInteractionPrompt(int interactionsLeft)
    {
        ShowMessage($"Please choose a doll to interact with ({interactionsLeft}/3)");
    }

    // ── Doll Selection & Interaction Panel ──────────────────────────────────────

    /// <summary>
    /// Starts the doll selection phase. Shows header and enables doll clickability.
    /// </summary>
    public void StartDollSelection(int interactionsLeft)
    {
        if (interactionHeaderPanel != null)
            interactionHeaderPanel.SetActive(true);

        UpdateInteractionHeader(interactionsLeft);

        HideChoices();
        HideDollPanel();
    }

    /// <summary>
    /// Shows the doll interaction panel with stats, sprite, and action buttons.
    /// </summary>
    public void ShowDollPanel(DollBase doll, (string label, System.Action action)[] actions)
    {
        if (dollInteractionPanel != null)
            dollInteractionPanel.SetActive(true);

        if (dollNameText != null)
            dollNameText.text = doll.state.dollName;

        if (dollStatsText != null)
            dollStatsText.text = $"Mood: {doll.state.mood}/100\nCleanliness: {doll.state.cleanliness}/100\nCorruption: {doll.state.corruption}/100";

        // Display the doll's sprite
        if (dollSpriteImage != null && doll.visuals != null)
        {
            var spriteVariants = doll.visuals.GetSpriteVariants();
            if (spriteVariants != null)
            {
                // Use SpriteVariants to get appropriate sprite based on current state
                Sprite displaySprite = spriteVariants.GetAppropriateSprite(doll.state, doll.visuals.GetSpriteState());
                if (displaySprite != null)
                    dollSpriteImage.sprite = displaySprite;
            }
        }

        // Populate action buttons
        for (int i = 0; i < dollActionButtons.Length; i++)
        {
            if (dollActionButtons[i] == null) continue;

            if (i < actions.Length)
            {
                dollActionButtons[i].gameObject.SetActive(true);
                var labelText = dollActionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (labelText != null) labelText.text = actions[i].label;

                dollActionButtons[i].onClick.RemoveAllListeners();
                var action = actions[i].action;  // capture
                dollActionButtons[i].onClick.AddListener(() =>
                {
                    HideDollPanel();
                    action?.Invoke();
                });
            }
            else
            {
                dollActionButtons[i].gameObject.SetActive(false);
            }
        }

        // Setup close button
        if (closeDollPanelButton != null)
        {
            closeDollPanelButton.onClick.RemoveAllListeners();
            closeDollPanelButton.onClick.AddListener(() =>
            {
                HideDollPanel();
                // Return to doll selection
                StartDollSelection(GameManager.Instance.interactionsLeft);
            });
        }
    }

    /// <summary>
    /// Shows the doll interaction panel with stats and action buttons (legacy - for compatibility).
    /// </summary>
    public void ShowDollPanel(string dollName, int mood, int cleanliness, int corruption, (string label, Action action)[] actions)
    {
        if (dollInteractionPanel != null)
            dollInteractionPanel.SetActive(true);

        if (dollNameText != null)
            dollNameText.text = dollName;

        if (dollStatsText != null)
            dollStatsText.text = $"Mood: {mood}/100\nCleanliness: {cleanliness}/100\nCorruption: {corruption}/100";

        // Populate action buttons
        for (int i = 0; i < dollActionButtons.Length; i++)
        {
            if (dollActionButtons[i] == null) continue;

            if (i < actions.Length)
            {
                dollActionButtons[i].gameObject.SetActive(true);
                var labelText = dollActionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (labelText != null) labelText.text = actions[i].label;

                dollActionButtons[i].onClick.RemoveAllListeners();
                var action = actions[i].action;  // capture
                dollActionButtons[i].onClick.AddListener(() =>
                {
                    HideDollPanel();
                    action?.Invoke();
                });
            }
            else
            {
                dollActionButtons[i].gameObject.SetActive(false);
            }
        }

        // Setup close button
        if (closeDollPanelButton != null)
        {
            closeDollPanelButton.onClick.RemoveAllListeners();
            closeDollPanelButton.onClick.AddListener(() =>
            {
                HideDollPanel();
                // Return to doll selection
                StartDollSelection(GameManager.Instance.interactionsLeft);
            });
        }
    }

    /// <summary>
    /// Hides the doll interaction panel (the stats panel with action buttons).
    /// The header text remains visible during doll selection.
    /// </summary>
    public void HideDollPanel()
    {
        if (dollInteractionPanel != null)
            dollInteractionPanel.SetActive(false);
    }

    /// <summary>
    /// Hides all interaction UI (header + panel). Call this when ending doll selection phase.
    /// </summary>
    public void HideInteractionUI()
    {
        HideDollPanel();

        if (interactionHeaderPanel != null)
            interactionHeaderPanel.SetActive(false);
    }

    public void UpdateInteractionHeader(int interactionsLeft)
    {
        if (interactionHeaderText != null)
        {
            interactionHeaderText.text = $"Interact with dolls today ({interactionsLeft}/3 interactions left)";
        }
    }

    /// <summary>
    /// Shows the special event panel for story moments (Day 5, 8, 6).
    /// </summary>
    public void ShowSpecialEventPanel(string title, string description, (string label, Action action)[] choices)
    {
        HideInteractionUI();  // Hide header + panel during story moments
        HideChoices();

        if (specialEventPanel != null)
            specialEventPanel.SetActive(true);

        if (specialEventTitleText != null)
            specialEventTitleText.text = title;

        if (specialEventDescText != null)
            specialEventDescText.text = description;

        // Populate event buttons
        for (int i = 0; i < specialEventButtons.Length; i++)
        {
            if (specialEventButtons[i] == null) continue;

            if (i < choices.Length)
            {
                specialEventButtons[i].gameObject.SetActive(true);
                var labelText = specialEventButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (labelText != null) labelText.text = choices[i].label;

                specialEventButtons[i].onClick.RemoveAllListeners();
                var action = choices[i].action;  // capture
                specialEventButtons[i].onClick.AddListener(() =>
                {
                    HideSpecialEventPanel();
                    action?.Invoke();
                });
            }
            else
            {
                specialEventButtons[i].gameObject.SetActive(false);
            }
        }

        // Setup close button
        if (closeSpecialEventButton != null)
        {
            closeSpecialEventButton.onClick.RemoveAllListeners();
            closeSpecialEventButton.onClick.AddListener(HideSpecialEventPanel);
        }
    }

    /// <summary>
    /// Hides the special event panel.
    /// </summary>
    public void HideSpecialEventPanel()
    {
        if (specialEventPanel != null)
            specialEventPanel.SetActive(false);
    }

    // ── Basic display ───────────────────────────────────────────────────────────

    public void ShowMessage(string msg)
    {
        if (dialogueText != null)
            dialogueText.text = msg;
    }

    public void UpdateDay(int day)
    {
        if (dayText != null)
            dayText.text = "Day " + day;
    }

    /// <summary>
    /// Updates the stat readout labels if you have them in the UI.
    /// Pass dollManager.allDolls.
    /// </summary>
    public void UpdateStatsDisplay(DollBase[] dolls)
    {
        foreach (var doll in dolls)
        {
            string stats = $"Mood: {doll.state.mood}  Clean: {doll.state.cleanliness}  Corrupt: {doll.state.corruption}";
            switch (doll.state.dollName)
            {
                case "Elizabeth": if (elizabethStatsText) elizabethStatsText.text = stats; break;
                case "Oliver": if (oliverStatsText) oliverStatsText.text = stats; break;
                case "Marie": if (marieStatsText) marieStatsText.text = stats; break;
            }
        }
    }

    // ── Choice buttons ──────────────────────────────────────────────────────────

    /// <summary>
    /// Populates choice buttons with labels and click actions.
    /// Pass an array of (label, action) tuples. Unused buttons are hidden.
    /// </summary>
    public void SetChoices((string label, Action action)[] choices)
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (choiceButtons[i] == null) continue;

            if (i < choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);

                // Set label
                var labelText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (labelText != null) labelText.text = choices[i].label;

                // Clear old listeners and set new one
                choiceButtons[i].onClick.RemoveAllListeners();
                var action = choices[i].action;            // capture for lambda
                choiceButtons[i].onClick.AddListener(() => action());
            }
            else
            {
                // Hide buttons that aren't needed this turn
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void HideChoices()
    {
        foreach (var btn in choiceButtons)
            if (btn != null) btn.gameObject.SetActive(false);
    }

    // ── Night sequence ──────────────────────────────────────────────────────────

    /// <summary>
    /// Fades to night, waits, then calls onComplete so GameManager can advance the day.
    /// </summary>
    public void StartNightSequence(Action onComplete)
    {
        StartCoroutine(NightRoutine(onComplete));
    }

    private IEnumerator NightRoutine(Action onComplete)
    {
        // Fade in
        if (fadePanel != null) fadePanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        if (nightOverlay != null) nightOverlay.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        // Fade out
        if (nightOverlay != null) nightOverlay.SetActive(false);
        if (fadePanel != null) fadePanel.SetActive(false);

        yield return new WaitForSeconds(0.8f);  // Extended buffer to prevent scene flash during transition

        onComplete?.Invoke();   // → GameManager.StartDay() runs here
    }

    // ── Endings ─────────────────────────────────────────────────────────────────

    public void ShowEnding(string message)
    {
        HideChoices();

        if (endingPanel != null) endingPanel.SetActive(true);
        if (endingText != null) endingText.text = message;

        // Also put it in dialogue as fallback
        ShowMessage(message);
    }
}