using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenScript : MonoBehaviour
{
    public GameObject SplashScreen;
    public Image RingFillImg;

    public void Start()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        RingFillImg.fillAmount = 0;
        SplashScreen.SetActive(true);
        while (RingFillImg.fillAmount < 1)
        {
            RingFillImg.fillAmount += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        //SceneManager.LoadScene("Lobby");
        yield return new WaitForSeconds(0.5f);
        SplashScreen.SetActive(false);

    }
}
