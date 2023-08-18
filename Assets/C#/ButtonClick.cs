using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClick : EventTrigger
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.buttonEvent = true;
    }

    /// <summary>
    /// Called by the EventSystem when the pointer exits the object associated with this EventTrigger.
    /// </summary>
    public override void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.buttonEvent = false;
    }


    /// <summary>
    /// Called by the EventSystem when a PointerDown event occurs.
    /// </summary>
    public override void OnPointerDown(PointerEventData eventData)
    {

        Debug.Log("Pointer down");
        GameManager.Instance.buttonEvent = true;
    }

    /// <summary>
    /// Called by the EventSystem when a PointerUp event occurs.
    /// </summary>
    public override void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer up");
        GameManager.Instance.buttonEvent = false;
    }
    public override void OnSelect(BaseEventData eventData)
    {
        //Debug.Log("Selected");
        //GameManager.Instance.buttonEvent = true;
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        //GameManager.Instance.buttonEvent = true;
    }

}
