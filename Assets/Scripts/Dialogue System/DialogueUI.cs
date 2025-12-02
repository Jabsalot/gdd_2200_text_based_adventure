using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class DialogueUI : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public TextMeshProUGUI speakerTextDisplay;
    public TextMeshProUGUI dialogueTextDisplay;
    public List<Button> buttons;
    public List<TextMeshProUGUI> ButtonLabels;

    private void OnEnable()
    {
        dialogueManager.OnDialogueUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        dialogueManager.OnDialogueUpdated -= UpdateUI;
    }

    private void UpdateUI(string speaker, string dialogue, List<DialogueChoices> choices)
    {
        speakerTextDisplay.text = speaker;
        dialogueTextDisplay.text = dialogue;

        if (choices == null)
            return;

        foreach (var b in buttons)
            b.gameObject.SetActive(false);

        for (int i = 0; i < choices.Count; i++)
        {
            if(i < choices.Count)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].GetComponentInChildren<TMP_Text>().text = choices[i].choiceText;
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }
}
