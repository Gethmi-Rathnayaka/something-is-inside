using UnityEngine;

public class WeepingDollLogic : DollBase
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void ProcessNightEvent()
    {
        // Unique Event Logic for Weeping Doll
        
        // Stage 1: Sad (neglect 1-2)
        if (state.neglectCounter >= 1 && state.neglectCounter < 3)
        {
            Debug.Log("You hear soft sobbing from the doll's direction...");
            // Play gentle crying sound
            if (audioSource != null)
                audioSource.Play();
        }
        
        // Stage 2: Distressed (neglect 3+)
        else if (state.neglectCounter >= 3)
        {
            Debug.Log("The Weeping Doll moves closer to you. Its face is wet.");
            state.corruption += 5; // Corruption from prolonged sadness
            
            // Doll becomes MORE creepy the more you ignore it
            state.ModifyMood(-10);
            
            // Play louder, more disturbing sound
            if (audioSource != null && !audioSource.isPlaying)
                audioSource.Play();
        }

        // Natural behavior: If mood is good, neglect doesn't affect it as much
        if (state.mood > 70)
        {
            state.neglectCounter = 0; // Reset neglect counter if doll is happy
        }
    }
}
