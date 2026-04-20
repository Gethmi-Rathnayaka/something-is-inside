using UnityEngine;

public abstract class DollBase : MonoBehaviour
{
    public DollState state;
    
    // Every doll MUST implement its own night event logic
    public abstract void ProcessNightEvent();
}