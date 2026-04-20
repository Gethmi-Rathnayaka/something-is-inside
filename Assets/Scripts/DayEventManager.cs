using UnityEngine;

/// <summary>
/// Defines all 10 days of narrative events and available player choices.
/// This is the "script" for the game—what happens each day and what the player can do.
/// </summary>
[System.Serializable]
public class DayEvent
{
    public int dayNumber;
    public string eventTitle;
    public string eventDescription;

    // Up to 3 choices per day (3 interactions per day)
    [System.Serializable]
    public class Choice
    {
        public string choiceText;
        public string affectedDoll; // "Angry", "Weeping", "Ribbon"
        public string visualEffect; // "clean", "dirty", "torn", "comforted", etc.
        public int moodImpact; // How this affects the doll's mood
    }

    public Choice[] choices = new Choice[3];
}

public class DayEventManager : MonoBehaviour
{
    [SerializeField] private DayEvent[] all10Days = new DayEvent[10];

    private int currentDay = 1;

    private void Start()
    {
        InitializeDefaultEvents();
    }

    public void SetDay(int day)
    {
        currentDay = day;
    }

    public DayEvent GetCurrentDayEvent()
    {
        if (currentDay >= 1 && currentDay <= 10)
            return all10Days[currentDay - 1];
        return null;
    }

    public DayEvent GetDayEvent(int day)
    {
        if (day >= 1 && day <= 10)
            return all10Days[day - 1];
        return null;
    }

    public bool IsFixedEventDay(int day)
    {
        // Days 1, 3, 5, 7, 10 have scripted events
        return day == 1 || day == 3 || day == 5 || day == 7 || day == 10;
    }

