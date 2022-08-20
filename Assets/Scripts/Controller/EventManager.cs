using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;


namespace Controller
{
    public class EventManager : BaseController, IOnEventCallback
    {
        public Action onOwnerPlayerDead;
        public EventManager()
        {
            PhotonNetwork.AddCallbackTarget(this);
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
            Debug.Log($"Player {player.NickName} comming to game");
            Debug.Log(PhotonNetwork.LocalPlayer.UserId==player.UserId);
        }

        private void PlayerKillPlayerEvent(Player killer, Player killed)
        {
            Debug.Log($"Player {killer.NickName} kill {killed.NickName}");
            if (PhotonNetwork.LocalPlayer.UserId == killed.UserId)
            {
                onOwnerPlayerDead?.Invoke();
            };
        }

        private void PlayerKillMonsterEvents(Player player)
        {
            Debug.Log($"Player {player.NickName} kill Zombie");
        }

        private void MonsterKillPlayerEvent(Player player)
        {
            Debug.Log($"Player  {player.NickName} was killed by zombie");
            if (PhotonNetwork.LocalPlayer.UserId == player.UserId)
            {
                onOwnerPlayerDead?.Invoke();
            };
        }
    }
}



