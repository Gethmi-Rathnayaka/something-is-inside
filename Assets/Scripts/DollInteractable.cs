using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach to: Each doll GameObject (Elizabeth, Oliver, Marie).
/// References the doll-specific logic script and enables click interaction.
/// </summary>
public class DollInteractable : MonoBehaviour, IPointerClickHandler
{
    [Header("Doll Reference")]
    [SerializeField] private DollBase dollLogic;

    private UIManager uiManager;
    private DayEventManager dayEventManager;

    private void Start()
    {
        if (dollLogic == null)
            dollLogic = GetComponent<DollBase>();

        uiManager = FindFirstObjectByType<UIManager>();
        dayEventManager = FindFirstObjectByType<DayEventManager>();

        var collider2D = GetComponent<Collider2D>();
        if (collider2D == null)
        {
            Debug.LogWarning($"[DollInteractable] {gameObject.name} has no Collider2D! Clicks won't work.");
        }
    }

    // ✅ REPLACES OnMouseDown (works with 2D)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (dollLogic == null || uiManager == null || dayEventManager == null)
        {
            Debug.LogError("[DollInteractable] Missing references!");
            return;
        }

        var actions = GetDollActions();

        uiManager.ShowDollPanel(
            dollLogic.state.dollName,
            dollLogic.state.mood,
            dollLogic.state.cleanliness,
            dollLogic.state.corruption,
            actions
        );
    }

    /// <summary>
    /// Returns the available actions for this doll.
    /// </summary>
    private (string label, System.Action action)[] GetDollActions()
    {
        InteractionManager intManager = FindFirstObjectByType<InteractionManager>();

        return new (string label, System.Action action)[]
        {
            ("Clean", () => intManager.CleanDoll(dollLogic.state.dollName.ToLower())),
            ("Brush Hair", () => intManager.BrushHair()),
            ("Gift", () => ShowGiftSubmenu()),
            ("Comfort", () => intManager.ComfortOliver())
        };
    }

    private void ShowGiftSubmenu()
    {
        InteractionManager intManager = FindFirstObjectByType<InteractionManager>();

        if (dollLogic is OliverLogic)
        {
            uiManager.ShowDollPanel(
                dollLogic.state.dollName,
                dollLogic.state.mood,
                dollLogic.state.cleanliness,
                dollLogic.state.corruption,
                new (string label, System.Action action)[]
                {
                    ("Gift: Ribbon", () => intManager.GiftOliver("ribbon")),
                    ("Gift: Clover", () => intManager.GiftOliver("clover")),
                    ("Gift: Rock", () => intManager.GiftOliver("rock")),
                    ("Back", () => OnPointerClick(null))
                }
            );
        }
        else if (dollLogic is ElizabethLogic)
        {
            uiManager.ShowDollPanel(
                dollLogic.state.dollName,
                dollLogic.state.mood,
                dollLogic.state.cleanliness,
                dollLogic.state.corruption,
                new (string label, System.Action action)[]
                {
                    ("Gift: Ribbon", () => intManager.GiftElizabeth("ribbon")),
                    ("Gift: Clover", () => intManager.GiftElizabeth("clover")),
                    ("Gift: Rock", () => intManager.GiftElizabeth("rock")),
                    ("Back", () => OnPointerClick(null))
                }
            );
        }
        else if (dollLogic is MarieLogic)
        {
            uiManager.ShowDollPanel(
                dollLogic.state.dollName,
                dollLogic.state.mood,
                dollLogic.state.cleanliness,
                dollLogic.state.corruption,
                new (string label, System.Action action)[]
                {
                    ("Gift: Ribbon", () => intManager.GiftMarie("ribbon")),
                    ("Gift: Clover", () => intManager.GiftMarie("clover")),
                    ("Gift: Rock", () => intManager.GiftMarie("rock")),
                    ("Back", () => OnPointerClick(null))
                }
            );
        }
        else
        {
            uiManager.ShowMessage("Invalid doll type.");
            OnPointerClick(null);
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log($"[Hover] {gameObject.name}");
    }

    private void OnMouseExit()
    {
    }
}