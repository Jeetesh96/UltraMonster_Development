using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TestConnect : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Debug.Log("Connecting to server...");
        PhotonNetwork.SerializationRate = 5;//10.
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon. " ,this);
        //MasterManager.DebugConsole.AddText("Connected to Photon.", this);
        Debug.Log("My Nick Name is "+PhotonNetwork.LocalPlayer.NickName, this);
        PhotonNetwork.JoinLobby();

        //if (!PhotonNetwork.InLobby)
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server for reason " + cause.ToString(), this);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
    }
}
