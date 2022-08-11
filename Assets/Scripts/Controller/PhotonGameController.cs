using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;


namespace Controller
{
    public class PhotonGameController : PunCallbacksBaseController
	{
        public Action onPhotonDisconnect;
        public Action<Player> onPlayerEnteredRoom;
        public Action<Player> onPlayerLeftRoom;

        public void LeftRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.Log("OnPlayerEnteredRoom() " + other.NickName);
            onPlayerEnteredRoom?.Invoke(other);
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log("OnPlayerLeftRoom() " + other.NickName);
            onPlayerLeftRoom?.Invoke(other);
        }

        public override void OnLeftRoom()
        {
            onPhotonDisconnect?.Invoke();
            PhotonNetwork.Disconnect();
        }
    }
}



