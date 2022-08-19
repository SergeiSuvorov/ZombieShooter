using Model;
using Photon.Pun;
using Photon.Realtime;
using System;
using Tools;
using UnityEngine;


namespace Controller
{
    public class PhotonMovableObjectManager : PunCallbacksBaseController
    {
        private static PhotonMovableObjectManager _instance;
        public static PhotonMovableObjectManager Instance => _instance;

        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = ViewPathLists.CharacterView };

        private UpdateManager _updateManager;
        private Transform _characterTransform;
        private EnemyManager _enemyManager;

        private ProfilePlayer _profilePlayer;
        private CharactersController _charactersController;

        public Action onOwnerPlayerDead;
        public Action<CharacterView> onOwnerPlayerRegister;

        public PhotonMovableObjectManager(UpdateManager updateManager, InputController inputController, ProfilePlayer profilePlayer)
        {
            if (Instance == null)
                _instance = this;

            _updateManager = updateManager;
            _profilePlayer = profilePlayer;

            var characterView = LoadCharacterView();
            _charactersController = new CharactersController(updateManager, inputController, profilePlayer);
            AddController(_charactersController);

            _charactersController.onOwnerPlayerDead += OnOwnerPlayerDead;

            _enemyManager = new EnemyManager(_updateManager, characterView.transform);
            AddController(_enemyManager);
        }

        public CharacterView LoadCharacterView()
        {
            var objectView = PhotonNetwork.Instantiate(_viewPath.PathResource, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);

            return objectView.GetComponent<CharacterView>();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            _enemyManager.OnMasterClientSwitched();
        }

        public void RegisterEnemy(ZombieView view)
        {
            _enemyManager.RegisterEnemy(view);
        }

        public void RegisterPlayer(CharacterView view)
        {
            _charactersController.RegisterPlayer(view);
            if (view.photonView.IsMine)
            {
                onOwnerPlayerRegister?.Invoke(view);
            }
        }

        public void OnOwnerPlayerDead()
        {
            onOwnerPlayerDead?.Invoke();
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            _charactersController.OnPlayerLeftRoom(other);
        }

        protected override void OnDispose()
        {
            _instance = null;
            _charactersController.onOwnerPlayerDead -= OnOwnerPlayerDead;
            base.OnDispose();
        }
    }
}



