using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LeaveRoomMenu2 : MonoBehaviour
{
    private RoomCanvases2 _roomsCanvases;
    public void FirstInitialize(RoomCanvases2 canvases)
    {
        _roomsCanvases = canvases;
    }
    public void OnClick_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        _roomsCanvases.CurrentRoomCanvas.Hide();
    }
}
