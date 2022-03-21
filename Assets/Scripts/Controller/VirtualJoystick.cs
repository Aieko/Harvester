using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;


public class VirtualJoystick: MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image bgImage;
    private Image joystickImage;
    private Vector3 inputVector;

    public bool wasTouched { get; private set; }


    private void Start()
    {
        wasTouched = false;
        bgImage = GetComponent<Image>();
        joystickImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, eventData.position, eventData.pressEventCamera, out position))
        {
            position.x = (position.x / bgImage.rectTransform.sizeDelta.x);
            position.y = (position.y / bgImage.rectTransform.sizeDelta.y);

            inputVector = new Vector3(position.x * 2, 0, position.y * 2);

            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            //Move Joystick Image

            joystickImage.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImage.rectTransform.rect.width / 3), inputVector.z * (bgImage.rectTransform.rect.height / 3));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        wasTouched = true;
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        wasTouched = false;
        inputVector = Vector3.zero;
        joystickImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        if (inputVector.z != 0)
            return inputVector.z;
        else return Input.GetAxis("Vertical");
    }
}
