using UnityEngine;

public class RibbonDollLogic : DollBase
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void ProcessNightEvent()
    {
        // Unique Event Logic for Ribbon Doll
        
        // This doll is all about CORRUPTION
        // The more you interact with it (or don't), the more corrupt it becomes
        
        // Stage 1: Subtle presence (corruption 20-50)
        if (state.corruption >= 20 && state.corruption < 50)
        {
            Debug.Log("The ribbon seems to shimmer with an unnatural light.");
            // Play subtle, eerie sound
            if (audioSource != null)
                audioSource.Play();
        }
        
        // Stage 2: Dangerous (corruption 50-70)
        else if (state.corruption >= 50 && state.corruption < 70)
        {
            Debug.Log("The ribbon TIGHTENS around the doll's body. Is it getting bigger?");
            // Corruption spreads passively
            state.corruption += 3;
        }
        
        // Stage 3: CRITICAL (corruption >= 70)
        else if (state.corruption >= 70)
        {
            Debug.Log("The ribbon unravels violently. The doll is WRONG.");
            state.mood = 0; // Doll is now inert, corrupted beyond recovery
            
            // Play disturbing sound
            if (audioSource != null && !audioSource.isPlaying)
                audioSource.Play();
        }

        // Natural decay: Ribbon doll's corruption increases slowly over time
        state.corruption += 2;
        
        // BUT: Can be slowed by positive interactions
        if (state.mood > 60)
        {
            state.corruption -= 3; // Reduce corruption if doll is in good mood
        }
    }
}
