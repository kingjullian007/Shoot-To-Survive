using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform joystickBackground;
    [SerializeField] private RectTransform joystickKnob;
    private Vector2 inputVector;
    private Vector2 joystickOriginalPosition;
    private bool isDragging = false;

    private void Start ()
    {
        joystickOriginalPosition = joystickKnob.anchoredPosition;
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            joystickKnob.anchoredPosition = localPoint;
            OnDrag(eventData);
            isDragging = true;
        }
    }

    public void OnDrag (PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            var halfWidth = joystickBackground.sizeDelta.x / 2;
            var halfHeight = joystickBackground.sizeDelta.y / 2;
            localPoint.x = Mathf.Clamp(localPoint.x, -halfWidth, halfWidth);
            localPoint.y = Mathf.Clamp(localPoint.y, -halfHeight, halfHeight);

            inputVector = new Vector2(localPoint.x / halfWidth, localPoint.y / halfHeight);
            inputVector = ( inputVector.magnitude > 1.0f ) ? inputVector.normalized : inputVector;

            joystickKnob.anchoredPosition = new Vector2(inputVector.x * halfWidth, inputVector.y * halfHeight);
        }
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        isDragging = false;
        inputVector = Vector2.zero;
        joystickKnob.anchoredPosition = joystickOriginalPosition;
    }

    public float Horizontal ()
    {
        return inputVector.x;
    }

    public float Vertical ()
    {
        return inputVector.y;
    }
}