    private void InitializeDefaultEvents()
    {
        // DAY 1: Introduction
        all10Days[0] = new DayEvent
        {
            dayNumber = 1,
            eventTitle = "Grandma's Dolls",
            eventDescription = "You found a box with a note:\n\"Sweetie, these are my companions. Be kind to them. First 10 days to get along... or leave the house.\"",
            choices = new DayEvent.Choice[3]
            {
                new DayEvent.Choice { choiceText = "Take them out & observe", affectedDoll = "All", visualEffect = "first_look" },
                new DayEvent.Choice { choiceText = "Read the note again", affectedDoll = "None", visualEffect = "re_read" },
                new DayEvent.Choice { choiceText = "Put them back", affectedDoll = "All", visualEffect = "ignored" }
            }
        };

        // DAY 2: Free day
        all10Days[1] = new DayEvent
        {
            dayNumber = 2,
            eventTitle = "Getting to Know Them",
            eventDescription = "The dolls seem to watch you. One of them looks particularly dusty...",
            choices = new DayEvent.Choice[3]
            {
                new DayEvent.Choice { choiceText = "Clean the dusty one", affectedDoll = "Angry", visualEffect = "clean" },
                new DayEvent.Choice { choiceText = "Ignore & observe", affectedDoll = "Angry", visualEffect = "observe" },
                new DayEvent.Choice { choiceText = "Check the ribbon doll", affectedDoll = "Ribbon", visualEffect = "inspect" }
            }
        };

        // DAY 3: FIXED EVENT - First Decision
        all10Days[2] = new DayEvent
        {
            dayNumber = 3,
            eventTitle = "Who is Crying?",
            eventDescription = "At night, you hear crying. The Weeping Doll is trembling. You have something to give her...",
            choices = new DayEvent.Choice[3]
            {
                new DayEvent.Choice { choiceText = "Give the pretty ribbon", affectedDoll = "Weeping", visualEffect = "comforted_ribbon" },
                new DayEvent.Choice { choiceText = "Give the 4-leaf clover", affectedDoll = "Weeping", visualEffect = "comforted_clover" },
                new DayEvent.Choice { choiceText = "Do nothing", affectedDoll = "Weeping", visualEffect = "ignored_crying" }
            }
        };

        // DAY 4: Free day
        all10Days[3] = new DayEvent
        {
            dayNumber = 4,
            eventTitle = "Something's Wrong",
            eventDescription = "You notice one of the dolls' eyes look... different. Wider. Wrong.",
            choices = new DayEvent.Choice[3]
            {
                new DayEvent.Choice { choiceText = "Look closer at Angry Doll", affectedDoll = "Angry", visualEffect = "inspect_eyes" },
                new DayEvent.Choice { choiceText = "Check the Weeping Doll", affectedDoll = "Weeping", visualEffect = "inspect_eyes" },
                new DayEvent.Choice { choiceText = "Ignore it", affectedDoll = "All", visualEffect = "fear_grow" }
            }
        };

        // DAY 5: FIXED EVENT - Critical Choice
        all10Days[4] = new DayEvent
        {
            dayNumber = 5,
            eventTitle = "The Ribbon Doll's Secret",
            eventDescription = "The Ribbon Doll's ribbon looks filthy and wrong. It seems to be tightening on its own.",
            choices = new DayEvent.Choice[3]
            {
                new DayEvent.Choice { choiceText = "Remove the ribbon COMPLETELY", affectedDoll = "Ribbon", visualEffect = "ribbon_removed" },
                new DayEvent.Choice { choiceText = "Clean the ribbon", affectedDoll = "Ribbon", visualEffect = "ribbon_cleaned" },
                new DayEvent.Choice { choiceText = "Leave it alone", affectedDoll = "Ribbon", visualEffect = "ribbon_ignored" }
            }
        };

        // DAY 6: Free day
        all10Days[5] = new DayEvent
        {
            dayNumber = 6,
            eventTitle = "Dreams and Reality",
            eventDescription = "You had a nightmare. Someone tried to strangle you. When you woke up... the Angry Doll was closer than before.",
            choices = new DayEvent.Choice[3]
            {
                new DayEvent.Choice { choiceText = "Move the Angry Doll away", affectedDoll = "Angry", visualEffect = "repositioned" },
                new DayEvent.Choice { choiceText = "Comfort it", affectedDoll = "Angry", visualEffect = "comforted" },
                new DayEvent.Choice { choiceText = "Don't touch it", affectedDoll = "Angry", visualEffect = "fear_grow" }
            }
        };

        // DAY 7: FIXED EVENT - Maintenance Choice
        all10Days[6] = new DayEvent
        {
            dayNumber = 7,
            eventTitle = "Grooming Day",
            eventDescription = "The Angry Doll's hair is matted and messy. The Weeping Doll's dress has a small tear.",
            choices = new DayEvent.Choice[3]
            {
                new DayEvent.Choice { choiceText = "Brush Angry Doll's hair", affectedDoll = "Angry", visualEffect = "brushed" },
                new DayEvent.Choice { choiceText = "Repair Weeping Doll's dress", affectedDoll = "Weeping", visualEffect = "dress_repaired" },
                new DayEvent.Choice { choiceText = "Separate the dolls", affectedDoll = "All", visualEffect = "separated" }
            }
        };

        // DAY 8: Free day
        all10Days[7] = new DayEvent
        {
            dayNumber = 8,
            eventTitle = "Quiet Day",
            eventDescription = "The house is unusually quiet. The dolls seem almost... peaceful. This is unusual.",
            choices = new DayEvent.Choice[3]
            {
                new DayEvent.Choice { choiceText = "Play with them", affectedDoll = "All", visualEffect = "played_with" },
                new DayEvent.Choice { choiceText = "Leave them be", affectedDoll = "All", visualEffect = "isolation" },
                new DayEvent.Choice { choiceText = "Rearrange them", affectedDoll = "All", visualEffect = "rearranged" }
            }
        };

        // DAY 9: Free day
        all10Days[8] = new DayEvent
        {
            dayNumber = 9,
            eventTitle = "The Final Night Approaches",
            eventDescription = "Tomorrow is day 10. The dolls' eyes follow you. You can feel them... judging.",
            choices = new DayEvent.Choice[3]
            {
                new DayEvent.Choice { choiceText = "Sleep peacefully", affectedDoll = "All", visualEffect = "accepted_sleep" },
                new DayEvent.Choice { choiceText = "Sleep fitfully", affectedDoll = "All", visualEffect = "nightmare" },
                new DayEvent.Choice { choiceText = "Stay awake with them", affectedDoll = "All", visualEffect = "vigil" }
            }
        };

        // DAY 10: FINAL JUDGMENT
        all10Days[9] = new DayEvent
        {
            dayNumber = 10,
            eventTitle = "Judgment Day",
            eventDescription = "The dolls will decide if you can stay in this house... or if you must leave immediately.",
            choices = new DayEvent.Choice[3]
            {
                new DayEvent.Choice { choiceText = "Face them directly", affectedDoll = "All", visualEffect = "face_down" },
                new DayEvent.Choice { choiceText = "Beg for forgiveness", affectedDoll = "All", visualEffect = "apologize" },
                new DayEvent.Choice { choiceText = "Run", affectedDoll = "All", visualEffect = "run_away" }
            }
        };
    }
}
