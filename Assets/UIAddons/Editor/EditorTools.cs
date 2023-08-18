using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UIAddons;
using UnityEditor;

public class EditorTools 
{
    [MenuItem("GameObject/UI Addons/Set Anchors/Top/Left", false, 0)]
    static void AnchorTopLeft()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.TopLeft, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Top/Center", false, 1)]
    static void AnchorTopCenter()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.TopCenter, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Top/Right", false, 2)]
    static void AnchorTopRight()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.TopRight, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Middle/Left", false, 0)]
    static void AnchorMiddleLeft()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.MiddleLeft, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Middle/Center", false, 1)]
    static void AnchorMiddleCenter()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.MiddleCenter, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Middle/Right", false, 2)]
    static void AnchorMiddleRight()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.MiddleRight, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Bottom/Left", false, 0)]
    static void AnchorBottomLeft()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.BottomLeft, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Bottom/Center", false, 1)]
    static void AnchorBottomCenter()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.BottomCenter, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Bottom/Right", false, 2)]
    static void AnchorBottomRight()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.BottomRight, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Strech/Right Strech", false, 6)]
    static void AnchorRight()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.RightStrech, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Strech/Full Strech", false, 0)]
    static void AnchorStrech()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.FullStrech, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Strech/Top Strech", false, 1)]
    static void AnchorTop()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.TopStrech, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Strech/Middle Strech", false, 2)]
    static void AnchorMiddle()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.MiddleStrech, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Strech/Bottom Strech", false, 3)]
    static void AnchorBottom()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.BottomStrech, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Strech/Left Strech", false, 4)]
    static void AnchorLeft()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.LeftStrech, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Set Anchors/Strech/Center Strech", false, 5)]
    static void AnchorCenter()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            UITools.SetElementAnchor(LocalAnchor.CenterStrech, Selection.gameObjects[i]);
        }
    }

    [MenuItem("GameObject/UI Addons/Add/Pulse Effect", false, 0)]
    static void AddPulseEffect()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObject currentGameObject = Selection.gameObjects[i];
            currentGameObject.AddComponent<PulseEffect>();
        }
    }

    [MenuItem("GameObject/UI Addons/Add/Pop Effect", false, 1)]
    static void AddPopEffect()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObject currentGameObject = Selection.gameObjects[i];
            currentGameObject.AddComponent<PopEffect>();
        }
    }

    [MenuItem("GameObject/UI Addons/Add/Rolling Text Effect", false, 2)]
    static void AddRollingText()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObject currentGameObject = Selection.gameObjects[i];
            currentGameObject.AddComponent<RollingTextEffect>();
        }
    }

    [MenuItem("GameObject/UI Addons/Add/Blink Effect", false, 3)]
    static void AddBlinkEffect()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObject currentGameObject = Selection.gameObjects[i];
            currentGameObject.AddComponent<BlinkEffect>();
        }
    }

    [MenuItem("GameObject/UI Addons/Add/Fade Effect", false, 4)]
    static void AddFadeEffect()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObject currentGameObject = Selection.gameObjects[i];
            currentGameObject.AddComponent<FadeEffectScript>();
        }
    }

    [MenuItem("GameObject/UI Addons/Add/Moving Item Effect", false, 5)]
    static void AddMovingEffect()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObject currentGameObject = Selection.gameObjects[i];
            currentGameObject.AddComponent<MovingItem>();
        }
    }

    [MenuItem("GameObject/UI Addons/Add/Dynamic Resize Effect", false, 6)]
    static void AddDynamicResize()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObject currentGameObject = Selection.gameObjects[i];
            currentGameObject.AddComponent<DynamicResize>();
        }
    }

    [MenuItem("GameObject/UI Addons/Add/Text Appear Effect", false, 7)]
    static void AddTextAppearEffect()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObject currentGameObject = Selection.gameObjects[i];
            currentGameObject.AddComponent<TextAppearEffect>();
        }
    }

    [MenuItem("GameObject/UI Addons/Add/Grayscale", false, 8)]
    static void AddGrayscale()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObject currentGameObject = Selection.gameObjects[i];
            UITools.MakeGrayScale(currentGameObject);
        }
    }

    [MenuItem("GameObject/UI Addons/Remove/Grayscale", false, 0)]
    static void RemoveGrayscale()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObject currentGameObject = Selection.gameObjects[i];
            UITools.RemoveGrayscale(currentGameObject);
        }
    }
}
