using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LongClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float ClickDuration = 0.5f;
    public UnityEvent OnLongClick;
    public UnityEvent OnShortClick;

    bool clicking = false;
    float totalDownTime = 0;
    bool pointerDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;

        Debug.Log("OnPointerDown");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        totalDownTime = 0;
        pointerDown = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (pointerDown)
        {


            // Detect the first click
            if (Input.GetMouseButtonDown(0))
            {
                totalDownTime = 0;
                clicking = true;
            }

            // If a first click detected, and still clicking,
            // measure the total click time, and fire an event
            // if we exceed the duration specified
            if (clicking && Input.GetMouseButton(0))
            {

                totalDownTime += Time.deltaTime;

                if (totalDownTime >= ClickDuration)
                {
                    Debug.Log("Long click");
                    clicking = false;
                    OnLongClick.Invoke();
                }
            }
        }
        else
        { 
            // If a first click detected, and we release before the
            // duraction, do nothing, just cancel the click
            if (clicking && Input.GetMouseButtonUp(0))
            {
                OnShortClick.Invoke();
                clicking = false;
            }
        }

        
    }


}
