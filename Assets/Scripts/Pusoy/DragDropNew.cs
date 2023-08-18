using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropNew : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    public Vector2 startAnchoredPosition;
    private CanvasGroup canvasGroup;
    Vector2 newpos;
    public bool colliding = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        Transform testcanvastransform = transform;
        do
        {
            testcanvastransform = testcanvastransform.parent;
            canvas = testcanvastransform.GetComponent<Canvas>();
        } while (canvas == null);
        
    }
    private void Start()
    {
        startAnchoredPosition = rectTransform.anchoredPosition;
    }
    public void OnDrag(PointerEventData eventData)
    {
        colliding = false;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
      //  Debug.Log(rectTransform.rect.Contains)
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Invoke("DropCheck", 0.1f);
        
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .8f;
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();
    }
    void DropCheck()
    {
        if(colliding == false)
            rectTransform.anchoredPosition = startAnchoredPosition;
    }

    public void SetPos()
    {
        rectTransform.anchoredPosition = startAnchoredPosition;
    }
}
