using Model;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

namespace Controller
{
    public class PhotonGameConnectionController: PunCallbacksBaseController
    {
        public Action<string> onPhotonConnect;
        public Action onPhotonRoomJoin;
        public Action<string> onPhotonRandomRoomJoinFailed;
        public Action<DisconnectCause> onPhotonDisconnect;

        private bool _isConnecting;
        private string _gameVersion = "1";
        private byte _maxPlayersPerRoom;

		public PhotonGameConnectionController(byte maxPlayersPerRoom)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
			_maxPlayersPerRoom = maxPlayersPerRoom;
        }

		public void SetNickName(string nickName)
        {
			PhotonNetwork.NickName = nickName;
        }
        public void Connect()
        {
            _isConnecting = true;

            if (PhotonNetwork.IsConnected)
            {
				Debug.Log("IsConnected Before");
				PhotonNetwork.JoinRandomOrCreateRoom();
			}
            else
            {
				Debug.Log("Connect");
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = _gameVersion;
            }
        }

        public override void OnConnected()
        {
            base.OnConnected();
			Debug.Log("OnConnected");
		}
        public override void OnConnectedToMaster()
		{
			base.OnConnectedToMaster();
			Debug.Log("OnConnectedToMaster");
			if (_isConnecting)
			{
				onPhotonConnect?.Invoke("Connect to Photon");
				PhotonNetwork.JoinRandomOrCreateRoom();
			}
		}

		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			onPhotonRandomRoomJoinFailed?.Invoke(message);

			PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = _maxPlayersPerRoom });
		}

		public override void OnJoinRoomFailed(short returnCode, string message)
		{
			PhotonNetwork.JoinRandomOrCreateRoom();
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			onPhotonDisconnect?.Invoke(cause);
			_isConnecting = false;
		}

		public override void OnJoinedRoom()
		{
			onPhotonRoomJoin?.Invoke();
		}

		protected override void OnDispose()
		{
			base.OnDispose();
			onPhotonConnect = null;
			onPhotonRandomRoomJoinFailed = null;
			onPhotonRoomJoin = null;
			onPhotonDisconnect = null;
		}
	 

	}
}


