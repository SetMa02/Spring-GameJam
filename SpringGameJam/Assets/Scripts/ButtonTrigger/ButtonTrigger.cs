using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer indicatorRenderer;
    [SerializeField] private Color inactiveColor = Color.red;
    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private DoorController door;

    private int objectsOnButton = 0;

    private void Start()
    {
        UpdateVisual();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsValidActivator(collision))
        {
            objectsOnButton++;
            UpdateVisual();

            if (door != null)
                door.CheckButtons();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsValidActivator(collision))
        {
            objectsOnButton--;

            if (objectsOnButton < 0)
                objectsOnButton = 0;

            UpdateVisual();

            if (door != null)
                door.CheckButtons();
        }
    }

    private bool IsValidActivator(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            return true;

        if (collision.GetComponent<Corps>() != null)
            return true;

        if (collision.GetComponentInParent<Player>() != null)
            return true;

        if (collision.GetComponentInParent<Corps>() != null)
            return true;

        return false;
    }

    public bool IsActivated()
    {
        return objectsOnButton > 0;
    }

    private void UpdateVisual()
    {
        if (indicatorRenderer != null)
            indicatorRenderer.color = IsActivated() ? activeColor : inactiveColor;
    }
}