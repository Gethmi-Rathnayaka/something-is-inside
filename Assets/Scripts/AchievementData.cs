using System;

[Serializable]
public class AchievementData
{
    // ── Endings ────────────────────────────────────────────────────────────
    public bool gotGoodEnding;
    public bool gotNeutralEnding;
    public bool gotBadEnding_RibbonRemoved;      // Marie's ribbon removed on Day 5
    public bool gotBadEnding_BloodNotCleaned;    // Elizabeth's blood not cleaned on Day 8
    public bool gotBadEnding_OliverNoComfort;    // Oliver not comforted for 3 days
    public bool gotBadEnding_HighCorruption;     // 2+ dolls corruption >= 70

    // ── Days Reached ────────────────────────────────────────────────────────
    public int maxDayReached;
    public bool reachedDay1;
    public bool reachedDay2;
    public bool reachedDay3;
    public bool reachedDay4;
    public bool reachedDay5;
    public bool reachedDay6;
    public bool reachedDay7;
    public bool reachedDay8;
    public bool reachedDay9;
    public bool reachedDay10;

    // ── Special Events ─────────────────────────────────────────────────────
    public bool day2_OliverCried;               // Oliver's mood < 50
    public bool day3_ElizabethDistorted;        // Elizabeth's mood < 50 & distorted face shown
    public bool day5_RibbonEvent;               // Marie's ribbon inspection
    public bool day5_RibbonRemoved;             // Player removed the ribbon
    public bool day5_RibbonWrapped;             // Ribbon wrapped around Marie (corruption > 70)
    public bool day6_DollsMoved;                // 2+ dolls corruption > 50 & dolls switched places
    public bool day8_BloodSplash;               // Elizabeth's blood event triggered
    public bool day8_BloodIgnored;              // Player ignored the blood
    public bool day9_CorruptionIntensified;     // Average corruption > 50

    // ── Doll Specific Flags ────────────────────────────────────────────────
    public bool elizabeth_LongHairUnlocked;     // Long hair sprite shown (Day 7+)
    public bool elizabeth_DistortedFaceUnlocked;// Distorted face sprite shown (Day 3+)
    public bool oliver_WetSpriteUnlocked;       // Wet sprite shown (Day 2+)
    public bool marie_RibbonWrappedUnlocked;    // Wrapped in ribbon sprite shown

    // ── Flags & Actions ────────────────────────────────────────────────────
    public bool ribbonRemoved;                  // Marie's ribbon removed
    public bool ignoredBlood;                   // Blood not cleaned on Day 8
    public bool elizabethNightmare;             // Nightmare flag triggered (2 days uncleaned)

    // ── Reset function ────────────────────────────────────────────────────
    public void ResetForNewGame()
    {
        // Keep endings for UI display, but reset event tracking for new playthrough
        maxDayReached = 0;
        day2_OliverCried = false;
        day3_ElizabethDistorted = false;
        day5_RibbonEvent = false;
        day5_RibbonRemoved = false;
        day5_RibbonWrapped = false;
        day6_DollsMoved = false;
        day8_BloodSplash = false;
        day8_BloodIgnored = false;
        day9_CorruptionIntensified = false;
    }
}