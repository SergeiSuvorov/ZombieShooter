using Model;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;


namespace Controller
{
    public class CharactersController:BaseController, IOnEventCallback
    {
        private OwnerPlayerCharacterController _ownerCharacterController;
        private Dictionary<Player, BasePlayerCharacterController> _playerControllerDictionary = new Dictionary<Player, BasePlayerCharacterController>();
        private SubscriptionProperty<Vector2> _moveDiff = new SubscriptionProperty<Vector2>();
        private SubscriptionProperty<Vector2> _rotateDiff = new SubscriptionProperty<Vector2>();
        private SubscriptionProperty<bool> _isFire = new SubscriptionProperty<bool>();
        private ProfilePlayer _profilePlayer;
        private UpdateManager _updateManager;
        private Transform _placeForUi;

        public Action onOwnerPlayerDead;
        public Action<CharacterView> onOwnerPlayerRegister;

        public CharactersController( UpdateManager updateManager, InputController inputController, ProfilePlayer profilePlayer, Transform placeForUi)
        {
            PhotonNetwork.AddCallbackTarget(this);

            _placeForUi = placeForUi;
            _moveDiff = inputController.MoveDiff;
            _rotateDiff = inputController.RotateDiff;
            _isFire = inputController.IsFire;
            _updateManager = updateManager;
            _profilePlayer= profilePlayer;
        }

        public void RegisterPlayer(CharacterView view)
        {
            if (view.photonView.IsMine)
            {
                var debugPlayerModel = new PlayerModel(_profilePlayer.UserName, 100);
                _ownerCharacterController = new OwnerPlayerCharacterController(_moveDiff, _rotateDiff, _isFire, view, debugPlayerModel);
                _ownerCharacterController.CreatePlayerUI(_placeForUi);
                _ownerCharacterController.onPlayerDied += OnOwnerPlayerDead;
                AddController(_ownerCharacterController);

                _updateManager.UpdateList.Add(_ownerCharacterController);
                _updateManager.FixUpdateList.Add(_ownerCharacterController);

                _playerControllerDictionary.Add(view.photonView.Owner, _ownerCharacterController);
            }
            else
            {
                var characterController = new RemotePlayerCharacterController(view);
                AddController(characterController);

                _updateManager.UpdateList.Add(characterController);
                _updateManager.FixUpdateList.Add(characterController);

                _playerControllerDictionary.Add(view.photonView.Owner, characterController);
            }
        }

        public void OnOwnerPlayerDead()
        {
            onOwnerPlayerDead?.Invoke();
        }

        public void OnPlayerLeftRoom(Player other)
        {
            Debug.Log(other.NickName);
            if (_playerControllerDictionary.TryGetValue(other, out var characterController))
            {
                DestroyCharacterController(characterController);
                _playerControllerDictionary.Remove(other);
            }
        }

        private void DestroyCharacterController(BasePlayerCharacterController characterController)
        {
            _updateManager.UpdateList.Remove(characterController);
            _updateManager.FixUpdateList.Remove(characterController);
            RemoveController(characterController);
            characterController.Dispose();
        }
        protected override void OnDispose()
        {
            PhotonNetwork.RemoveCallbackTarget(this);

            var tKey = _playerControllerDictionary.Keys;

            foreach (var key in tKey)
            {
                if (_playerControllerDictionary.TryGetValue(key, out var characterController) )
                {
                    _updateManager.UpdateList.Remove(characterController);
                    _updateManager.FixUpdateList.Remove(characterController);
                }
            }

            _playerControllerDictionary.Clear();
            base.OnDispose();
        }

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;

            if (eventCode == PhotonEvenCodeList.PlayerKillsPlayerCode)
            {
                object[] data = (object[])photonEvent.CustomData;
                Player player = (Player)data[1];
                onPlayerDie(player);
            }
            if (eventCode == PhotonEvenCodeList.MonsterKillsPlayerCode)
            {
                object[] data = (object[])photonEvent.CustomData;
                Player player = (Player)data[0];
                onPlayerDie(player);
            }
        }

        private void onPlayerDie(Player player)
        {
            if (player == PhotonNetwork.LocalPlayer)
            {
                DestroyCharacterController(_ownerCharacterController);
            }
            else
                OnPlayerLeftRoom(player);
        }
    }
}



