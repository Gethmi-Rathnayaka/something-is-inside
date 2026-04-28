using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class AchievementsUIManager : MonoBehaviour
{
    public Transform gridParent;
    public GameObject badgePrefab;

    // Ending icons
    public Sprite goodEndingIcon;
    public Sprite neutralEndingIcon;
    public Sprite badEndingRibbonIcon;
    public Sprite badBloodIcon;
    public Sprite badOliverIcon;
    public Sprite badCorruptionIcon;

    // Event icons
    public Sprite oliverCriedIcon;
    public Sprite elizabethDistortedIcon;
    public Sprite ribbonEventIcon;
    public Sprite ribbonWrappedIcon;
    public Sprite dollsMovedIcon;
    public Sprite bloodSplashIcon;
    public Sprite bloodIgnoredIcon;
    public Sprite corruptionIcon;

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

        if (badgePrefab == null)
        {
            Debug.LogError("AchievementsUIManager: badgePrefab is not assigned!");
            return;
        }

        var data = SaveManager.Instance.data;

        // ── Endings ────────────────────────────────────────────────────────
        CreateBadge(goodEndingIcon, data.gotGoodEnding, "Good Ending", "Keep all dolls stable until Day 10.");
        CreateBadge(neutralEndingIcon, data.gotNeutralEnding, "Neutral Ending", "Reach Day 10 without a bad ending.");
        CreateBadge(badEndingRibbonIcon, data.gotBadEnding_RibbonRemoved, "Ribbon Removed", "Remove Marie's ribbon before Day 10.");
        CreateBadge(badBloodIcon, data.gotBadEnding_BloodNotCleaned, "Blood Neglect", "Do not clean Elizabeth's blood.");
        CreateBadge(badOliverIcon, data.gotBadEnding_OliverNoComfort, "Neglected", "Ignore Oliver for 3 days.");
        CreateBadge(badCorruptionIcon, data.gotBadEnding_HighCorruption, "Corrupted", "Let 2+ dolls reach 70% corruption.");

        // ── Special Events ─────────────────────────────────────────────────
        CreateBadge(oliverCriedIcon, data.day2_OliverCried, "Sorrow", "Make Oliver's mood drop below 50.");
        CreateBadge(elizabethDistortedIcon, data.day3_ElizabethDistorted, "Distorted", "Let Elizabeth's mood drop below 50.");
        CreateBadge(ribbonEventIcon, data.day5_RibbonEvent, "Discovery", "Inspect Marie's ribbon.");
        CreateBadge(ribbonWrappedIcon, data.day5_RibbonWrapped, "Corruption Spreads", "Ribbon wraps around Marie's body.");
        CreateBadge(dollsMovedIcon, data.day6_DollsMoved, "Unnatural", "Dolls switch places on their own.");
        CreateBadge(bloodSplashIcon, data.day8_BloodSplash, "Accident", "Elizabeth's blood spills.");
        CreateBadge(bloodIgnoredIcon, data.day8_BloodIgnored, "Neglect", "Ignore the blood for 2 days.");
        CreateBadge(corruptionIcon, data.day9_CorruptionIntensified, "Chaos", "Average corruption exceeds 50%.");
    }

    void CreateBadge(Sprite icon, bool unlocked, string title, string desc)
    {
        if (icon == null)
        {
            Debug.LogWarning($"AchievementsUIManager: Icon for '{title}' is not assigned!");
            return;
        }

        GameObject obj = Instantiate(badgePrefab, gridParent);
        var badge = obj.GetComponent<AchievementBadgeUI>();

        if (badge == null)
        {
            Debug.LogError("AchievementsUIManager: badgePrefab does not have AchievementBadgeUI component!");
            Destroy(obj);
            return;
        }

        badge.Setup(icon, unlocked, title, desc);
    }

    public void GoToBootScene()
    {
        SceneManager.LoadScene("Boot");
    }
}