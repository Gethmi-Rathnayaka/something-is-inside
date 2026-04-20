using UnityEngine;

public class DollState : MonoBehaviour
{
    public string dollName;
    public SpriteRenderer spriteRenderer;

    // Assign these in the Inspector
    public Sprite stage1_Normal;
    public Sprite stage2_Creepy;
    public Sprite stage3_Distorted;

    [Range(0, 100)] public int mood = 50;
    [Range(0, 100)] public int corruption = 0;
    public int neglectCounter = 0;

    public void UpdateVisuals()
    {
        // Example logic: Change sprite based on corruption or mood
        if (corruption >= 70 || mood <= 20)
            spriteRenderer.sprite = stage3_Distorted;
        else if (corruption >= 30 || mood <= 40)
            spriteRenderer.sprite = stage2_Creepy;
        else
            spriteRenderer.sprite = stage1_Normal;
    }

    public void ModifyMood(int value)
    {
        mood = Mathf.Clamp(mood + value, 0, 100);
        UpdateVisuals(); 
    }
}