using UnityEngine;
using UnityEngine.UI;
using UIAddons;
using System;
using System.Collections;
using System.Linq;

public class SceneExample : MonoBehaviour 
{
    //fist scene
    public RectTransform popButton;
    public RectTransform rollingText;
    public RectTransform fadeBackground;
    public CustomProgressBar progressBar;
    public RectTransform slidingButton;
    public RectTransform resizingButton;
    public RectTransform pulsingButton;
    bool isFillingProgressBar;
    bool isSceneOne;

	// Use this for initializatiowon
	void Start () 
    {
        Application.targetFrameRate = 60;

        if (Application.loadedLevelName == "SceneOne")
        {
            isSceneOne = true;
            UITools.StartFadeEffect(fadeBackground, FadingType.In, 2f, EffectEndType.Callback, () => { Debug.Log("Background Finished Fading In"); });
            UITools.StartPulseEffect(pulsingButton, 1.2f, 0.8f, delay: 4f);
            progressBar.slider.maxValue = 10f;
            progressBar.slider.minValue = 0f;
            progressBar.slider.value = 0f;
        }
        else
        {
            scrollPanel = GameObject.FindObjectOfType<ScrollRect>().transform.GetChild(0).GetComponent<RectTransform>();
            expandParent = scrollPanel.GetChild(0).GetComponent<ExpandItemParent>();
            expandItemHeight = expandParent.allExpandingItems[1].maskRect.rect.height;                      //needed for the calculation of the Y coordinate
            expandedItemHeight = expandParent.allExpandingItems[1].itemRect.rect.height - expandItemHeight; //That heigh is used in case you are adding items to the scroller while there is already expanded items.
            scrollerItems = expandParent.childs.Count - 1;                                                  //Child with zero index is actually the item we are instatiating.It doesnt' need to get in to the calculations of the new added items coordinates.
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!isSceneOne)
        {
            return;
        }

        if (isFillingProgressBar)
        {
            progressBar.slider.value += Time.deltaTime;
        }

        if (progressBar.slider.value >= progressBar.slider.maxValue && isFillingProgressBar)
        {
            isFillingProgressBar = false;
        }
	}

    public void OnPulsingButtonClicked()
    {
        if (rollingText == null)
        {
            return;
        }

        UITools.StartTextRollingEffect(rollingText, "Example Scene", 3f);
    }

    public void OnPopButtonClicked()
    {
        if (popButton == null)
        {
            return;
        }

        UITools.StartPopEffect(popButton, 1f, PopEffectType.Up, EffectEndType.Callback, StartFillingUpProgressBar);
    }

    void StartFillingUpProgressBar()
    {
        progressBar.slider.value = 0f;
        isFillingProgressBar = true;
    }

    public void OnSlidingButtonClicked()
    {
        UITools.MoveUIElement(slidingButton, new Vector2(0.6f * Screen.width, slidingButton.anchoredPosition.y), 2f, () => { Debug.Log("End of slide reached"); }, false, isSmooth: true);
    }

    public void OnResizeButtonClicked()
    {
        if (resizingButton.rect.width > 71)
        {
            UITools.ResizeUIElementAbsolute(resizingButton, 70, 50, 0.2f);
        }
        else
        {
            UITools.ResizeUIElementAbsolute(resizingButton, 120, 100, 0.2f);
        }
    }

    //Second scene 
    public RectTransform textAppearRectTransform;

    public void OnTextAppearClicked()
    {
        if (textAppearRectTransform == null)
        {
            return;
        }

        string text = "1.This is text appear effect\n2.On press of the expand button scroll content will be dynamically resized without affecting the children.Note the pivot point of the Panel. Also note that the actuall object that is resized is parent of the expanding items group.\n3.We wish you happy coding";

        UITools.StartTextAppearEffect(textAppearRectTransform, text, 8f);
    }

    public RectTransform item;
    int scrollerItems;
    RectTransform scrollPanel;
    ExpandItemParent expandParent;
    float expandItemHeight;
    float expandedItemHeight;
    public void OnAddItemToScrollRect()
    {
        UITools.ResizeUIParentAbsolute(scrollPanel, (int)scrollPanel.rect.width, (int)scrollPanel.rect.height + (int)expandItemHeight);
        scrollerItems++;

        //since this is dragged from the scrollview object it already has its X coordinate assigned correctly.We need to find the new y coordinate
        float position_y = (expandItemHeight / 2) - (scrollerItems * expandItemHeight) - (expandParent.expandedItems * expandedItemHeight);
        RectTransform newItemRect = (RectTransform)GameObject.Instantiate(item) as RectTransform;

        newItemRect.SetParent(expandParent.transform);
        newItemRect.localScale = Vector2.one;
        newItemRect.localPosition = new Vector3(item.localPosition.x, position_y, item.localPosition.z);
        newItemRect.gameObject.SetActive(true);

        expandParent.RefreshChilds();
    }

    bool isColored = true;
    public GameObject grayScaleButton;
    public void ChangeColor()
    {
        if (grayScaleButton == null)
        {
            return;
        }

        if (isColored)
        {
            UITools.MakeGrayScale(grayScaleButton, true);
            isColored = false;
        }
        else
        {
            UITools.RemoveGrayscale(grayScaleButton, true);
            isColored = true;
        }
    }
}
