using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

namespace Controller
{
    public class PunCallbacksBaseController : BaseController, IConnectionCallbacks, IMatchmakingCallbacks, IInRoomCallbacks, ILobbyCallbacks, IWebRpcCallback, IErrorInfoCallback
    {
        public PunCallbacksBaseController()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }
        protected override void OnDispose()
        {
            base.OnDispose();
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public virtual void OnConnected()
        {
        }

        public virtual void OnConnectedToMaster()
        {
        }

        public virtual void OnCreatedRoom()
        {
        }

        public virtual void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        public virtual void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        public virtual void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public virtual void OnDisconnected(DisconnectCause cause)
        {
        }

        public virtual void OnErrorInfo(ErrorInfo errorInfo)
        {
        }

        public virtual void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public virtual void OnJoinedLobby()
        {
        }

        public virtual void OnJoinedRoom()
        {
        }

        public virtual void OnJoinRandomFailed(short returnCode, string message)
        {
        }

        public virtual void OnJoinRoomFailed(short returnCode, string message)
        {
        }

        public virtual void OnLeftLobby()
        {
        }

        public virtual void OnLeftRoom()
        {
        }

        public virtual void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }

        public virtual void OnMasterClientSwitched(Player newMasterClient)
        {
        }

        public virtual void OnPlayerEnteredRoom(Player newPlayer)
        {
        }

        public virtual void OnPlayerLeftRoom(Player otherPlayer)
        {
        }

        public virtual void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
        }

        public virtual void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        public virtual void OnRoomListUpdate(List<RoomInfo> roomList)
        {
        }

        public virtual void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
        }

        public virtual void OnWebRpcResponse(OperationResponse response)
        {
        }
    }
}


