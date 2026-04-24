using UnityEngine;

/// <summary>
/// Pure data container for a doll's stats.
/// This is a serializable data class, NOT a MonoBehaviour.
/// DollBase holds an instance of this in its 'state' field.
/// </summary>
[System.Serializable]
public class DollState
{
    // Identity
    public string dollName;

    // Stats (0-100)
    public int mood;
    public int cleanliness;
    public int corruption;

    // Tracking
    public int neglectCounter;          // days in a row NOT interacted
    public int consecutiveIgnoreCount;  // used by Oliver / Marie

    // Bad End Flags
    public bool ribbonRemovedFlag;      // Marie only
    public bool bloodNotCleanedFlag;    // Elizabeth only (Day 8)
    public bool oliverBadEndFlag;       // Oliver: 3 consecutive days no comfort

    // ── Stat modifiers ─────────────────────────────────────────────────────────

    public void ModifyMood(int amount)
    {
        mood = Mathf.Clamp(mood + amount, 0, 100);
    }

    public void ModifyCorruption(int amount)
    {
        corruption = Mathf.Clamp(corruption + amount, 0, 100);
    }

    public void ModifyCleanliness(int amount)
    {
        cleanliness = Mathf.Clamp(cleanliness + amount, 0, 100);
    }

    // ── End-of-day passive decay ────────────────────────────────────────────────
    // Call this once per day from DollBase.ProcessNightEvent()

    /// <param name="isElizabeth">Elizabeth loses 20 cleanliness instead of 10</param>
    public void ApplyDailyDecay(bool isElizabeth = false)
    {
        int cleanLoss = isElizabeth ? 20 : 10;
        ModifyCleanliness(-cleanLoss);

        // Corruption escalation
        bool moodLow = mood < 30;
        bool cleanLow = cleanliness < 30;

        if (moodLow && cleanLow)
            ModifyCorruption(20);
        else if (moodLow || cleanLow)
            ModifyCorruption(10);

        // Positive care bonus (handled in interaction, but also prevents passive gain)
        // We do NOT add -5 here; that's handled when an interaction is performed.
    }

    // ── Quick read-only helpers ─────────────────────────────────────────────────

    public bool IsMoodLow => mood < 30;
    public bool IsCleanLow => cleanliness < 30;
    public bool IsCorruptHigh => corruption > 60;
}