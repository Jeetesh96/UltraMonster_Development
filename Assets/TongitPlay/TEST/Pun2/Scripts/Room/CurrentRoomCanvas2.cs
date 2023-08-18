using UnityEngine;

public class CurrentRoomCanvas2 : MonoBehaviour
{
    [SerializeField]
    private PlayerListingMenu2 _playerListingsMenu;
    [SerializeField]
    private LeaveRoomMenu2 _leaveRoomMenu;



    private RoomCanvases2 _roomsCanvases;
    public void FirstInitialize(RoomCanvases2 canvases)
    {
        _roomsCanvases = canvases;
        _playerListingsMenu.FirstInitialize(canvases);
        _leaveRoomMenu.FirstInitialize(canvases);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}