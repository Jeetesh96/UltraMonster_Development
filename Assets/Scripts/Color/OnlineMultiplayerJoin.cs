using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class OnlineMultiplayerJoin : MonoBehaviourPunCallbacks
{
    int totalNoofPlayer;
    int maxplayers = 20;
    //List<string> pnames = new List<string>();
    //List<int> pPicno = new List<int>();
    //[SerializeField] private Image[] searching;
    //[SerializeField] private Sprite[] avatarset;

    public static OnlineMultiplayerJoin instance;
    public LoadingScript lscript;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void Start()
    {
        instance = this;
        //if (PhotonNetwork.InRoom)
        //{
        //    Debug.Log("why am i In a room in start?");
        //    PhotonNetwork.LeaveRoom();
        //}
        //if (PhotonNetwork.InLobby)
        //{
        //    Debug.Log("why am i In a Lobby in start?");
        //    PhotonNetwork.LeaveLobby();
        //}

        JoinRoom();
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
        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = (byte)maxplayers });
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
        roomOptions.MaxPlayers = 20;

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
            lscript.LoadScene("ColorGame");
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            lscript.LoadScene("ColorGame");
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        totalNoofPlayer = PhotonNetwork.PlayerList.Length;
        Debug.Log(totalNoofPlayer);

        newPlayer.NickName = PlayerPrefs.GetString("Username", "No name");
        Debug.Log("name" + PhotonNetwork.LocalPlayer.NickName);
    }
  

    public void CancelClick()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        //PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
        // while (PhotonNetwork.IsConnected)
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + "has left the game");
        // playerleft = otherPlayer.ActorNumber;
        //photonView.RPC("RemovePlayerData", RpcTarget.Others, playerleft);
    }
}

