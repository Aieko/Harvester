using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;


public class VirtualJoystick: MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Image bgImage;
    [SerializeField] private Image joystickImage;
    private Vector3 inputVector;

    public bool wasTouched { get; private set; }
    private bool wasDragged;

    private void Start()
    {   
        wasTouched = false;
        var bgColor = bgImage.color;
        bgColor.a = 0;
        bgImage.color = bgColor;
        var joystickColor = joystickImage.color;
        joystickColor.a = 0;
        joystickImage.color = joystickColor;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!wasDragged)
        {
            FadeJoystick(false);
            wasDragged = true;
        }
      
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
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, eventData.pressEventCamera, out pos);
        bgImage.transform.position = transform.TransformPoint(pos);

        //OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(wasDragged)
        {
            FadeJoystick(true);
            wasDragged = false;
        }
        wasTouched = false;
        inputVector = Vector3.zero;
        joystickImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    private void FadeJoystick(bool fade)
    {
        
        StartCoroutine(FadeImage(fade, bgImage));
        StartCoroutine(FadeImage(fade, joystickImage));
    }

    private IEnumerator FadeImage(bool fadeAway, Image img)
    {
        
        if (fadeAway)
        {
            var origColor = img.color;

            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                
                img.color = new Color(img.color.r, img.color.g, img.color.b, i);
                yield return null;
            }

            img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        }
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                
                img.color = new Color(img.color.r, img.color.g, img.color.b, i);
                yield return null;
            }

            img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
        }
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
