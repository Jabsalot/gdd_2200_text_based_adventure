using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Main Game UI")]
    public GameObject DialogueUI;
    public GameObject UtilUI;
    [Header("Map UI")]
    public GameObject MapUIPanel;

    private void OnEnable()
    {
        MapController.OnLocationSelected += ShowMainUI;
    }

    private void OnDisable()
    {
        MapController.OnLocationSelected -= ShowMainUI;
    }

    private void Start()
    {
        ShowMainUI();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (MapUIPanel.activeSelf)
            {
                ShowMainUI();
            }
            else
            {
                ShowMapUI();
            }
        }
    }

    private void ShowMainUI()
    {
        DialogueUI.SetActive(true);
        UtilUI.SetActive(true);

        MapUIPanel.SetActive(false);
    }

    // TODO: Map should not be toggeled if in dialogue with an NPC
    // TODO: Should only be toggelable if player is in an environment dialogue
    private void ShowMapUI()
    {
        DialogueUI.SetActive(false);
        UtilUI.SetActive(false);

        MapUIPanel.SetActive(true);
    }
}
