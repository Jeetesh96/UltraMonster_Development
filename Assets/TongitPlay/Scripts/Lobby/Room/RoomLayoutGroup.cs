using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomLayoutGroup : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject _roomListingPrefab;
    private GameObject RoomListingPrefab
    {
        get { return _roomListingPrefab; }
    }

    private List<RoomListing> _roomListingButtons = new List<RoomListing>();

    private List<RoomListing> RoomListingButtons
    {
        get { return _roomListingButtons; }
    }

    //public override void OnRoomListUpdate(List<RoomInfo> roomList)
    //{
    //    //int index = RoomListingButtons.FindIndex(x => x.RoomName == roomList);
    //    foreach (RoomInfo info in roomList)
    //    {
    //        //Removed from rooms list.
    //        if (info.RemovedFromList)
    //        {
    //            int index = _roomListingButtons.FindIndex(x => x.RoomName == info.Name);
    //            if (index != -1)
    //            {
    //                Destroy(_roomListingButtons[index].gameObject);
    //                _roomListingButtons.RemoveAt(index);
    //            }
    //        }

    //        //Added to rooms list.
    //        else
    //        {
    //            int index = _roomListingButtons.FindIndex(x => x.RoomName == info.Name);
    //            RoomListing roomListing = Instantiate(_roomListingPrefab);

    //            if (roomListing != null)
    //            {
    //                roomListing.SetRoomInfo(info);
    //                _roomListingButtons.Add(roomListing);
                    
    //            }
    //        }
    //    }
    //}

    //private void OnReceivedRoomListUpdate()
    //{
    //    RoomInfo[] rooms = PhotonNetwork.GetRoomList();

    //    foreach (RoomInfo CurrentRoom in rooms)
    //    {
    //        RoomReceived(CurrentRoom);
    //    }

    //    RemoveOldRooms();
    //}
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            RoomReceived(room);
        }

        RemoveOldRooms();
    }
    public void RoomReceived(RoomInfo room)
    {
        int index = RoomListingButtons.FindIndex(x => x.RoomName == room.Name);

        if (index == -1)
        {
            if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomListingObj = Instantiate(RoomListingPrefab);
                roomListingObj.transform.SetParent(transform, false);

                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
                RoomListingButtons.Add(roomListing);

                index = (RoomListingButtons.Count - 1);
            }
        }

        if (index != -1)
        {
            RoomListing roomListing = RoomListingButtons[index];
            roomListing.SetRoomNameText(room.Name);
            roomListing.Updated = true;
        }
    }

    private void RemoveOldRooms()
    {
        List<RoomListing> removeRooms = new List<RoomListing>();

        foreach (RoomListing roomListing in RoomListingButtons)
        {
            if (!roomListing.Updated)
                removeRooms.Add(roomListing);
            else
                roomListing.Updated = false;
        }

        foreach (RoomListing roomListing in removeRooms)
        {
            GameObject roomListingObj = roomListing.gameObject;
            RoomListingButtons.Remove(roomListing);
            Destroy(roomListingObj);
        }

    }

    public override void OnLeftRoom()
    {
        print("Back to lobby!");

        
    }

}