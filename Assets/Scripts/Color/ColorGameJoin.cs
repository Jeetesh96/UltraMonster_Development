using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColorGameJoin : MonoBehaviour
{
    private SoundManager soundManager;

    private void Start()
    {
        soundManager = GameObject.FindWithTag("GameMusic").GetComponent<SoundManager>();
       // PlayerPrefs.SetInt("PlayerCoins", 100000);
    }
    public void ColorPress()
    {
       soundManager.PlayClick();
        SceneManager.LoadScene("ColorLoading");
    }
    public void PusoyJoin()
    {
       soundManager.PlayClick();
        SceneManager.LoadScene("PusoyLoading");
    }
    public void PokerJoin()
    {
         soundManager.PlayClick();
        SceneManager.LoadScene("PokerLoading");
    }
    public void TongitsJoin()
    {
        soundManager.PlayClick();
        SceneManager.LoadScene("RoomsPun2");
    }
}
