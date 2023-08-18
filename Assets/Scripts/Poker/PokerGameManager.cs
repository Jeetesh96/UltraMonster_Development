using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PokerGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<Sprite> cardset = new List<Sprite>();
    [SerializeField] private GameObject waitingtext;
    [SerializeField] private Image[] playercards;
    [SerializeField] private Image[] maincards;
    [SerializeField] private GameObject buttonset;
    [SerializeField] private Text totalworthtext;
    [SerializeField] private Text playercointext;
    [SerializeField] private Text playername;
    [SerializeField] private GameObject playerset2;
    [SerializeField] private Text winnertxt;
    [SerializeField] private OrderCheckPoker orderCheckPoker;
    [SerializeField] private Sprite dftcard;
    //testing purpose
    [SerializeField] private Text player2name;
    [SerializeField] private Text player2coins;
    [SerializeField] private Text playerinfo;
    [SerializeField] private Text player2info;
    public List<int> cardnumbers = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51 };
    int playercount = 0;
    int waiton = 0;
    bool waitingtimer = false;
    float waittime = 10f;
    int testcoins = 0;
    int playerturn = 0;
    bool playertimer = false;
    float playtime = 10f;
    int totalamount = 0;
    int player2coincnt = 50000;
    int optionselected = 0;
    bool maincardson = false;
    bool gameOn = false;
    int opponentchoice = 0;
    int roundcnt = 0;
    public int ordrcnt = 0;
    int setcount = 0;
    public bool gamebegin = false;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRndName();
        if (PhotonNetwork.IsMasterClient)
        {
            waitingtext.SetActive(true);
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("PlayerJoined", RpcTarget.All);
        }
    }
    void GenerateRndName()
    {
        //int rndid = Random.Range(1111, 9999);
        string name = PlayerPrefs.GetString("Username", "No name");
        PhotonNetwork.LocalPlayer.NickName = name;
        playername.text = name;
    }
    [PunRPC]
    public void PlayerJoined()
    {
        waitingtext.SetActive(false);
        playercount += 1;
        photonView.RPC("SetOpponent", RpcTarget.All, playercount);
        waiton += 1;
        if (waiton == 1)
            waitingtimer = true;

    }
    [PunRPC]
    public void SetOpponent(int players)
    {
        switch (playercount)
        {
            case 1:
                playerset2.SetActive(true);
                player2name.text = PhotonNetwork.PlayerListOthers[0].NickName;
                player2coins.text = player2coins.ToString();
                break;
        }
    }
    private void Update()
    {
        testcoins = PlayerPrefs.GetInt("PlayerCoins", 0);
        if (waitingtimer)
        {
            if (waittime > 0)
            {
                waittime -= Time.deltaTime;
                int newtime = Mathf.RoundToInt(waittime);
            }
            else
            {
                waitingtimer = false;
                waittime = 10f;
                GameStart();
            }
        }
        if (playertimer)
        {
            if (playtime > 0)
            {
                playtime -= Time.deltaTime;
                int newtime = Mathf.RoundToInt(playtime);
            }
            else
            {
                playertimer = false;
                playtime = 10f;
                // SelectFold();
            }
        }
        totalworthtext.text = totalamount.ToString();
        playercointext.text = testcoins.ToString();

        if (gameOn)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("Sendcoindetails", RpcTarget.Others, playername.text, testcoins);
            }
            else if(PhotonNetwork.LocalPlayer.ActorNumber == 2)
            {
                photonView.RPC("Sendcoindetails2", RpcTarget.MasterClient, playername.text, testcoins);
            }
        }
    }
    [PunRPC]
    public void Sendcoindetails2(string name, int coins)
    {
        if (player2name.text == name)
        {
            player2coincnt = coins;
            player2coins.text = player2coincnt.ToString();
        }
    }
    [PunRPC]
    public void Sendcoindetails(string name, int coins)
    {
        if (player2name.text == name)
        {
            player2coincnt = coins;
            player2coins.text = player2coincnt.ToString();
        }
    }
    void SelectFold()
    {
        FoldButton();
    }
    [PunRPC]
    public void RemoveCardList(int rndcard)
    {
        cardnumbers.Remove(rndcard);
    }
    void GameStart()
    {
        int newcoins = testcoins;
        newcoins -= 500;
        PlayerPrefs.SetInt("PlayerCoins", newcoins);
        gamebegin = true;
        totalamount = 1000;
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 2; i++)
            {
                int rndcard = Random.Range(0, cardnumbers.Count);
                int value = cardnumbers[rndcard];
                playercards[i].sprite = cardset[value];
                playercards[i].name = cardset[value].name;
                SetCardIndex(value, i);
                photonView.RPC("RemoveCardList", RpcTarget.All, value);
            }
            for (int i = 0; i < 2; i++)
            {
                int rndcard = Random.Range(0, cardnumbers.Count);
                int value = cardnumbers[rndcard];
                photonView.RPC("SetCards", RpcTarget.Others, value, i);
                photonView.RPC("RemoveCardList", RpcTarget.All, value);
            }
            Invoke("RoundStart", 2f);
        }
    }
    [PunRPC]
    public void SetCards(int cardtype, int cardno)
    {
        playercards[cardno].sprite = cardset[cardtype];
        playercards[cardno].name = cardset[cardtype].name;
        SetCardIndex(cardtype, cardno);
    }
    [PunRPC]
    public void PassRounCount(int rndcount)
    {
        roundcnt = rndcount;
    }
    [PunRPC]
    public void PassOrderCount(int odrcunt)
    {
        ordrcnt = odrcunt;
    }
    [PunRPC]
    public void VariablePass()
    {
        gameOn = true;
    }
    void RoundStart()
    {
        playerturn = 0;
        roundcnt = 1;
        ordrcnt = 1;
        photonView.RPC("PassRounCount", RpcTarget.Others, roundcnt);
        photonView.RPC("PassOrderCount", RpcTarget.Others, ordrcnt);
        
        gameOn = true;
        photonView.RPC("VariablePass", RpcTarget.Others);
        if (PhotonNetwork.IsMasterClient)
        {
            buttonset.SetActive(true);
            playertimer = true;
        }
    }
    [PunRPC]
    public void ShowWinner(int winner)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (winner == 1)
            {
                int newcoins = testcoins;
                newcoins += totalamount;
                totalamount = 0;
                PlayerPrefs.SetInt("PlayerCoins", newcoins);
                winnertxt.text = PhotonNetwork.LocalPlayer.NickName;
            }
            else if (winner == 2)
            {
                totalamount = 0;
                winnertxt.text = PhotonNetwork.PlayerListOthers[0].NickName;
            }
            else if (winner == 3)
            {
                int newcoins = testcoins;
                int amt = totalamount / 2;
                newcoins += amt;
                PlayerPrefs.SetInt("PlayerCoins", newcoins);
                winnertxt.text = "Draw";
            }
        }
        else
        {
            if (winner == 2)
            {
                testcoins += totalamount;
                totalamount = 0;
                winnertxt.text = PhotonNetwork.LocalPlayer.NickName;
            }
            else if (winner == 1)
            {
                totalamount = 0;
                winnertxt.text = PhotonNetwork.PlayerListOthers[0].NickName;
            }
            else if (winner == 3)
            {
                int newcoins = testcoins;
                int amt = totalamount / 2;
                newcoins += amt;
                PlayerPrefs.SetInt("PlayerCoins", newcoins);
                winnertxt.text = "Draw";
            }
        }
        Invoke("ResetGame", 3f);
    }
    void ResetGame()
    {
        for (int i = 0; i < 2; i++)
        {
            playercards[i].sprite = dftcard;
            playercards[i].name = dftcard.name;
        }
        for (int i = 0; i < 5; i++)
        {
            maincards[i].gameObject.SetActive(false);
        }
        playerinfo.text = "";
        player2info.text = "";
        winnertxt.text = "";
        buttonset.transform.GetChild(1).gameObject.SetActive(true);
        buttonset.transform.GetChild(2).gameObject.SetActive(false);
        buttonset.transform.GetChild(3).gameObject.SetActive(false);
        cardnumbers.Clear();
        cardnumbers = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51 };
        GameStart();
    }
    [PunRPC]
    public void Changeturn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (opponentchoice == 1 || optionselected == 1)
            {
                if (opponentchoice == 1)
                {
                    photonView.RPC("ShowWinner", RpcTarget.All, 2);
                }
                else
                {
                    photonView.RPC("ShowWinner", RpcTarget.All, 1);
                }

            }
            else
            {
                playerturn += 1;
                if (playerturn == 1)
                    photonView.RPC("SetPlayerTurn", RpcTarget.Others);
                else if (playerturn == 2)
                {
                    playerturn = 0;
                    roundcnt += 1;
                    photonView.RPC("PassRounCount", RpcTarget.Others, roundcnt);
                    SetNextRound();
                }
            }
        }
    }
    [PunRPC]
    public void ButtonInfo()
    {
        buttonset.transform.GetChild(1).gameObject.SetActive(false);
        buttonset.transform.GetChild(2).gameObject.SetActive(true);
    }
    [PunRPC]
    public void ButtonInfo2()
    {
        buttonset.transform.GetChild(3).gameObject.SetActive(true);
    }
    void SetNextRound()
    {
        if (roundcnt == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                int rndcard = Random.Range(0, cardnumbers.Count);
                int value = cardnumbers[rndcard];
                photonView.RPC("SetMainCards", RpcTarget.All, value, i);
            }
            if (PhotonNetwork.IsMasterClient)
            {
                buttonset.transform.GetChild(1).gameObject.SetActive(false);
                buttonset.transform.GetChild(2).gameObject.SetActive(true);
                photonView.RPC("ButtonInfo", RpcTarget.Others);
                buttonset.SetActive(true);
                playertimer = true;
            }
            ordrcnt = 2;
            photonView.RPC("PassOrderCount", RpcTarget.Others, ordrcnt);
        }
        if (roundcnt == 3)
        {
            int rndcard = Random.Range(0, cardnumbers.Count);
            int value = cardnumbers[rndcard];
            photonView.RPC("SetMainCards", RpcTarget.All, value, 3);
            if (PhotonNetwork.IsMasterClient)
            {
                buttonset.transform.GetChild(3).gameObject.SetActive(true);
                photonView.RPC("ButtonInfo2", RpcTarget.Others);
                buttonset.SetActive(true);
                playertimer = true;
            }
            ordrcnt = 3;
            photonView.RPC("PassOrderCount", RpcTarget.Others, ordrcnt);
        }
        if (roundcnt == 4)
        {  
            int rndcard = Random.Range(0, cardnumbers.Count);
            int value = cardnumbers[rndcard];
            photonView.RPC("SetMainCards", RpcTarget.All, value, 4);
            if (PhotonNetwork.IsMasterClient)
            {
                buttonset.SetActive(true);
                playertimer = true;
            }
            ordrcnt = 4;
            photonView.RPC("PassOrderCount", RpcTarget.Others, ordrcnt);
        }
        if (roundcnt == 5)
        {
            photonView.RPC("GetScore", RpcTarget.Others);
        }
    }
    [PunRPC]
    public void GetScore()
    {
        int score = orderCheckPoker.backorder;
        photonView.RPC("SendScore", RpcTarget.MasterClient, score);
    }
    [PunRPC]
    public void SendScore(int score)
    {
        int p2score = score;
        int p1score = orderCheckPoker.backorder;
        int winner = 0;
        if (p1score > p2score)
        {
            winner = 1;
        }
        else if (p2score > p1score)
        {
            winner = 2;
        }
        else if (p1score == p2score)
        {
            winner = 3;
        }
        photonView.RPC("ShowWinner", RpcTarget.All, winner);
    }
    [PunRPC]
    public void SetRoundCount(int count)
    {
        setcount = count;
    }
    [PunRPC]
    public void SetMainCards(int rndcard, int i)
    {
        Debug.Log("check");
        maincardson = true;
        maincards[i].sprite = cardset[rndcard];
        maincards[i].name = cardset[rndcard].name;
        SetCardIndex(rndcard, i);
        cardnumbers.Remove(rndcard);
        maincards[i].gameObject.SetActive(true);
    }
    [PunRPC]
    public void SetPlayerTurn()
    {
        buttonset.SetActive(true);
        playertimer = true;
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
        if (maincardson)
        {
            maincards[cardorder].GetComponent<CardData>().index = indexvalue;
            maincards[cardorder].GetComponent<CardData>().cardType = cardtype;
            maincards[cardorder].GetComponent<CardData>().spritenum = value;
        }
        else
        {
            playercards[cardorder].GetComponent<CardData>().index = indexvalue;
            playercards[cardorder].GetComponent<CardData>().cardType = cardtype;
            playercards[cardorder].GetComponent<CardData>().spritenum = value;
        }
    }
    public void FoldButton()
    {
        optionselected = 1;
        DoAction();
        buttonset.SetActive(false);
    }
    public void CheckButton()
    {
        optionselected = 2;
        DoAction();
        buttonset.SetActive(false);
    }
    public void RaiseButton()
    {
        optionselected = 3;
        DoAction();
        buttonset.SetActive(false);
    }
    public void AllinButton()
    {
        optionselected = 4;
        DoAction();
        buttonset.SetActive(false);
    }
    void DoAction()
    {
        playertimer = false;
        playtime = 10f;
        if (optionselected == 1)
        {
            playerinfo.text = "Fold";
            PassPlayerTurn();
        }
        if (optionselected == 2)
        {
            playerinfo.text = "Call";
            PassPlayerTurn();
        }
        if (optionselected == 3)
        {
            playerinfo.text = "Raise";
            testcoins -= 5000;
            photonView.RPC("PassAmount", RpcTarget.All, 5000);
            PassPlayerTurn();
        }
        if (optionselected == 4)
        {
            playerinfo.text = "All in";
            int coins = testcoins;
            testcoins = 0;
            photonView.RPC("PassAmount", RpcTarget.All, coins);
            PassPlayerTurn();
        }
        photonView.RPC("SendData", RpcTarget.Others, playername.text, optionselected);
    }
    [PunRPC]
    public void PassAmount(int amount)
    {
        totalamount += amount;
    }
    [PunRPC]
    public void SendData(string name, int selectednum)
    {
        if (player2name.text == name)
        {
            opponentchoice = selectednum;
            if (opponentchoice == 1)
            {
                player2info.text = "Fold";
            }
            if (opponentchoice == 2)
            {
                player2info.text = "Call";
            }
            if (opponentchoice == 3)
            {
                player2info.text = "Raise";
            }
            if (opponentchoice == 4)
            {
                player2info.text = "All in";
            }
        }
    }
    void PassPlayerTurn()
    {
        photonView.RPC("Changeturn", RpcTarget.MasterClient);
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

