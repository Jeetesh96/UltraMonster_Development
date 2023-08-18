using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectServerPusoy : MonoBehaviourPunCallbacks
{
    public LoadingScript instance;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        instance.LoadScene("PusoyRoom");
    }
}
