using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayScreenManager : MonoBehaviour
{

    [SerializeField] Animator anim;
    bool opened = false;




    public void Pause()
    {
        //Time.timeScale = 0;
        if(!opened)
        {
            anim.SetTrigger("Open");
            opened = true;
        }
        else
        {
            anim.SetTrigger("Close");
            opened = false;
        }
    }

    public void Resume()
    {
        //Time.timeScale = 1;
    }

    public void Home()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Fish");
        GameManager.Instance.gameStarted = false;
        GameManager.Instance.gameEnded = true;
    }
}
