using Model;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;


namespace Controller
{
    public class CharactersController:BaseController
    {
        private OwnerPlayerCharacterController _ownerCharacterController;
        private Dictionary<Player, BasePlayerCharacterController> _playerControllerDictionary = new Dictionary<Player, BasePlayerCharacterController>();
        private SubscriptionProperty<Vector2> _moveDiff = new SubscriptionProperty<Vector2>();
        private SubscriptionProperty<Vector2> _rotateDiff = new SubscriptionProperty<Vector2>();
        private SubscriptionProperty<bool> _isFire = new SubscriptionProperty<bool>();
        private ProfilePlayer _profilePlayer;
        private UpdateManager _updateManager;

        public Action onOwnerPlayerDead;
        public Action<CharacterView> onOwnerPlayerRegister;

        public CharactersController( UpdateManager updateManager, InputController inputController, ProfilePlayer profilePlayer)
        {
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
            if (_playerControllerDictionary.ContainsKey(other))
            {
                var characterController = _playerControllerDictionary[other];
                _updateManager.UpdateList.Remove(characterController);
                _updateManager.FixUpdateList.Remove(characterController);
                _playerControllerDictionary.Remove(other);
                characterController.Dispose();
            }
        }

        protected override void OnDispose()
        {
            var tKey = _playerControllerDictionary.Keys;

            foreach (var key in tKey)
            {
                if (_playerControllerDictionary.ContainsKey(key))
                {
                    var characterController = _playerControllerDictionary[key];

                    if (characterController != null)
                    {
                        _updateManager.UpdateList.Remove(characterController);
                        _updateManager.FixUpdateList.Remove(characterController);
                    }
                }
            }

            _playerControllerDictionary.Clear();
            base.OnDispose();
        }
    }
}



