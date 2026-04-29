using System;
using UnityEngine;

/// <summary>
/// Holds the current game state (day, interactions, doll stats).
/// </summary>
[System.Serializable]
public class GameState
{
    public int currentDay;
    public int interactionsLeft;
    public DollStateSnapshot elizabeth;
    public DollStateSnapshot oliver;
    public DollStateSnapshot marie;
}

/// <summary>
/// Snapshot of a single doll's state for saving/loading.
/// </summary>
[System.Serializable]
public class DollStateSnapshot
{
    public string dollName;
    public int mood;
    public int cleanliness;
    public int corruption;
    public bool ribbonRemovedFlag;
    public bool bloodNotCleanedFlag;
    public bool oliverBadEndFlag;

    public DollStateSnapshot(DollState state)
    {
        dollName = state.dollName;
        mood = state.mood;
        cleanliness = state.cleanliness;
        corruption = state.corruption;
        ribbonRemovedFlag = state.ribbonRemovedFlag;
        bloodNotCleanedFlag = state.bloodNotCleanedFlag;
        oliverBadEndFlag = state.oliverBadEndFlag;
    }

    public void ApplyTo(DollState state)
    {
        state.mood = mood;
        state.cleanliness = cleanliness;
        state.corruption = corruption;
        state.ribbonRemovedFlag = ribbonRemovedFlag;
        state.bloodNotCleanedFlag = bloodNotCleanedFlag;
        state.oliverBadEndFlag = oliverBadEndFlag;
    }
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public static event Action<string> OnAchievementUnlocked;

