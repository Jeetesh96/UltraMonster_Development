using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerListingMenu2 : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerListing2 _playerListing;
    [SerializeField]
    private Text _readyUpText;

    private List<PlayerListing2> _listings = new List<PlayerListing2>();
    private RoomCanvases2 _roomsCanvases;
    private bool _ready = false;

    private void Awake()
    {
        GetCurrentRoomPlayers();
    }

    //public override void OnEnable()
    //{
    //    base.OnEnable();
    //    SetReadyUp(false);
    //    GetCurrentRoomPlayers();
    //}

    //public override void OnDisable()
    //{
    //    base.OnDisable();
    //    for (int i = 0; i < _listings.Count; i++)
    //        Destroy(_listings[i].gameObject);

    //    _listings.Clear();
    //}

    public void FirstInitialize(RoomCanvases2 canvases)
    {
        _roomsCanvases = canvases;
    }

    //private void SetReadyUp(bool state)
    //{
    //    _ready = state;
    //    if (_ready)
    //        _readyUpText.text = "R";
    //    else
    //        _readyUpText.text = "N";
    //}

    public override void OnLeftRoom()
    {
        _content.DestroyChildren();
    }

    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player player)
    {
        Debug.Log("Player Instantiate");
        int index = _listings.FindIndex(x => x.Player == player);
        if (index != -1)
        {

            _listings[index].SetPlayerInfo(player);
        }
        else
        {
            PlayerListing2 listing = Instantiate(_playerListing, _content);
            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                _listings.Add(listing);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listings.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {

            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }

    public void OnClick_StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel("GamePlay");
        }
    }
}