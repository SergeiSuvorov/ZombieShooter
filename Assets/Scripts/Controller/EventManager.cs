using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using Tools;
using UnityEngine;


namespace Controller
{
    public class EventManager : BaseController, IOnEventCallback
    {
        private EventUIController _eventUIController;
        private ResultTableController _resultTableController;
        public Action onOwnerPlayerDead;
        public EventManager(Transform _placeForUi, UpdateManager updateManager)
        {
            PhotonNetwork.AddCallbackTarget(this);
            _eventUIController = new EventUIController(_placeForUi, updateManager);
            AddController(_eventUIController);

            _resultTableController = new ResultTableController(_placeForUi);
            AddController(_resultTableController);
        }

        protected override void OnDispose()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            base.OnDispose();
        }

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;
            if (eventCode == PhotonEvenCodeList.PlayerRegisterCode)
            {
                object[] data = (object[])photonEvent.CustomData;
                Player player = (Player)data[0];
                PlayerRegistrateEvent( player);
            }
            if (eventCode == PhotonEvenCodeList.PlayerKillsPlayerCode)
            {
                object[] data = (object[])photonEvent.CustomData;
                Player killer = (Player)data[0];
                Player killed = (Player)data[1];
                PlayerKillPlayerEvent(killer, killed);
            }
            if (eventCode == PhotonEvenCodeList.PlayerKillsZombieCode)
            {
                object[] data = (object[])photonEvent.CustomData;
                Player player = (Player)data[0];
                PlayerKillMonsterEvents(player);
            }
            if (eventCode == PhotonEvenCodeList.MonsterKillsPlayerCode)
            {
                object[] data = (object[])photonEvent.CustomData;
                Player player = (Player)data[0];
                MonsterKillPlayerEvent(player);
            }
            if (eventCode == PhotonEvenCodeList.ZombieRegisterCode)
            {
                object[] data = (object[])photonEvent.CustomData;
                int index = (int)data[0];
            }
        }

        private void PlayerRegistrateEvent(Player player)
        {
            var message = $"Player {player.NickName} comming to game";
            _eventUIController.ShowMessage(message, Color.green);
            Debug.Log(PhotonNetwork.LocalPlayer.UserId==player.UserId);
            _resultTableController.AddRecord(player);
        }

        private void PlayerKillPlayerEvent(Player killer, Player killed)
        {
            _resultTableController.ChangeRecordPlayerKill(killer, 1);
            var message = $"Player {killer.NickName} kill {killed.NickName}";
            _eventUIController.ShowMessage(message, Color.red);
            if (PhotonNetwork.LocalPlayer.UserId == killed.UserId)
            {
                onOwnerPlayerDead?.Invoke();
                _resultTableController.CreateTableController();
            };
        }

        private void PlayerKillMonsterEvents(Player player)
        {
            _resultTableController.ChangeRecordZombieKill(player, 1);
            var message = $"Player {player.NickName} kill Zombie";
            _eventUIController.ShowMessage(message, Color.blue);
        }

        private void MonsterKillPlayerEvent(Player player)
        {
            
            var message = $"Player  {player.NickName} was killed by zombie";
            _eventUIController.ShowMessage(message, Color.red);

            if (PhotonNetwork.LocalPlayer.UserId == player.UserId)
            {
                onOwnerPlayerDead?.Invoke();
                _resultTableController.CreateTableController();
            };
        }

        public ResultTableRecord GetResult(Player player)
        {
            return _resultTableController.GetRecord(player);
        }

        public void EndGame()
        {
            _resultTableController.CreateTableController();
        }
    }
}