    public AchievementData data = new AchievementData();
    public GameState gameState = new GameState();

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
    /// Save the current game state (day, interactions, doll stats).
    /// Call this when returning to menu or before loading/exiting.
    /// </summary>
    public void SaveGameState(int currentDay, int interactionsLeft, DollBase elizabeth, DollBase oliver, DollBase marie)
    {
        gameState.currentDay = currentDay;
        gameState.interactionsLeft = interactionsLeft;
        gameState.elizabeth = new DollStateSnapshot(elizabeth.state);
        gameState.oliver = new DollStateSnapshot(oliver.state);
        gameState.marie = new DollStateSnapshot(marie.state);

        string json = JsonUtility.ToJson(gameState);
        PlayerPrefs.SetString("GAME_STATE", json);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Load the saved game state.
    /// </summary>
    public void LoadGameState()
    {
        if (PlayerPrefs.HasKey("GAME_STATE"))
        {
            string json = PlayerPrefs.GetString("GAME_STATE");
            gameState = JsonUtility.FromJson<GameState>(json);
        }
    }

    /// <summary>
    /// Check if a game state save exists.
    /// </summary>
    public bool HasGameState()
    {
        return PlayerPrefs.HasKey("GAME_STATE");
    }

    /// <summary>
    /// Clear the saved game state (for New Game).
    /// </summary>
    public void ClearGameState()
    {
        PlayerPrefs.DeleteKey("GAME_STATE");
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Track day reached and save automatically.
    /// </summary>
    public void TrackDayReached(int day)
    {
        bool unlocked = false;

        if (day > data.maxDayReached)
            data.maxDayReached = day;

        switch (day)
        {
            case 1: unlocked = UnlockFlag(data.reachedDay1, value => data.reachedDay1 = value); if (unlocked) NotifyAchievementUnlocked("Day 1 Reached"); break;
            case 2: unlocked = UnlockFlag(data.reachedDay2, value => data.reachedDay2 = value); if (unlocked) NotifyAchievementUnlocked("Day 2 Reached"); break;
            case 3: unlocked = UnlockFlag(data.reachedDay3, value => data.reachedDay3 = value); if (unlocked) NotifyAchievementUnlocked("Day 3 Reached"); break;
            case 4: unlocked = UnlockFlag(data.reachedDay4, value => data.reachedDay4 = value); if (unlocked) NotifyAchievementUnlocked("Day 4 Reached"); break;
            case 5: unlocked = UnlockFlag(data.reachedDay5, value => data.reachedDay5 = value); if (unlocked) NotifyAchievementUnlocked("Day 5 Reached"); break;
            case 6: unlocked = UnlockFlag(data.reachedDay6, value => data.reachedDay6 = value); if (unlocked) NotifyAchievementUnlocked("Day 6 Reached"); break;
            case 7: unlocked = UnlockFlag(data.reachedDay7, value => data.reachedDay7 = value); if (unlocked) NotifyAchievementUnlocked("Day 7 Reached"); break;
            case 8: unlocked = UnlockFlag(data.reachedDay8, value => data.reachedDay8 = value); if (unlocked) NotifyAchievementUnlocked("Day 8 Reached"); break;
            case 9: unlocked = UnlockFlag(data.reachedDay9, value => data.reachedDay9 = value); if (unlocked) NotifyAchievementUnlocked("Day 9 Reached"); break;
            case 10: unlocked = UnlockFlag(data.reachedDay10, value => data.reachedDay10 = value); if (unlocked) NotifyAchievementUnlocked("Day 10 Reached"); break;
        }

        if (unlocked)
            Save();
    }

    /// <summary>
    /// Track when a special event occurs.
    /// </summary>
    public void TrackEvent(string eventName)
    {
        bool unlocked = false;

        switch (eventName)
        {
            case "day2_OliverCried":
                unlocked = UnlockFlag(data.day2_OliverCried, value => data.day2_OliverCried = value);
                if (unlocked) NotifyAchievementUnlocked("Sorrow");
                break;
            case "day3_ElizabethDistorted":
                unlocked = UnlockFlag(data.day3_ElizabethDistorted, value => data.day3_ElizabethDistorted = value);
                if (unlocked) NotifyAchievementUnlocked("Distorted");
                break;
            case "day5_RibbonEvent":
                unlocked = UnlockFlag(data.day5_RibbonEvent, value => data.day5_RibbonEvent = value);
                if (unlocked) NotifyAchievementUnlocked("Discovery");
                break;
            case "day5_RibbonRemoved":
                unlocked = UnlockFlag(data.day5_RibbonRemoved, value => data.day5_RibbonRemoved = value);
                data.ribbonRemoved = true;
                if (unlocked) NotifyAchievementUnlocked("Ribbon Removed");
                break;
            case "day5_RibbonWrapped":
                unlocked = UnlockFlag(data.day5_RibbonWrapped, value => data.day5_RibbonWrapped = value);
                if (unlocked) NotifyAchievementUnlocked("Corruption Spreads");
                break;
            case "day6_DollsCorrupted":
                unlocked = UnlockFlag(data.day6_DollsMoved, value => data.day6_DollsMoved = value);
                if (unlocked) NotifyAchievementUnlocked("Unnatural");
                break;
            case "day8_BloodSplash":
                unlocked = UnlockFlag(data.day8_BloodSplash, value => data.day8_BloodSplash = value);
                if (unlocked) NotifyAchievementUnlocked("Accident");
                break;
            case "day8_BloodIgnored":
                unlocked = UnlockFlag(data.day8_BloodIgnored, value => data.day8_BloodIgnored = value);
                data.ignoredBlood = true;
                if (unlocked) NotifyAchievementUnlocked("Neglect");
                break;
            case "day9_CorruptionIntensified":
                unlocked = UnlockFlag(data.day9_CorruptionIntensified, value => data.day9_CorruptionIntensified = value);
                if (unlocked) NotifyAchievementUnlocked("Chaos");
                break;
            case "elizabethNightmare":
                unlocked = UnlockFlag(data.elizabethNightmare, value => data.elizabethNightmare = value);
                if (unlocked) NotifyAchievementUnlocked("Nightmare");
                break;
        }

        if (unlocked)
            Save();
    }

    /// <summary>
    /// Track when a sprite variant is first shown.
    /// </summary>
    public void TrackSpriteUnlocked(string spriteName)
    {
        bool unlocked = false;

        switch (spriteName)
        {
            case "elizabeth_LongHair":
                unlocked = UnlockFlag(data.elizabeth_LongHairUnlocked, value => data.elizabeth_LongHairUnlocked = value);
                if (unlocked) NotifyAchievementUnlocked("Long Hair");
                break;
            case "elizabeth_DistortedFace":
                unlocked = UnlockFlag(data.elizabeth_DistortedFaceUnlocked, value => data.elizabeth_DistortedFaceUnlocked = value);
                if (unlocked) NotifyAchievementUnlocked("Distorted");
                break;
            case "oliver_Wet":
                unlocked = UnlockFlag(data.oliver_WetSpriteUnlocked, value => data.oliver_WetSpriteUnlocked = value);
                if (unlocked) NotifyAchievementUnlocked("Sorrow");
                break;
            case "marie_WrappedInRibbon":
                unlocked = UnlockFlag(data.marie_RibbonWrappedUnlocked, value => data.marie_RibbonWrappedUnlocked = value);
                if (unlocked) NotifyAchievementUnlocked("Corruption Spreads");
                break;
        }

        if (unlocked)
            Save();
    }

    /// <summary>
    /// Reset all achievements (for debugging).
    /// </summary>
    public void ResetAllAchievements()
    {
        data = new AchievementData();
        PlayerPrefs.DeleteKey("SAVE_DATA");
        PlayerPrefs.Save();
        Debug.Log("All achievements cleared.");
    }
    
    /// <summary>
    /// Track ending reached.
    /// </summary>
    public void TrackEnding(string endingType)
    {
        bool unlocked = false;

        switch (endingType)
        {
            case "good":
                unlocked = UnlockFlag(data.gotGoodEnding, value => data.gotGoodEnding = value);
                if (unlocked) NotifyAchievementUnlocked("Good Ending");
                break;
            case "neutral":
                unlocked = UnlockFlag(data.gotNeutralEnding, value => data.gotNeutralEnding = value);
                if (unlocked) NotifyAchievementUnlocked("Neutral Ending");
                break;
            case "bad_ribbon":
                unlocked = UnlockFlag(data.gotBadEnding_RibbonRemoved, value => data.gotBadEnding_RibbonRemoved = value);
                if (unlocked) NotifyAchievementUnlocked("Ribbon Removed");
                break;
            case "bad_blood":
                unlocked = UnlockFlag(data.gotBadEnding_BloodNotCleaned, value => data.gotBadEnding_BloodNotCleaned = value);
                if (unlocked) NotifyAchievementUnlocked("Blood Neglect");
                break;
            case "bad_oliver":
                unlocked = UnlockFlag(data.gotBadEnding_OliverNoComfort, value => data.gotBadEnding_OliverNoComfort = value);
                if (unlocked) NotifyAchievementUnlocked("Neglected");
                break;
            case "bad_corruption":
                unlocked = UnlockFlag(data.gotBadEnding_HighCorruption, value => data.gotBadEnding_HighCorruption = value);
                if (unlocked) NotifyAchievementUnlocked("Corrupted");
                break;
        }

        if (unlocked)
            Save();
    }

    private bool UnlockFlag(bool currentValue, Action<bool> setter)
    {
        if (currentValue)
            return false;

        setter(true);
        return true;
    }

    private void NotifyAchievementUnlocked(string title)
    {
        OnAchievementUnlocked?.Invoke(title);
    }
}