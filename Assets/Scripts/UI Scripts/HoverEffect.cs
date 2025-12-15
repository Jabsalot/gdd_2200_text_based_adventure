using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image uiImage; // Drag your Image GameObject here in the Inspector
    public Color normalColor = Color.white;
    public Color hoverColor = Color.gray;

    void Start()
    {
        // Get the Image component if not assigned in the Inspector
        if (uiImage == null)
        {
            uiImage = GetComponent<Image>();
        }
        // Set the initial color
        uiImage.color = normalColor;
    }

    // Called when the pointer enters the Image area
    public void OnPointerEnter(PointerEventData eventData)
    {
        uiImage.color = hoverColor;
    }

    // Called when the pointer exits the Image area
    public void OnPointerExit(PointerEventData eventData)
    {
        uiImage.color = normalColor;
    }
}
