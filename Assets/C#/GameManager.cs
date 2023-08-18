using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Net.NetworkInformation;

using System;
using System.Text;
using System.Net;
using System.ComponentModel;
using System.Threading;

public class GameManager : MonoBehaviour
{

    public int playerScore = 0;
    public int enemyScore = 0;
    public AudioSource bgMusicSource;
    public AudioSource soundSource;
    public float pingInMs = 0;
    public bool gameStarted = false;
    public bool gameEnded = false;
    public bool buttonEvent;

    #region Singleton

    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject gb = new GameObject("GameManager");
                GameManager component = gb.AddComponent<GameManager>();
                _instance = component;
            }
            return _instance;
        }
    }

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        SceneManager.LoadScene("LoadingHomeScreen");

        //PlayerPrefs.DeleteAll();

        Application.targetFrameRate = 60;

    }

    #endregion
}
