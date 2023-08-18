using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ColorManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject cubeholder;
    [SerializeField] private Animator[] anims;
    [SerializeField] private GameObject[] animObjects;
    [SerializeField] private Image[] coinsselector;
    [SerializeField] private Sprite nonselect;
    [SerializeField] private Sprite selected;
    [SerializeField] private Text[] coloramount;
     public DiceScript[] dicescript;
    [SerializeField] private GameObject[] dice;
    [SerializeField] private Animator[] diceset;
    //[SerializeField] private AnimationClip[] dice1anm;
    //[SerializeField] private AnimationClip[] dice2anm;
    //[SerializeField] private AnimationClip[] dice3anm;
    float timer = 0f;
    bool timeron = false;
    int gamestep = 0;
    bool selectcoins = false;
    int amountselector = 1;
    int color1, color2, color3, color4, color5, color6;
    int c1pressed = 0, c2pressed = 0, c3pressed = 0, c4pressed = 0, c5pressed = 0, c6pressed = 0;
    int plcoins;
    int total;
    int value1, value2, value3;
    private SoundManager soundManager;

    private void Start()
    {  
        soundManager = GameObject.FindWithTag("GameMusic").GetComponent<SoundManager>();
        if (PhotonNetwork.IsMasterClient)
        {
            GameStart();

        }
    }
    public void GameStart()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            StartTimer();
        }
    }
    private void Update()
    {
        plcoins = PlayerPrefs.GetInt("PlayerCoins", 0);

        if (PhotonNetwork.IsMasterClient)
        {
            if (timeron)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    timeron = false;
                    if (gamestep == 1)
                    {
                        SetNextAnm();
                    }
                    else if(gamestep == 2)
                    {
                        SetFinalAnm();
                    }
                    else if(gamestep == 3)
                    {
                        SetGamePlay();
                    }
                }
            }
        }

        //if (DiceNumberTextScript.spindone)
        //{
        //    DiceNumberTextScript.spindone = false;
        //    Invoke("CheckResult", 2f);
        //}
    }

    void StartTimer()
    {
        timer = 4f;
        gamestep = 1;
        timeron = true;
        photonView.RPC("SetStep1", RpcTarget.All);
    }
    [PunRPC]
    public void SetStep1()
    {
        animObjects[0].SetActive(true);
        anims[0].Play("GameStartanm");
    }
    void SetNextAnm()
    {
        timer = 11f;
        gamestep = 2;
        timeron = true;
        photonView.RPC("SetStep2", RpcTarget.All);
    }
    [PunRPC]
    public void SetStep2()
    {
        animObjects[0].SetActive(false);
        animObjects[1].SetActive(true);
        anims[1].Play("ColorPickanm");
        selectcoins = true;
    }
    void SetFinalAnm()
    {
        timer = 1f;
        gamestep = 3;
        timeron = true;
        photonView.RPC("SetStep3", RpcTarget.All);
    }
    [PunRPC]
    public void SetStep3()
    {
        selectcoins = false;
        animObjects[1].SetActive(false);
        animObjects[2].SetActive(true);
        anims[2].Play("Readyanm");
    }
    void SetGamePlay()
    {
        photonView.RPC("BeginGame", RpcTarget.All);
    }
    [PunRPC]
    public void BeginGame()
    {
        animObjects[2].SetActive(false);
         cubeholder.transform.Rotate(30.0f, 0.0f, 0.0f, Space.Self);
       // cubeholder.SetActive(false);
        //for (int i = 0; i < 3; i++)
        //{
        //    dicescript[i].DiceSpin();
        //}
       // dicescript[0].DiceSpin();
       // dicescript[1].DiceSpin();
        //dicescript[2].DiceSpin();
        if(PhotonNetwork.IsMasterClient)
        {
            int rnd1 = Random.Range(1, 6);
            int rnd2 = Random.Range(1, 6);
            int rnd3 = Random.Range(1, 6);
            photonView.RPC("SetDiceAnim", RpcTarget.All, rnd1, rnd2, rnd3);
        }
        Invoke("CheckResult", 5f);
    }
    [PunRPC]
    public void SetDiceAnim(int a1, int a2, int a3)
    {
        value1 = a1;
        value2 = a2;
        value3 = a3;
       soundManager.DiceRoll();
        switch(a1)
        {
            case 1:
                diceset[0].Play("Dice1color1");
                break;
            case 2:
                diceset[0].Play("Dice1color2");
                break;
            case 3:
                diceset[0].Play("Dice1color3");
                break;
            case 4:
                diceset[0].Play("Dice1color4");
                break;
            case 5:
                diceset[0].Play("Dice1color5");
                break;
            case 6:
                diceset[0].Play("Dice1color6");
                break;
        }
        switch (a2)
        {
            case 1:
                diceset[1].Play("Dice2color1");
                break;
            case 2:
                diceset[1].Play("Dice2color2");
                break;
            case 3:
                diceset[1].Play("Dice2color3");
                break;
            case 4:
                diceset[1].Play("Dice2color4");
                break;
            case 5:
                diceset[1].Play("Dice2color5");
                break;
            case 6:
                diceset[1].Play("Dice2color6");
                break;
        }
        switch (a3)
        {
            case 1:
                diceset[2].Play("Dice3color1");
                break;
            case 2:
                diceset[2].Play("Dice3color2");
                break;
            case 3:
                diceset[2].Play("Dice3color3");
                break;
            case 4:
                diceset[2].Play("Dice3color4");
                break;
            case 5:
                diceset[2].Play("Dice3color5");
                break;
            case 6:
                diceset[2].Play("Dice3color6");
                break;
        }
    }
    void CheckResult()
    {
       soundManager.Applause();
        int dice1 = value1;
        int dice2 = value2;
        int dice3 = value3;

        if(dice1 == 1 || dice2 == 1 || dice3 == 1)
        {
            if(color1 > 0)
            {
                c1pressed = 2 * color1;
            }
        }
        if (dice1 == 2 || dice2 == 2 || dice3 == 2)
        {
            if (color2 > 0)
            {
                c2pressed = 2 * color2;
            }
        }
        if (dice1 == 3 || dice2 == 3 || dice3 == 3)
        {
            if (color3 > 0)
            {
                c3pressed = 2 * color3;
            }
        }
        if (dice1 == 4 || dice2 == 4 || dice3 == 4)
        {
            if (color4 > 0)
            {
                c4pressed = 2 * color4;
            }
        }
        if (dice1 == 5 || dice2 == 5 || dice3 == 5)
        {
            if (color5 > 0)
            {
                c5pressed = 2 * color5;
            }
        }
        if (dice1 == 6 || dice2 == 6 || dice3 == 6)
        {
            if (color6 > 0)
            {
                c6pressed = 2 * color6;
            }
        }
        total = c1pressed + c2pressed + c3pressed + c4pressed + c5pressed + c6pressed;

        Invoke("ResetCoins", 2f);
    }
    void ResetCoins()
    {
        int coinsnew = plcoins + total;
        PlayerPrefs.SetInt("PlayerCoins", coinsnew);
        color1 = 0;
        color2 = 0;
        color3 = 0;
        color4 = 0;
        color5 = 0;
        color6 = 0;
        c1pressed = 0;
        c2pressed = 0;
        c3pressed = 0;
        c4pressed = 0;
        c5pressed = 0;
        c6pressed = 0;
        for(int i = 0; i < 6; i++)
        {
            coloramount[i].text = 0.ToString();
        }
        ResetObject();
        if (PhotonNetwork.IsMasterClient)
            GameStart();
    }
    void ResetObject()
    {
        cubeholder.transform.Rotate(-30.0f, 0.0f, 0.0f, Space.Self);
        diceset[0].Play("Dice1dft");
        diceset[1].Play("Dice2dft");
        diceset[2].Play("Dice3dft");

    }
    public void CoinSelect1K()
    {
        if(selectcoins)
        {
            soundManager.PlayClick();
            amountselector = 1;
            RunSelector(amountselector);
        }
    }
    public void CoinSelect5K()
    {
        if (selectcoins)
        {
            soundManager.PlayClick();
            amountselector = 2;
            RunSelector(amountselector);
        }
    }
    public void CoinSelect10K()
    {
        if (selectcoins)
        {
            soundManager.PlayClick();
            amountselector = 3;
            RunSelector(amountselector);
        }
    }
    void RunSelector(int coinspick)
    {
        int picked = coinspick - 1;
        for(int i = 0; i < 3; i++)
        {
            if(i == picked)
            {
                coinsselector[i].sprite = selected;
            }
            else
            {
                coinsselector[i].sprite = nonselect;
            }
        }
    }
    #region ColorSelect
    public void YellowClick()
    {
        if (selectcoins)
        {
            soundManager.PlayClick();
            if ((amountselector == 1 && plcoins > 1000) || (amountselector == 2 && plcoins > 5000) || (amountselector == 3 && plcoins > 10000))
            {
                YellowClicked();
            }
        }
    }
    void YellowClicked()
    {
       if(amountselector == 1)
        {
            color1 = color1 + 1000;
        }
       else if(amountselector == 2)
        {
            color1 = color1 + 5000;
        }
        else if (amountselector == 3)
        {
            color1 = color1 + 10000;
        }
        coloramount[0].text = color1.ToString();
        SubCoins(amountselector);
    }
    public void WhiteClick()
    {
        if (selectcoins)
        {
            soundManager.PlayClick();
            if ((amountselector == 1 && plcoins > 1000) || (amountselector == 2 && plcoins > 5000) || (amountselector == 3 && plcoins > 10000))
            {
                WhiteClicked();
            }
        }
    }
    void WhiteClicked()
    {
        if (amountselector == 1)
        {
            color2 = color2 + 1000;
        }
        else if (amountselector == 2)
        {
            color2 = color2 + 5000;
        }
        else if (amountselector == 3)
        {
            color2 = color2 + 10000;
        }
        coloramount[1].text = color2.ToString();
        SubCoins(amountselector);
    }
    public void GreenClick()
    {
        if (selectcoins)
        {
            soundManager.PlayClick();
            if ((amountselector == 1 && plcoins > 1000) || (amountselector == 2 && plcoins > 5000) || (amountselector == 3 && plcoins > 10000))
            {
                GreenClicked();
            }
        }
    }
    void GreenClicked()
    {
        if (amountselector == 1)
        {
            color3 = color3 + 1000;
        }
        else if (amountselector == 2)
        {
            color3 = color3 + 5000;
        }
        else if (amountselector == 3)
        {
            color3 = color3 + 10000;
        }
        coloramount[2].text = color3.ToString();
        SubCoins(amountselector);
    }
    public void VioletClick()
    {
        soundManager.PlayClick();
        if (selectcoins)
        {
            if ((amountselector == 1 && plcoins > 1000) || (amountselector == 2 && plcoins > 5000) || (amountselector == 3 && plcoins > 10000))
            {
                VioletClicked();
            }
        }
    }
    void VioletClicked()
    {
        if (amountselector == 1)
        {
            color4 = color4 + 1000;
        }
        else if (amountselector == 2)
        {
            color4 = color4 + 5000;
        }
        else if (amountselector == 3)
        {
            color4 = color4 + 10000;
        }
        coloramount[3].text = color4.ToString();
        SubCoins(amountselector);
    }
    public void RedClick()
    {
        soundManager.PlayClick();
        if (selectcoins)
        {
            if ((amountselector == 1 && plcoins > 1000) || (amountselector == 2 && plcoins > 5000) || (amountselector == 3 && plcoins > 10000))
            {
                RedClicked();
            }
        }
    }
    void RedClicked()
    {
        if (amountselector == 1)
        {
            color5 = color5 + 1000;
        }
        else if (amountselector == 2)
        {
            color5 = color5 + 5000;
        }
        else if (amountselector == 3)
        {
            color5 = color5 + 10000;
        }
        coloramount[4].text = color5.ToString();
        SubCoins(amountselector);
    }
    public void BlueClick()
    {
        soundManager.PlayClick();
        if (selectcoins)
        {
            if ((amountselector == 1 && plcoins > 1000) || (amountselector == 2 && plcoins > 5000) || (amountselector == 3 && plcoins > 10000))
            {
                BlueClicked();
            }
        }
    }
    void BlueClicked()
    {
        if (amountselector == 1)
        {
            color6 = color6 + 1000;
        }
        else if (amountselector == 2)
        {
            color6 = color6 + 5000;
        }
        else if (amountselector == 3)
        {
            color6 = color6 + 10000;
        }
        coloramount[5].text = color6.ToString();
        SubCoins(amountselector);
    }
    #endregion

    void SubCoins(int type)
    {
        if(type == 1)
        {
            plcoins = plcoins - 1000;
        }
        if(type == 2)
        {
            plcoins = plcoins - 5000;
        }
        if (type == 3)
        {
            plcoins = plcoins - 10000;
        }
        PlayerPrefs.SetInt("PlayerCoins", plcoins);
    }
    [PunRPC]
    public void GameQuit()
    {
        StartCoroutine(DisconnectAndLoad());
    }
    [PunRPC]
    IEnumerator DisconnectAndLoad()
    {
        //PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
        // while (PhotonNetwork.IsConnected)
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene("MainScene");
    }
}
