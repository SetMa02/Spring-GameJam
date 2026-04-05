using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private ButtonTrigger[] buttons;

    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private SpriteRenderer doorRenderer;

    [SerializeField] private SpriteRenderer[] indicators;
    [SerializeField] private Color inactiveColor = Color.red;
    [SerializeField] private Color activeColor = Color.green;

    private bool isOpen = false;

    public void CheckButtons()
    {
        UpdateIndicators();

        if (isOpen)
            return;

        foreach (ButtonTrigger button in buttons)
        {
            if (!button.IsActivated())
                return;
        }

        OpenDoor();
    }

    private void UpdateIndicators()
    {
        for (int i = 0; i < indicators.Length; i++)
        {
            if (i < buttons.Length && indicators[i] != null)
            {
                indicators[i].color = buttons[i].IsActivated() ? activeColor : inactiveColor;
            }
        }
    }

    private void OpenDoor()
    {
        isOpen = true;

        if (doorCollider != null)
            doorCollider.enabled = false;

        if (doorRenderer != null)
        {
            Color color = doorRenderer.color;
            color.a = 0.4f;
            doorRenderer.color = color;
        }
    }
}