using UnityEngine;

/// <summary>
/// Tracks all visual variations for a doll across 10 days.
/// Each doll has multiple visual states based on player choices.
/// Example: Angry Doll can be clean, dusty, with torn dress, possessed look, etc.
/// </summary>
public class DollVisualState : MonoBehaviour
{
    [System.Serializable]
    public class DayVisualVariations
    {
        public int day;

        // You'll assign these sprites in Inspector based on what happened
        public Sprite defaultSprite;  // What the doll looks like today
        public Sprite altSprite;      // Alternative if player made different choice

        public string eventDescription; // "Doll is dusty", "Dress is torn", etc.
        public string[] choiceTexts;    // ["Clean her", "Just observe"]
    }

    [SerializeField] private DayVisualVariations[] allDays = new DayVisualVariations[10];

    private int currentDay = 1;
    private Sprite currentSprite;

    // Track player choices per day (0 = first choice, 1 = second choice, -1 = no choice yet)
    private int[] playerChoicesPerDay = new int[10];

    private void Start()
    {
        for (int i = 0; i < playerChoicesPerDay.Length; i++)
        {
            playerChoicesPerDay[i] = -1;
        }
    }

    public void SetDay(int day)
    {
        currentDay = day;
    }

    public void ApplyChoice(int choiceIndex)
    {
        if (currentDay >= 1 && currentDay <= 10)
        {
            playerChoicesPerDay[currentDay - 1] = choiceIndex;
            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        if (currentDay >= 1 && currentDay <= 10)
        {
            DayVisualVariations dayVar = allDays[currentDay - 1];

            // If player chose option 0, show default. If option 1, show alternative
            int choice = playerChoicesPerDay[currentDay - 1];

            if (choice == 0)
                currentSprite = dayVar.defaultSprite;
            else if (choice == 1)
                currentSprite = dayVar.altSprite;

            // Fallback
            if (currentSprite == null)
                currentSprite = dayVar.defaultSprite;

            GetComponent<SpriteRenderer>().sprite = currentSprite;
        }
    }

    public Sprite GetCurrentSprite()
    {
        return currentSprite;
    }

    public int GetChoice(int day)
    {
        if (day >= 1 && day <= 10)
            return playerChoicesPerDay[day - 1];
        return -1;
    }
}
