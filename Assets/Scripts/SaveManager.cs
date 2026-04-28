using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public AchievementData data = new AchievementData();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Load();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SAVE_DATA", json);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("SAVE_DATA"))
        {
            string json = PlayerPrefs.GetString("SAVE_DATA");
            data = JsonUtility.FromJson<AchievementData>(json);
        }
    }

    /// <summary>
    /// Track day reached and save automatically.
    /// </summary>
    public void TrackDayReached(int day)
    {
        if (day > data.maxDayReached)
            data.maxDayReached = day;

        switch (day)
        {
            case 1: data.reachedDay1 = true; break;
            case 2: data.reachedDay2 = true; break;
            case 3: data.reachedDay3 = true; break;
            case 4: data.reachedDay4 = true; break;
            case 5: data.reachedDay5 = true; break;
            case 6: data.reachedDay6 = true; break;
            case 7: data.reachedDay7 = true; break;
            case 8: data.reachedDay8 = true; break;
            case 9: data.reachedDay9 = true; break;
            case 10: data.reachedDay10 = true; break;
        }

        Save();
    }

    /// <summary>
    /// Track when a special event occurs.
    /// </summary>
    public void TrackEvent(string eventName)
    {
        switch (eventName)
        {
            case "day2_OliverCried":
                data.day2_OliverCried = true;
                break;
            case "day3_ElizabethDistorted":
                data.day3_ElizabethDistorted = true;
                break;
            case "day5_RibbonEvent":
                data.day5_RibbonEvent = true;
                break;
            case "day5_RibbonRemoved":
                data.day5_RibbonRemoved = true;
                data.ribbonRemoved = true;
                break;
            case "day5_RibbonWrapped":
                data.day5_RibbonWrapped = true;
                break;
            case "day6_DollsMoved":
                data.day6_DollsMoved = true;
                break;
            case "day8_BloodSplash":
                data.day8_BloodSplash = true;
                break;
            case "day8_BloodIgnored":
                data.day8_BloodIgnored = true;
                data.ignoredBlood = true;
                break;
            case "day9_CorruptionIntensified":
                data.day9_CorruptionIntensified = true;
                break;
            case "elizabethNightmare":
                data.elizabethNightmare = true;
                break;
        }

        Save();
    }

    /// <summary>
    /// Track when a sprite variant is first shown.
    /// </summary>
    public void TrackSpriteUnlocked(string spriteName)
    {
        switch (spriteName)
        {
            case "elizabeth_LongHair":
                data.elizabeth_LongHairUnlocked = true;
                break;
            case "elizabeth_DistortedFace":
                data.elizabeth_DistortedFaceUnlocked = true;
                break;
            case "oliver_Wet":
                data.oliver_WetSpriteUnlocked = true;
                break;
            case "marie_WrappedInRibbon":
                data.marie_RibbonWrappedUnlocked = true;
                break;
        }

        Save();
    }

    /// <summary>
    /// Track ending reached.
    /// </summary>
    public void TrackEnding(string endingType)
    {
        switch (endingType)
        {
            case "good":
                data.gotGoodEnding = true;
                break;
            case "neutral":
                data.gotNeutralEnding = true;
                break;
            case "bad_ribbon":
                data.gotBadEnding_RibbonRemoved = true;
                break;
            case "bad_blood":
                data.gotBadEnding_BloodNotCleaned = true;
                break;
            case "bad_oliver":
                data.gotBadEnding_OliverNoComfort = true;
                break;
            case "bad_corruption":
                data.gotBadEnding_HighCorruption = true;
                break;
        }

        Save();
    }
}