using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text dialogueText;
    public Text dayText;

    public GameObject nightOverlay;
    public GameObject fadePanel;

    // Choice UI elements (assign in Inspector)
    public Button choiceButton1;
    public Button choiceButton2;
    public Button choiceButton3;

    public InteractionManager interactionManager;

    public void ShowMessage(string msg)
    {
        dialogueText.text = msg;
    }

    public void UpdateDay(int day)
    {
        dayText.text = "Day " + day;
    }

    public void DisplayChoices(DayEvent.Choice[] choices)
    {
        // Show the 3 choice buttons with the text from choices
        if (choices.Length >= 1 && choiceButton1 != null)
        {
            choiceButton1.GetComponentInChildren<Text>().text = choices[0].choiceText;
            choiceButton1.onClick.RemoveAllListeners();
            int index0 = 0;
            choiceButton1.onClick.AddListener(() => interactionManager.HandleChoice(index0));
        }

        if (choices.Length >= 2 && choiceButton2 != null)
        {
            choiceButton2.GetComponentInChildren<Text>().text = choices[1].choiceText;
            choiceButton2.onClick.RemoveAllListeners();
            int index1 = 1;
            choiceButton2.onClick.AddListener(() => interactionManager.HandleChoice(index1));
        }

        if (choices.Length >= 3 && choiceButton3 != null)
        {
            choiceButton3.GetComponentInChildren<Text>().text = choices[2].choiceText;
            choiceButton3.onClick.RemoveAllListeners();
            int index2 = 2;
            choiceButton3.onClick.AddListener(() => interactionManager.HandleChoice(index2));
        }

        // Show the buttons
        if (choiceButton1 != null) choiceButton1.gameObject.SetActive(true);
        if (choiceButton2 != null) choiceButton2.gameObject.SetActive(true);
        if (choiceButton3 != null) choiceButton3.gameObject.SetActive(true);
    }

    public void StartNightSequence()
    {
        // Hide choice buttons during night
        if (choiceButton1 != null) choiceButton1.gameObject.SetActive(false);
        if (choiceButton2 != null) choiceButton2.gameObject.SetActive(false);
        if (choiceButton3 != null) choiceButton3.gameObject.SetActive(false);

        StartCoroutine(NightRoutine());
    }

    System.Collections.IEnumerator NightRoutine()
    {
        fadePanel.SetActive(true);
        yield return new WaitForSeconds(1f);

        nightOverlay.SetActive(true);
        yield return new WaitForSeconds(1f);

        fadePanel.SetActive(false);
    }
}
