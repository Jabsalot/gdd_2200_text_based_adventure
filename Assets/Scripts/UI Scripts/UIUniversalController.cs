using UnityEngine;

/// <summary>
/// Stores universal UI events and handlers.
/// Stores universal functions for UI elements.
/// </summary>
public class UIUniversalController : MonoBehaviour
{
    public delegate void OnBackButtonPresseed();
    public static event OnBackButtonPresseed onBackButtonPressed;

    public void BackButtonPressed()
    {
        Debug.Log("[UIUniversalController] Back button pressed.");
        onBackButtonPressed?.Invoke();
    }
}
