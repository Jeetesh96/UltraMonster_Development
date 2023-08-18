using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleTween3 : MonoBehaviour
{
    public float tweenTime;

    void Start()
    {
        Tween();
    }

    public void Tween()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.one;
        LeanTween.scale(gameObject, Vector3.one * 2, tweenTime).setEasePunch();
    }
}
