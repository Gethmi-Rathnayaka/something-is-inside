using UnityEngine;

public class AngryDollLogic : DollBase
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void ProcessNightEvent()
    {
        // Unique Event Logic for Angry Doll

        // Stage 1: Minor disturbance (mood between 30-50)
        if (state.mood >= 30 && state.mood < 50)
        {
            Debug.Log("The Angry Doll shifts uncomfortably...");
            // Could trigger subtle visual shake or tint
        }

        // Stage 2: Aggressive (mood 20-29)
        else if (state.mood >= 20 && state.mood < 30)
        {
            Debug.Log("The Angry Doll's face darkens. Its fists clench.");
            // Play menacing sound
            if (audioSource != null)
                audioSource.Play(); // Play aggressive sound
        }

        // Stage 3: VERY dangerous (mood < 20)
        else if (state.mood < 20)
        {
            Debug.Log("RAGE. The Angry Doll is out of control!");
            state.corruption += 10; // Corruption increases when doll is extremely angry

            // You could trigger a visual effect here (red screen tint, screen shake)
            // For now, we just escalate the corruption
        }

        // Natural decay: Angry doll's mood decreases over time if neglected
        if (state.neglectCounter > 0)
        {
            state.ModifyMood(-5 * state.neglectCounter);
        }
    }
}
