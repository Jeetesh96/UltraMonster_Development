using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOrJoinRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private CreateRoomMenu2 _createRoomMenu;
    [SerializeField]
    private RoomListingsMenu2 _roomListingsMenu;

    private RoomCanvases2 _roomsCanvases;
    public void FirstInitialize(RoomCanvases2 canvases)
    {
        _roomsCanvases = canvases;
        _createRoomMenu.FirstInitialize(canvases);
        _roomListingsMenu.FirstInitialize(canvases);
    }
}
