using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomListingsMenu2 : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private RoomListing2 _roomListing;

    private List<RoomListing2> _listings = new List<RoomListing2>();

    private RoomCanvases2 _roomsCanvases;
    public void FirstInitialize(RoomCanvases2 canvases)
    {
        _roomsCanvases = canvases;
    }
    public override void OnJoinedRoom()
    {
        _roomsCanvases.CurrentRoomCanvas.Show();
        _content.DestroyChildren();
        _listings.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            //Removed from rooms list when exit or close the room
            if (info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);

                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }

            //Added to rooms list or update if exist or close the room

            else
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);

                if (index == -1)
                {
                    RoomListing2 listing = Instantiate(_roomListing, _content);

                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        _listings.Add(listing);
                    }
                }
                else if (index != -1)
                {
                    _listings[index].SetRoomInfo(info);
                }
            }
        }
    }

}