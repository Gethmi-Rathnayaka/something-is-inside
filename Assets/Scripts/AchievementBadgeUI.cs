using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementBadgeUI : MonoBehaviour
{
    public Image badgeImage;
    public GameObject lockedOverlay;

    private string title;
    private string description;
    private bool unlocked;

    public void Setup(Sprite icon, bool isUnlocked, string title, string desc)
    {
        badgeImage.sprite = icon;
        unlocked = isUnlocked;
        this.title = title;
        this.description = desc;

        lockedOverlay.SetActive(!isUnlocked);
    }

    public void OnClick()
    {
        var popupUI = Object.FindFirstObjectByType<AchievementPopupUI>();
        if (popupUI != null)
        {
            popupUI.Show(title, description, badgeImage.sprite, unlocked);
        }
        else
        {
            Debug.LogError("AchievementBadgeUI: AchievementPopupUI not found in scene!");
        }
    }
}