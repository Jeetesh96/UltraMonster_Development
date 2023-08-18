using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class LobbyNetwork : MonoBehaviourPunCallbacks
{

    public void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connecting to server...");
            PhotonNetwork.ConnectUsingSettings();//("Remy");
        }
    }

    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("Reconnecting the Server");
        PhotonNetwork.Reconnect();
    }

    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    print("Disconnected from server for reason " + cause.ToString());
    //}

    //public override void OnConnectedToMaster()
    //{
    //    print("Connected to master.");
    //    PhotonNetwork.JoinLobby(TypedLobby.Default);
    //    PhotonNetwork.AutomaticallySyncScene = false;
    //}
    public override void OnConnectedToMaster()
    {
        print("Connected to master.");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        PhotonNetwork.AutomaticallySyncScene = false;
    }

}

