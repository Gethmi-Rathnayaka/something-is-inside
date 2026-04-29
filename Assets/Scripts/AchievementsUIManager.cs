using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class AchievementsUIManager : MonoBehaviour
{
    public Transform gridParent;

    public TextMeshProUGUI leftColumnText;
    public TextMeshProUGUI rightColumnText;

    public GameObject confirmationPanel;  
    public Button confirmResetButton;     
    public Button cancelResetButton;      

    void Start()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("AchievementsUIManager: SaveManager not found!");
            return;
        }

        if (gridParent == null)
        {
            Debug.LogError("AchievementsUIManager: gridParent is not assigned!");
            return;
        }

        BuildAchievementText(SaveManager.Instance.data);
    }

    private void BuildAchievementText(AchievementData data)
    {
        var left = new List<string>();
        var right = new List<string>();

        // LEFT SIDE (main achievements)
        AddAchievementLine(left, "Good Ending", data.gotGoodEnding);
        AddAchievementLine(left, "Neutral Ending", data.gotNeutralEnding);
        AddAchievementLine(left, "Ribbon Removed", data.gotBadEnding_RibbonRemoved);
        AddAchievementLine(left, "Blood Neglect", data.gotBadEnding_BloodNotCleaned);
        AddAchievementLine(left, "Neglected", data.gotBadEnding_OliverNoComfort);
        AddAchievementLine(left, "Corrupted", data.gotBadEnding_HighCorruption);

        AddAchievementLine(left, "Day 1 Reached", data.reachedDay1);
        AddAchievementLine(left, "Day 2 Reached", data.reachedDay2);
        AddAchievementLine(left, "Day 3 Reached", data.reachedDay3);
        AddAchievementLine(left, "Day 4 Reached", data.reachedDay4);
        AddAchievementLine(left, "Day 5 Reached", data.reachedDay5);
        AddAchievementLine(left, "Day 6 Reached", data.reachedDay6);
        AddAchievementLine(left, "Day 7 Reached", data.reachedDay7);
        AddAchievementLine(left, "Day 8 Reached", data.reachedDay8);
        AddAchievementLine(left, "Day 9 Reached", data.reachedDay9);
        AddAchievementLine(left, "Day 10 Reached", data.reachedDay10);

        // RIGHT SIDE (story / events)
        AddAchievementLine(right, "Sorrow", data.day2_OliverCried);
        AddAchievementLine(right, "Distorted", data.day3_ElizabethDistorted);
        AddAchievementLine(right, "Discovery", data.day5_RibbonEvent);
        AddAchievementLine(right, "Ribbon Removed", data.day5_RibbonRemoved);
        AddAchievementLine(right, "Corruption Spreads", data.day5_RibbonWrapped);
        AddAchievementLine(right, "Unnatural", data.day6_DollsMoved);
        AddAchievementLine(right, "Long Hair", data.elizabeth_LongHairUnlocked);
        AddAchievementLine(right, "Accident", data.day8_BloodSplash);
        AddAchievementLine(right, "Neglect", data.day8_BloodIgnored);
        AddAchievementLine(right, "Chaos", data.day9_CorruptionIntensified);
        AddAchievementLine(right, "Nightmare", data.elizabethNightmare);

        leftColumnText.text = string.Join("\n", left);
        rightColumnText.text = string.Join("\n", right);
    }

    private void AddAchievementLine(List<string> lines, string title, bool unlocked)
    {
        string color = unlocked ? "#000000" : "#808080";
        lines.Add($"<color={color}>{EscapeRichText(title)}</color>");
    }

    private static string EscapeRichText(string value)
    {
        return value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    }

    public void ClearAchievementsConfirm()
    {
        if (confirmationPanel != null)
            confirmationPanel.SetActive(true);
    }

    public void ConfirmClearAchievements()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.ResetAllAchievements();
            BuildAchievementText(SaveManager.Instance.data);
        }

        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
    }

    public void CancelClearAchievements()
    {
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
    }
    
    public void GoToBootScene()
    {
        SceneManager.LoadScene("Boot");
    }
}
