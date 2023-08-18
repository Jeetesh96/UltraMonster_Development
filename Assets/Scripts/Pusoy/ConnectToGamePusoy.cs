using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class ConnectToGamePusoy : MonoBehaviourPunCallbacks
{
    int totalNoofPlayer;
    int maxplayers = 10;
    //[SerializeField] private TextMeshProUGUI[] playername;
    //[SerializeField] private TextMeshProUGUI timertext;
    //bool waitingtimerOn = false;
    //float timer = 30f;
    //[SerializeField] private GameObject waitingset;
    //[SerializeField] private GameObject oops;
    //List<string> pnames = new List<string>();
    //List<int> pPicno = new List<int>();
    //[SerializeField] private Image[] searching;
    //[SerializeField] private Sprite[] avatarset;

    public static ConnectToGamePusoy instance;
    //[SerializeField] private PusoyGameManager gameManager;
    public LoadingScript lscript;


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void Start()
    {
        instance = this;
        JoinRoom();
        //waitingtimerOn = true;
    }
    void Update()
    {

    }
    public void JoinRoom()
    {
        Debug.Log("Checking");
        // RoomOptions options = new RoomOptions();
        // options.MaxPlayers = 2;
        //PhotonNetwork.JoinOrCreateRoom("basic", options, TypedLobby.Default);
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log(message);
        CreateJoinRoomGame();
    }
    void CreateJoinRoomGame()
    {
        string randomRoomName = "Room" + Random.Range(0, 10000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 10;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }
    public override void OnCreatedRoom()
    {

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {

    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            lscript.LoadScene("PusoyGame");
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            //lscript.LoadScene("ColorGame");
        }
    }
    //[PunRPC]
    //public void SetTimer(float timervalue)
    //{
    //    timer = timervalue;
    //}
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        totalNoofPlayer = PhotonNetwork.PlayerList.Length;
        Debug.Log(totalNoofPlayer);

        newPlayer.NickName = PlayerPrefs.GetString("Username", "No name");
        Debug.Log("name" + PhotonNetwork.LocalPlayer.NickName);
        if (totalNoofPlayer == 2)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
            {
                PlayerPrefs.SetInt("PlayersCount", 2);
                lscript.LoadScene("PusoyGame");
            }
        }
        else if (totalNoofPlayer == 3)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
            {
                PlayerPrefs.SetInt("PlayersCount", 3);
                lscript.LoadScene("PusoyGame");
            }
        }
        //else if (totalNoofPlayer == 4)
        //{
        //    playername[3].text = newPlayer.NickName;
        //}
        //if (totalNoofPlayer == maxplayers)
        //{
        //    if (PhotonNetwork.IsMasterClient)
        //    {
        //        GoToGame();
        //        photonView.RPC("GoToGame", RpcTarget.Others);
        //    }
        //}
    }

    //[PunRPC]
    //public void GoToGame()
    //{
    //    waitingtimerOn = false;
    //    gameManager.GameStart(totalNoofPlayer);
    //}
    //[PunRPC]
    //public void BotMode1()
    //{
    //    waitingtimerOn = false;
    //    SceneManager.LoadScene("GameScene-PlayOnlineBotMode1");
    //}
    //[PunRPC]
    //public void BotMode2()
    //{
    //    waitingtimerOn = false;
    //    SceneManager.LoadScene("GameScene-PlayOnlineBotMode2");
    //}

    [PunRPC]
    public void PlayerSelection()
    {
        Player[] players = PhotonNetwork.PlayerList;
        Debug.Log("PlayerSelectionCheck");
        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log("Actornumber:" + players[i].ActorNumber + "playernickname:" + players[i].NickName);
            Debug.Log("NextPlayeris:" + players[i].GetNext().NickName);
        }
    }

    public void CancelClick()
    {
        // SceneManager.LoadScene("GameSelect");
        // PhotonNetwork.Disconnect();
    }
    //[PunRPC]
    //public void PlayerNameBroadcast(int plcount, string pname, int pic)
    //{
    //    pnames.Add(pname);
    //    pPicno.Add(pic);
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        foreach (string i in pnames)
    //        {
    //            photonView.RPC("SetNames", RpcTarget.Others, i);
    //        }
    //        foreach (int i in pPicno)
    //        {
    //            photonView.RPC("SetPic", RpcTarget.Others, i);
    //        }
    //        photonView.RPC("SetnamesAll", RpcTarget.All, plcount);
    //    }
    //}
    //[PunRPC]
    //public void SetNames(string name)
    //{
    //    pnames.Add(name);
    //}
    //[PunRPC]
    //public void SetPic(int no)
    //{
    //    pPicno.Add(no);
    //}
    //[PunRPC]
    //public void SetnamesAll(int plcount)
    //{
    //    for (int i = 0; i <= plcount; i++)
    //    {
    //        playername[i].text = pnames[i];
    //        searching[i].sprite = avatarset[pPicno[i]];
    //        if (i == 0)
    //        {
    //            PlayerPrefs.SetString("Playername1", pnames[0]);
    //            PlayerPrefs.SetInt("PlayerPic1", pPicno[0]);
    //        }
    //        if (i == 1)
    //        {
    //            PlayerPrefs.SetString("Playername2", pnames[1]);
    //            PlayerPrefs.SetInt("PlayerPic2", pPicno[1]);
    //        }
    //        if (i == 2)
    //        {
    //            PlayerPrefs.SetString("Playername3", pnames[2]);
    //            PlayerPrefs.SetInt("PlayerPic3", pPicno[2]);
    //        }
    //        if (i == 3)
    //        {
    //            PlayerPrefs.SetString("Playername4", pnames[3]);
    //            PlayerPrefs.SetInt("PlayerPic4", pPicno[3]);
    //        }
    //    }
    //}
}



