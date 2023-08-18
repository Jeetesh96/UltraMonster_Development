using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private Text _roomName;
    private Text RoomName
    {
        get { return _roomName; }
    }

    public void OnClick_CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 3 };

        if (PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default))
        {
            print("create CurrentRoom successfully sent.");
        }
        else
        {
            print("create CurrentRoom failed to send");
        }
    }
    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        print("create CurrentRoom failed: " + codeAndMessage[1]);
    }
}