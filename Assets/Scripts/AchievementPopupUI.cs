using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementPopupUI : MonoBehaviour
{
    public GameObject panel;
    public Image badgeImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;

    private void PlayClickSfx()
    {
        if (AudioManager.Instance != null && AudioManager.Instance.click != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
        }
    }

    public void Show(string title, string desc, Sprite icon, bool unlocked)
    {
        panel.SetActive(true);

        badgeImage.sprite = icon;
        titleText.text = unlocked ? title : "???";
        descText.text = unlocked ? desc : "Unlock this achievement to reveal details.";
    }

    public void Close()
    {
        PlayClickSfx();
        panel.SetActive(false);
    }
}