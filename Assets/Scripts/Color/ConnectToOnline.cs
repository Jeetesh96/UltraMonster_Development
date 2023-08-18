using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToOnline : MonoBehaviourPunCallbacks
{
    public LoadingScript instance;
    void Start()
    {
        //if (PhotonNetwork.IsConnected)
        //{
        //    instance.LoadScene("ColorGame");
        //}
        //else
            PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        instance.LoadScene("ColorRoom");
    }
}
