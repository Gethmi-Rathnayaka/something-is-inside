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

        // Use the new overload that accepts DollBase for sprite display
        uiManager.ShowDollPanel(dollLogic, actions);
    }

    /// <summary>
    /// Returns the available actions for this doll.
    /// </summary>
    private (string label, System.Action action)[] GetDollActions()
    {
        InteractionManager intManager = FindFirstObjectByType<InteractionManager>();
        var list = new System.Collections.Generic.List<(string label, System.Action action)>();

        // Clean is universal
        list.Add(("Clean", () => intManager.CleanDoll(dollLogic.state.dollName.ToLower())));
        list.Add(("Brush Hair", () => intManager.BrushHair(dollLogic.state.dollName.ToLower())));

        // Doll-specific actions
        if (dollLogic is ElizabethLogic)
        {
            list.Add(("Gift", () => ShowGiftSubmenu()));
            // Elizabeth does not have a Comfort action in the UI
        }
        else if (dollLogic is OliverLogic)
        {
            // Oliver supports Comfort and Gifts
            list.Add(("Comfort", () => intManager.ComfortOliver()));
            list.Add(("Gift", () => ShowGiftSubmenu()));
        }
        else if (dollLogic is MarieLogic)
        {
            // Marie supports Gifts; brush/comfort not applicable
            list.Add(("Gift", () => ShowGiftSubmenu()));
        }
        else
        {
            // Fallback: offer Gift and Clean
            list.Add(("Gift", () => ShowGiftSubmenu()));
        }

        return list.ToArray();
    }

    private void ShowGiftSubmenu()
    {
        InteractionManager intManager = FindFirstObjectByType<InteractionManager>();

        if (dollLogic is OliverLogic)
        {
            uiManager.ShowDollPanel(
                dollLogic,
                new (string label, System.Action action)[]
                {
                    ("Ribbon", () => intManager.GiftOliver("ribbon")),
                    ("Clover", () => intManager.GiftOliver("clover")),
                    ("Rock", () => intManager.GiftOliver("rock")),
                    ("Back", () => OnPointerClick(null))
                }
            );
        }
        else if (dollLogic is ElizabethLogic)
        {
            uiManager.ShowDollPanel(
                dollLogic,
                new (string label, System.Action action)[]
                {
                    ("Ribbon", () => intManager.GiftElizabeth("ribbon")),
                    ("Clover", () => intManager.GiftElizabeth("clover")),
                    ("Rock", () => intManager.GiftElizabeth("rock")),
                    ("Back", () => OnPointerClick(null))
                }
            );
        }
        else if (dollLogic is MarieLogic)
        {
            uiManager.ShowDollPanel(
                dollLogic,
                new (string label, System.Action action)[]
                {
                    ("Ribbon", () => intManager.GiftMarie("ribbon")),
                    ("Clover", () => intManager.GiftMarie("clover")),
                    ("Rock", () => intManager.GiftMarie("rock")),
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