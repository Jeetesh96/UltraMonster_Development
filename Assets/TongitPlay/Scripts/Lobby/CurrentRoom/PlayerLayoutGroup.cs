using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerLayoutGroup : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject _playerListingPrefab;
    private GameObject PlayerLisitngPrefab
    {
        get { return _playerListingPrefab; }
    }

    private List<PlayerListing> _playerListings = new List<PlayerListing>();
    private List<PlayerListing> PlayerListings
    {
        get { return _playerListings; }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonNetwork.LeaveRoom();
        MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
    }

    public override void OnJoinedRoom()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }


        MainCanvasManager.Instance.CurrentRoomCanvas.transform.SetAsLastSibling();

        Player[] photonPlayers = PhotonNetwork.PlayerList;
        for (int i = 0; i < photonPlayers.Length; i++)
        {
            PlayerJoinedRoom(photonPlayers[i]);
        }
    }
    private void OnPhotonPlayerConnected(Player photonPlayer)
    {
        PlayerJoinedRoom(photonPlayer);
    }

    private void OnPhotonPlayerDisconnected(Player photonPlayer)
    {
        PlayerLeftRoom(photonPlayer);
    }

    private void PlayerJoinedRoom(Player photonPlayer)
    {
        if (photonPlayer == null)
            return;
        PlayerLeftRoom(photonPlayer);

        GameObject playerListingObj = Instantiate(PlayerLisitngPrefab);
        playerListingObj.transform.SetParent(transform, false);

        PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing>();
        playerListing.ApplyPhotonPlayer(photonPlayer);

        PlayerListings.Add(playerListing);
    }

    private void PlayerLeftRoom(Player photonPlayer)
    {
        int index = PlayerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if (index != -1)
        {
            Destroy(PlayerListings[index].gameObject);
            PlayerListings.RemoveAt(index);
        }
    }

    public void OnClickRoomState()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.CurrentRoom.IsOpen = !PhotonNetwork.CurrentRoom.IsOpen;
        PhotonNetwork.CurrentRoom.IsVisible = PhotonNetwork.CurrentRoom.IsOpen;
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();

        MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
    }

    
}
