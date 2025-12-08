using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject MainUIPanel;
    public GameObject MapUIPanel;

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
        MainUIPanel.SetActive(true);
        MapUIPanel.SetActive(false);
    }

    // TODO: Map should not be toggeled if in dialogue with an NPC
    // TODO: Should only be toggelable if player is in an environment dialogue
    private void ShowMapUI()
    {
        MainUIPanel.SetActive(false);
        MapUIPanel.SetActive(true);
    }
}
