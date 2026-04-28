using UnityEngine;

/// <summary>
/// Abstract base class. Attach NOTHING — attach ElizabethLogic / OliverLogic / MarieLogic instead.
/// Each doll GameObject needs: this script's child class + DollVisuals script.
/// </summary>
public abstract class DollBase : MonoBehaviour
{
    [Header("Doll Data")]
    [SerializeField] private DollState _state = new DollState();
    public DollState state => _state;

    // Was this doll interacted with today? Set to true by InteractionManager.
    [HideInInspector] public bool interactedToday = false;

    // Reference set by DollManager on startup
    [HideInInspector] public DollVisuals visuals;

    protected virtual void Awake()
    {
        visuals = GetComponent<DollVisuals>();
    }

    // ── Interaction actions ────────────────────────────────────────────────────
    // InteractionManager calls these directly. They return a feedback string.

    public virtual string Clean()
    {
        state.ModifyCleanliness(30);
        state.ModifyMood(5);
        state.ModifyCorruption(-5);
        interactedToday = true;
        visuals?.UpdateVisuals(state);
        return $"{state.dollName} looks cleaner.";
    }

    public virtual string BrushHair()
    {
        state.ModifyMood(3);
        state.ModifyCorruption(-2);
        interactedToday = true;
        visuals?.UpdateVisuals(state);
        return $"You smooth {state.dollName}'s hair.";
    }

    public virtual string Ignore()
    {
        state.ModifyMood(-10);
        state.consecutiveIgnoreCount++;
        // interactedToday stays false → neglect counter increments at night
        visuals?.UpdateVisuals(state);
        return $"You look away from {state.dollName}.";
    }

    // Override in subclasses for doll-specific actions
    public virtual string GiftItem(string item) => "Nothing happens.";
    public virtual string Comfort() => "Nothing happens.";

    // ── Night processing ───────────────────────────────────────────────────────
    // DollManager calls this at end of each day.

    public void NightProcess()
    {
        if (!interactedToday)
            state.neglectCounter++;
        else
            state.neglectCounter = 0;       // reset streak if cared for today

        state.ApplyDailyDecay(this is ElizabethLogic);

        ProcessNightEvent();                // doll-specific checks

        visuals?.UpdateVisuals(state);

        interactedToday = false;            // reset for next day
    }

    /// <summary>Override this to add doll-specific night logic (nightmare flag, bad-end checks, etc.)</summary>
    protected abstract void ProcessNightEvent();

    // ── Shared helpers ─────────────────────────────────────────────────────────

    protected void ApplyCareBonus()
    {
        // Any positive interaction grants -5 corruption (floor 0)
        state.ModifyCorruption(-5);
    }
}