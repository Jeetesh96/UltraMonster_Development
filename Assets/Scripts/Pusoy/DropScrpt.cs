using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropScrpt : MonoBehaviour, IDropHandler
{
    Vector2 newpos;
    Sprite oldcard;
  //  private SoundManager soundManager;

    private void Start()
    {
       // soundManager = GameObject.FindWithTag("GameMusic").GetComponent<SoundManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("DropCheck");
        if (eventData.pointerDrag != null)
        {
           // soundManager.OnDrop();
            Debug.Log("DropCheck2");
            eventData.pointerDrag.GetComponent<DragDropNew>().colliding = true;
            //newpos = GetComponent<RectTransform>().anchoredPosition;
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            //GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<DragDropNew>().startAnchoredPosition;
            //eventData.pointerDrag.GetComponent<DragDropNew>().startAnchoredPosition = newpos;
            //GetComponent<DragDropNew>().startAnchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            string newname = GetComponent<Image>().name;
            GetComponent<Image>().name = eventData.pointerDrag.GetComponent<Image>().name;
            eventData.pointerDrag.GetComponent<Image>().name = newname;

            oldcard = GetComponent<Image>().sprite;
            Sprite newsprite = eventData.pointerDrag.GetComponent<Image>().sprite;
            eventData.pointerDrag.GetComponent<Image>().sprite = oldcard;
            GetComponent<Image>().sprite = newsprite;
            eventData.pointerDrag.GetComponent<DragDropNew>().SetPos();
            

            int newindex = GetComponent<CardIndex>().index;
            int newtype = GetComponent<CardIndex>().cardType;
            int spritenum = GetComponent<CardIndex>().spritenum;

            GetComponent<CardIndex>().index = eventData.pointerDrag.GetComponent<CardIndex>().index;
            GetComponent<CardIndex>().cardType = eventData.pointerDrag.GetComponent<CardIndex>().cardType;
            GetComponent<CardIndex>().spritenum = eventData.pointerDrag.GetComponent<CardIndex>().spritenum;

            eventData.pointerDrag.GetComponent<CardIndex>().index = newindex;
            eventData.pointerDrag.GetComponent<CardIndex>().cardType = newtype;
            eventData.pointerDrag.GetComponent<CardIndex>().spritenum = spritenum;
        }
    }
}
