using UnityEngine;

/// <summary>
/// Attach to: same GameObject as GameManager, or its own "DollManager" GameObject.
/// Assign in Inspector: elizabeth, oliver, marie (drag doll GameObjects in).
/// </summary>
public class DollManager : MonoBehaviour
{
    [Header("Dolls (assign in Inspector)")]
    public ElizabethLogic elizabeth;
    public OliverLogic oliver;
    public MarieLogic marie;

    // Convenience array — populated automatically in Awake
    [HideInInspector] public DollBase[] allDolls;

    private void Awake()
    {
        allDolls = new DollBase[] { elizabeth, oliver, marie };
    }

    // ── Called once at game start ───────────────────────────────────────────────

    public void InitializeDolls()
    {
        // Elizabeth starting stats
        elizabeth.state.dollName = "Elizabeth";
        elizabeth.state.cleanliness = 80;
        elizabeth.state.mood = 50;
        elizabeth.state.corruption = 40;

        // Oliver starting stats
        oliver.state.dollName = "Oliver";
        oliver.state.cleanliness = 80;
        oliver.state.mood = 30;
        oliver.state.corruption = 10;

        // Marie starting stats
        marie.state.dollName = "Marie";
        marie.state.cleanliness = 100;
        marie.state.mood = 90;
        marie.state.corruption = 30;

        // Refresh all visuals with starting state
        foreach (var doll in allDolls)
            doll.visuals?.UpdateVisuals(doll.state);
    }

    // ── Called at end of each day by GameManager.EndDay() ─────────────────────

    public void RunNightForAllDolls()
    {
        foreach (var doll in allDolls)
            doll.NightProcess();        // DollBase.NightProcess() handles everything
    }

    // ── Helpers for DayEventManager / InteractionManager ───────────────────────

    public DollBase GetDollByName(string name)
    {
        switch (name.ToLower())
        {
            case "elizabeth": return elizabeth;
            case "oliver": return oliver;
            case "marie": return marie;
            default:
                Debug.LogWarning($"[DollManager] Unknown doll name: {name}");
                return null;
        }
    }
}