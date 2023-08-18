using Photon.Pun;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviourPunCallbacks {
    public void OnClickStartsync()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.LoadLevel(2);

        PhotonNetwork.CurrentRoom.IsVisible = true;
    }
	
    public void OnClickStartDelayed()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel(2);
    }
}
