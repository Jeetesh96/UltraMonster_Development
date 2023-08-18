using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColoChange : MonoBehaviour
{
    [SerializeField] private Image myImage;
    [SerializeField] private Color myColor;
    private void Start()
    {
        myColor.a = 1;

        myImage.color = myColor;
    }
}
