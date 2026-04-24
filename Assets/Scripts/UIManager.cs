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
    public TextMeshProUGUI       endingText;

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
                case "Oliver":    if (oliverStatsText)    oliverStatsText.text    = stats; break;
                case "Marie":     if (marieStatsText)     marieStatsText.text     = stats; break;
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
        if (fadePanel    != null) fadePanel.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        onComplete?.Invoke();   // → GameManager.StartDay() runs here
    }

    // ── Endings ─────────────────────────────────────────────────────────────────

    public void ShowEnding(string message)
    {
        HideChoices();

        if (endingPanel != null) endingPanel.SetActive(true);
        if (endingText  != null) endingText.text = message;

        // Also put it in dialogue as fallback
        ShowMessage(message);
    }
}