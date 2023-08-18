using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PusoyGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Sprite[] cardset;
    [SerializeField] private Image[] player1Set;
    [SerializeField] private Image[] playerdd;
    [SerializeField] private Image[] player2Set;
    [SerializeField] private Image[] playerdd2;
    [SerializeField] private Image[] player3Set;
    [SerializeField] private Image[] playerdd3;
    public List<int> cardno = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51 };
    [SerializeField] private OrderCheck orderCheck;
    [SerializeField] private GameObject waitingtext;
    [SerializeField] private GameObject ordercards;
    [SerializeField] private TextMeshProUGUI timertext;
    bool droptimer = false;
    bool gameinprogress = false;
    float droptimercount = 30f;
    int p1back, p1middle, p1front, p2back, p2middle, p2front, p3back, p3middle, p3front;
    [SerializeField] private GameObject cardsComparep1;
    [SerializeField] private GameObject cardsComparep2;
    [SerializeField] private Image[] p1comparecards;
    [SerializeField] private Image[] p2comparecards;
    List<int> oppcardlist = new List<int>();
    List<int> p1cardlist = new List<int>();
    List<int> p2cardlist = new List<int>();
    [SerializeField] private Text[] scorep1;
    [SerializeField] private Text[] scorep2;
    [SerializeField] private GameObject bombp1;
    [SerializeField] private GameObject bombp2;
    [SerializeField] private TextMeshProUGUI winnertext;
    [SerializeField] private GameObject player2full;
    [SerializeField] private Text playername1;
    [SerializeField] private Text playername2;
    [SerializeField] private Text CoinsP1;
    [SerializeField] private Text CoinsP2;
    bool gameOn = false;
    int p1total, p2total;
    int winnernum;
    int testcoins;
  //  private SoundManager soundManager;

    private void Start()
    {
        string name = PlayerPrefs.GetString("Username", "No name");
        PhotonNetwork.LocalPlayer.NickName = name;
        //   soundManager = GameObject.FindWithTag("GameMusic").GetComponent<SoundManager>();
        //Testing();
        if (PhotonNetwork.IsMasterClient)
        {
            waitingtext.SetActive(true);
            playername1.text = PhotonNetwork.LocalPlayer.NickName;
            CoinsP1.text = PlayerPrefs.GetInt("PlayerCoins", 0).ToString();
        }
       if(!PhotonNetwork.IsMasterClient)
        {
            if(PhotonNetwork.LocalPlayer.ActorNumber == 2)
            {
                playername2.text = PhotonNetwork.LocalPlayer.NickName;
                playername1.text = PhotonNetwork.PlayerListOthers[0].NickName;
                photonView.RPC("PassInformToMaster", RpcTarget.MasterClient, 2);
            }
            if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
            {
                photonView.RPC("PassInformToMaster", RpcTarget.MasterClient, 3);
            }
        }
    }
    IEnumerator Demo()
    {
        for (int i = 0; i < 13; i++)
        {
            int newcard = Random.Range(0, cardno.Count);
            int value = cardno[newcard];
            player1Set[i].sprite = cardset[value];
            playerdd[i].sprite = cardset[value];
            player1Set[i].name = cardset[value].name;
            playerdd[i].name = cardset[value].name;
            SetCardIndex(value, i);
            // playerdd[i].GetComponent<CardIndex>().index = value;
            cardno.Remove(value);
            player1Set[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void Update()
    {
        if (droptimer)
        {
            if (droptimercount > 0)
            {
                droptimercount -= Time.deltaTime;
                int newtime = Mathf.RoundToInt(droptimercount);
                timertext.text = newtime.ToString();
            }
            else
            {
                droptimer = false;
                droptimercount = 30f;
            }
        }
        testcoins = PlayerPrefs.GetInt("PlayerCoins", 0);

        if (gameOn)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("Sendcoindetails", RpcTarget.Others, testcoins);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
            {
                photonView.RPC("Sendcoindetails2", RpcTarget.MasterClient, testcoins);
            }
        }
    }
    [PunRPC]
    public void Sendcoindetails2( int coins)
    {
        CoinsP1.text = coins.ToString();
        CoinsP2.text = PlayerPrefs.GetInt("PlayerCoins", 0).ToString();
    }
    [PunRPC]
    public void Sendcoindetails(int coins)
    {
        CoinsP1.text = PlayerPrefs.GetInt("PlayerCoins", 0).ToString();
        CoinsP2.text = coins.ToString();
    }
    [PunRPC]
    public void PassInformToMaster(int playercount)
    {
        playername2.text = PhotonNetwork.PlayerListOthers[0].NickName;
        waitingtext.SetActive(false);
        int count = playercount;
        if (gameinprogress == false)
        {
            GameStart(playercount);
            gameinprogress = true;
            photonView.RPC("ShowPlayer2", RpcTarget.All);
        }
        else
        {
            NewPlayerJoined(playercount);
        }
    }
    [PunRPC]
    public void ShowPlayer2()
    {
        player2full.SetActive(true);
        gameOn = true;
        testcoins -= 5000;
    }
    void NewPlayerJoined(int count)
    {

    }
    void Testing()
    {
        for (int i = 0; i < 13; i++)
        {
            int newcard = Random.Range(0, cardno.Count);
            int value = cardno[newcard];
            player1Set[i].sprite = cardset[value];
            playerdd[i].sprite = cardset[value];
            player1Set[i].name = cardset[value].name;
            playerdd[i].name = cardset[value].name;
            SetCardIndex(value, i);
            // playerdd[i].GetComponent<CardIndex>().index = value;
            cardno.Remove(value);

        }
        CardsToArray();
    }
    IEnumerator Player1Cards()
    {
        for (int i = 0; i < 13; i++)
        {
            int newcard = Random.Range(0, cardno.Count);
            int value = cardno[newcard];
            player1Set[i].sprite = cardset[value];
            playerdd[i].sprite = cardset[value];
            player1Set[i].name = cardset[value].name;
            playerdd[i].name = cardset[value].name;
            SetCardIndex(value, i);
            player1Set[i].gameObject.SetActive(true);
            player2Set[i].gameObject.SetActive(true);
            // playerdd[i].GetComponent<CardIndex>().index = value;
            cardno.Remove(value);
            yield return new WaitForSeconds(0.25f);
        }
    }
    IEnumerator Player2Cards()
    {
        for (int i = 0; i < 13; i++)
        {
            int newcard = Random.Range(0, cardno.Count);
            int value = cardno[newcard];
            photonView.RPC("SetCardsOthers", RpcTarget.Others, value, i);
            // playerdd[i].GetComponent<CardIndex>().index = value;  
            cardno.Remove(value);

            yield return new WaitForSeconds(0.25f);
        }
    }
        public void GameStart(int playerscount)
    {
        testcoins -= 5000;
        if (playerscount >= 2)
        { 
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(Player1Cards());
                StartCoroutine(Player2Cards());
                //for (int i = 0; i < 13; i++)
                //{
                //    int newcard = Random.Range(0, cardno.Count);
                //    int value = cardno[newcard];
                //    player1Set[i].sprite = cardset[value];
                //    playerdd[i].sprite = cardset[value];
                //    player1Set[i].name = cardset[value].name;
                //    playerdd[i].name = cardset[value].name;
                //    SetCardIndex(value, i);
                //    // playerdd[i].GetComponent<CardIndex>().index = value;
                //    cardno.Remove(value);
                //}
                //for (int i = 0; i < 13; i++)
                //{
                //    int newcard = Random.Range(0, cardno.Count);
                //    int value = cardno[newcard];
                //    photonView.RPC("SetCardsOthers", RpcTarget.Others, value, i);
                //    // playerdd[i].GetComponent<CardIndex>().index = value;
                //    cardno.Remove(value);
                //}
                
                photonView.RPC("CardsToArray", RpcTarget.All);
                Invoke("StartRound", 5f);
            }      
        }
    }
    [PunRPC]
    public void SetCardsOthers(int cardno, int i)
    {
        player2Set[i].sprite = cardset[cardno];
        playerdd[i].sprite = cardset[cardno];
        player2Set[i].name = cardset[cardno].name;
        playerdd[i].name = cardset[cardno].name;
        SetCardIndex(cardno, i);
        player2Set[i].gameObject.SetActive(true);
        player1Set[i].gameObject.SetActive(true);
    }
    void SetCardIndex(int value, int cardorder)
    {
        int indexvalue = 0;
        int cardtype = 0;
        switch (value)
        {
            case 0:
            case 13:
            case 26:
            case 39:
                indexvalue = 1;
                if (value == 0)
                    cardtype = 1;

                if (value == 13)
                    cardtype = 2;

                if (value == 26)
                    cardtype = 3;

                if (value == 39)
                    cardtype = 4;
                break;
            case 1:
            case 14:
            case 27:
            case 40:
                indexvalue = 2;
                if (value == 1)
                    cardtype = 1;

                if (value == 14)
                    cardtype = 2;

                if (value == 27)
                    cardtype = 3;

                if (value == 40)
                    cardtype = 4;
                break;
            case 2:
            case 15:
            case 28:
            case 41:
                indexvalue = 3;
                if (value == 2)
                    cardtype = 1;

                if (value == 15)
                    cardtype = 2;

                if (value == 28)
                    cardtype = 3;

                if (value == 41)
                    cardtype = 4;
                break;
            case 3:
            case 16:
            case 29:
            case 42:
                indexvalue = 4;
                if (value == 3)
                    cardtype = 1;

                if (value == 16)
                    cardtype = 2;

                if (value == 29)
                    cardtype = 3;

                if (value == 42)
                    cardtype = 4;
                break;
            case 4:
            case 17:
            case 30:
            case 43:
                indexvalue = 5;
                if (value == 4)
                    cardtype = 1;

                if (value == 17)
                    cardtype = 2;

                if (value == 30)
                    cardtype = 3;

                if (value == 43)
                    cardtype = 4;
                break;
            case 5:
            case 18:
            case 31:
            case 44:
                indexvalue = 6;
                if (value == 5)
                    cardtype = 1;

                if (value == 18)
                    cardtype = 2;

                if (value == 31)
                    cardtype = 3;

                if (value == 44)
                    cardtype = 4;
                break;
            case 6:
            case 19:
            case 32:
            case 45:
                indexvalue = 7;
                if (value == 6)
                    cardtype = 1;

                if (value == 19)
                    cardtype = 2;

                if (value == 32)
                    cardtype = 3;

                if (value == 45)
                    cardtype = 4;
                break;
            case 7:
            case 20:
            case 33:
            case 46:
                indexvalue = 8;
                if (value == 7)
                    cardtype = 1;

                if (value == 20)
                    cardtype = 2;

                if (value == 33)
                    cardtype = 3;

                if (value == 46)
                    cardtype = 4;
                break;
            case 8:
            case 21:
            case 34:
            case 47:
                indexvalue = 9;
                if (value == 8)
                    cardtype = 1;

                if (value == 21)
                    cardtype = 2;

                if (value == 34)
                    cardtype = 3;

                if (value == 47)
                    cardtype = 4;
                break;
            case 9:
            case 22:
            case 35:
            case 48:
                indexvalue = 10;
                if (value == 9)
                    cardtype = 1;

                if (value == 22)
                    cardtype = 2;

                if (value == 35)
                    cardtype = 3;

                if (value == 48)
                    cardtype = 4;
                break;
            case 10:
            case 23:
            case 36:
            case 49:
                indexvalue = 11;
                if (value == 10)
                    cardtype = 1;

                if (value == 23)
                    cardtype = 2;

                if (value == 36)
                    cardtype = 3;

                if (value == 49)
                    cardtype = 4;
                break;
            case 11:
            case 24:
            case 37:
            case 50:
                indexvalue = 12;
                if (value == 11)
                    cardtype = 1;

                if (value == 24)
                    cardtype = 2;

                if (value == 37)
                    cardtype = 3;

                if (value == 50)
                    cardtype = 4;
                break;
            case 12:
            case 25:
            case 38:
            case 51:
                indexvalue = 13;
                if (value == 12)
                    cardtype = 1;

                if (value == 25)
                    cardtype = 2;

                if (value == 38)
                    cardtype = 3;

                if (value == 51)
                    cardtype = 4;
                break;
        }
        playerdd[cardorder].GetComponent<CardIndex>().index = indexvalue;
        playerdd[cardorder].GetComponent<CardIndex>().cardType = cardtype;
        playerdd[cardorder].GetComponent<CardIndex>().spritenum = value;
    }
    [PunRPC]
    public void CardsToArray()
    {
        orderCheck.front.Add(playerdd[0]);
        orderCheck.front.Add(playerdd[1]);
        orderCheck.front.Add(playerdd[2]);
        orderCheck.middle.Add(playerdd[3]);
        orderCheck.middle.Add(playerdd[4]);
        orderCheck.middle.Add(playerdd[5]);
        orderCheck.middle.Add(playerdd[6]);
        orderCheck.middle.Add(playerdd[7]);
        orderCheck.back.Add(playerdd[8]);
        orderCheck.back.Add(playerdd[9]);
        orderCheck.back.Add(playerdd[10]);
        orderCheck.back.Add(playerdd[11]);
        orderCheck.back.Add(playerdd[12]);
    }

    void StartRound()
    {
        photonView.RPC("StartDrop", RpcTarget.All);
    }
    [PunRPC]
    public void StartDrop()
    {
        ordercards.SetActive(true);
        droptimer = true;
    }

    public void DragDropDone()
    {
        ordercards.SetActive(false);
        SetNewCards();
        int forder = orderCheck.frontorder;
        int morder = orderCheck.middleorder;
        int border = orderCheck.backorder;
        if (PhotonNetwork.IsMasterClient)
        {
            ResultCheck(forder, morder, border);
        }
        else if(PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            photonView.RPC("SendToMaster", RpcTarget.MasterClient, forder, morder, border);
        }
    }
    void ResultCheck(int fvalue, int mvalue, int bvalue)
    {
        p1front = fvalue;
        p1middle = mvalue;
        p1back = bvalue;
        
    }
    [PunRPC]
    public void SendToMaster(int fvalue, int mvalue, int bvalue)
    {
        p2front = fvalue;
        p2middle = mvalue;
        p2back = bvalue;
        StartComparing();
    }
    void SetNewCards()
    {
        CardSprites();
    }
    void CardSprites()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 13; i++)
            {
                int value = orderCheck.cardSpritelist[i];
                Debug.Log("cardsprite" + value);
                player1Set[i].sprite = cardset[value];
            }
        }
        else if(PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            for (int i = 0; i < 13; i++)
            {
                int value = orderCheck.cardSpritelist[i];
                Debug.Log("cardsprite" + value);
                player2Set[i].sprite = cardset[value];
                photonView.RPC("PassCardList", RpcTarget.MasterClient, value);
                
            }
        }
    }
    [PunRPC]
    public void PassCardList(int value)
    {
        oppcardlist.Add(value);
    }
    void StartComparing()
    {
        foreach(int i in orderCheck.cardSpritelist)
        {
            photonView.RPC("SetP1list", RpcTarget.All, i);
        }
        foreach (int i in oppcardlist)
        {
            photonView.RPC("SetP2list", RpcTarget.All, i);
        }
        photonView.RPC("SetCompareCards", RpcTarget.All);
    }
    [PunRPC]
    public void SetP1list(int num)
    {
        p1cardlist.Add(num);
    }
    [PunRPC]
    public void SetP2list(int num)
    {
        p2cardlist.Add(num);
    }
    [PunRPC]
    public void SetCompareCards()
    {
        for (int i = 0; i < 13; i++)
        {
            int value = p1cardlist[i];
            int value2 = p2cardlist[i];
            p1comparecards[i].sprite = cardset[value];
            p2comparecards[i].sprite = cardset[value2];
        }

        cardsComparep1.SetActive(true);
        cardsComparep2.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            StartChecking();
        }
    }
    [PunRPC]
    public void StartChecking()
    {
        //back
        if(p1back > p2back)
        {
            p1total += 1;
            photonView.RPC("PassScoreBack", RpcTarget.All, 1, 1);
        }
        else if(p1back < p2back)
        {
            p2total += 1;
            photonView.RPC("PassScoreBack", RpcTarget.All, 2, 1);
        }
        else if(p1back == p2back)
        {
            photonView.RPC("PassScoreBack", RpcTarget.All, 3, 1);
        }
        Invoke("MiddleCheck", 1f);
    }
    void MiddleCheck()
    {
        if (p1middle > p2middle)
        {
            p1total += 1;
            photonView.RPC("PassScoreBack", RpcTarget.All, 1, 2);
        }
        else if (p1middle < p2middle)
        {
            p2total += 1;
            photonView.RPC("PassScoreBack", RpcTarget.All, 2, 2);
        }
        else if (p1middle == p2middle)
        {
            photonView.RPC("PassScoreBack", RpcTarget.All, 3, 2);
        }
        Invoke("FrontCheck", 1f);
    }
    void FrontCheck()
    {
        if (p1front > p2front)
        {
            p1total += 1;
            photonView.RPC("PassScoreBack", RpcTarget.All, 1, 3);
        }
        else if (p1front < p2front)
        {
            p2total += 1;
            photonView.RPC("PassScoreBack", RpcTarget.All, 2, 3);
        }
        else if (p1front == p2front)
        {
            photonView.RPC("PassScoreBack", RpcTarget.All, 3, 3);
        }
        Invoke("CheckFinalScore", 1f);
    }
    [PunRPC]
    public void PassScoreBack(int winner, int round)
    {
       // soundManager.RoundBell();
        if(winner == 1)
        {
            if(round == 1)
            {
                scorep1[0].color = Color.green;
                scorep1[0].text = "2000";
                scorep2[0].color = Color.red;
                scorep2[0].text = "0";
            }
            if(round == 2)
            {
                scorep1[1].color = Color.green;
                scorep1[1].text = "2000";
                scorep2[1].color = Color.red;
                scorep2[1].text = "0";
            }
            if (round == 3)
            {
                scorep1[2].color = Color.green;
                scorep1[2].text = "2000";
                scorep2[2].color = Color.red;
                scorep2[2].text = "0";
            }
        }
        else if(winner == 2)
        {
            if (round == 1)
            {
                scorep2[0].color = Color.green;
                scorep2[0].text = "2000";
                scorep1[0].color = Color.red;
                scorep1[0].text = "0";
            }
            if (round == 2)
            {
                scorep2[1].color = Color.green;
                scorep2[1].text = "2000";
                scorep1[1].color = Color.red;
                scorep1[1].text = "0";
            }
            if (round == 3)
            {
                scorep2[2].color = Color.green;
                scorep2[2].text = "2000";
                scorep1[2].color = Color.red;
                scorep1[2].text = "0";
            }
        }
        if (winner == 3)
        {
            if (round == 1)
            {
                scorep1[0].color = Color.yellow;
                scorep1[0].text = "1000";
                scorep2[0].color = Color.yellow;
                scorep2[0].text = "1000";
            }
            if (round == 2)
            {
                scorep1[1].color = Color.yellow;
                scorep1[1].text = "1000";
                scorep2[1].color = Color.yellow;
                scorep2[1].text = "1000";
            }
            if (round == 3)
            {
                scorep1[2].color = Color.yellow;
                scorep1[2].text = "1000";
                scorep2[2].color = Color.yellow;
                scorep2[2].text = "1000";
            }
        }
    }

    void CheckFinalScore()
    {
        if(p1total > p2total)
        {
            photonView.RPC("PlayBombAnim", RpcTarget.All, 2);
            winnernum = 1;
        }
        else if (p1total < p2total)
        {
            photonView.RPC("PlayBombAnim", RpcTarget.All, 1);
            winnernum = 2;
        }
        else if (p1total == p2total)
        {
            winnernum = 3;
        }
        Invoke("Result", 2f);
    }
    [PunRPC]
    public void PlayBombAnim(int loser)
    {
        if(loser == 2)
        {
            bombp2.SetActive(true);
        }
        else if(loser == 1)
        {
            bombp1.SetActive(true);
        }
       // soundManager.Bombblast();
        Invoke("AnimOff", 2f);
    }
    void AnimOff()
    {
        bombp1.SetActive(false);
        bombp2.SetActive(false);
    }
    void Result()
    {
        photonView.RPC("ShowResult", RpcTarget.All, winnernum);
    }
    [PunRPC]
    public void ShowResult(int winner)
    {
        // soundManager.Applause();
        if (PhotonNetwork.IsMasterClient)
        {
            if (winner == 1)
            {
                int newcoins = testcoins;
                newcoins += 10000;
                PlayerPrefs.SetInt("PlayerCoins", newcoins);
                winnertext.text = PhotonNetwork.LocalPlayer.NickName;
            }
            else if (winner == 2)
            {
                winnertext.text = PhotonNetwork.PlayerListOthers[0].NickName;
            }
            else if (winner == 3)
            {
                int newcoins = testcoins;
                newcoins += 5000;
                PlayerPrefs.SetInt("PlayerCoins", newcoins);
                winnertext.text = "Draw";
            }
        }
        else
        {
            if (winner == 2)
            {
                int newcoins = testcoins;
                newcoins += 10000;
                PlayerPrefs.SetInt("PlayerCoins", newcoins);
                winnertext.text = PhotonNetwork.LocalPlayer.NickName;
            }
            else if (winner == 1)
            {
                winnertext.text = PhotonNetwork.PlayerListOthers[0].NickName;
            }
            else if (winner == 3)
            {
                int newcoins = testcoins;
                newcoins += 5000;
                PlayerPrefs.SetInt("PlayerCoins", newcoins);
                winnertext.text = "Draw";
            }
        }
        Invoke("ResetGame", 2f);
    }
    void ResetGame()
    {
        for(int i = 0; i< 13; i++)
        {
            player1Set[i].gameObject.SetActive(false);
            player2Set[i].gameObject.SetActive(false);
        }
        winnertext.text = "";
        cardsComparep1.SetActive(false);
        cardsComparep2.SetActive(false);
        for(int i =0; i <= 2; i++)
        {
            scorep1[i].text = "";
            scorep2[i].text = "";
        }
        oppcardlist.Clear();
        p1cardlist.Clear();
        p2cardlist.Clear();
        cardno.Clear();
        cardno = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51 };
        if (PhotonNetwork.IsMasterClient)
        {
            GameStart(2);
        }
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
