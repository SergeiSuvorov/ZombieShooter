using Model;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
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
        private EnemyManager _enemyManager;

        private ProfilePlayer _profilePlayer;
        private CharactersController _charactersController;

        public Action onOwnerPlayerDead;
        public Action<CharacterView> onOwnerPlayerRegister;

        private List<Transform> _playerSpawnPoints;
        private List<Transform> _enemySpawnPoints;
        private List<Transform> _characterTransformList=new List<Transform>();

        public PhotonMovableObjectManager(UpdateManager updateManager, InputController inputController, ProfilePlayer profilePlayer, MapSpawnPoints mapSpawnPoints)
        {
            if (Instance == null)
                _instance = this;

            _updateManager = updateManager;
            _profilePlayer = profilePlayer;
            _enemySpawnPoints = mapSpawnPoints.EnemySpawnPoints;
            _playerSpawnPoints = mapSpawnPoints.PlayerSpawnPoints;

            var characterView = LoadCharacterView();
            _charactersController = new CharactersController(updateManager, inputController, profilePlayer);
            AddController(_charactersController);

            _charactersController.onOwnerPlayerDead += OnOwnerPlayerDead;

            _characterTransformList.Add(characterView.transform);
            _enemyManager = new EnemyManager(_updateManager, _characterTransformList, mapSpawnPoints.EnemySpawnPoints);
            AddController(_enemyManager);
        }

        public CharacterView LoadCharacterView()
        {
            var spawnPointIndex = PhotonNetwork.CurrentRoom.Players.Count;
            var spawnPosition = _playerSpawnPoints[spawnPointIndex - 1].position;
            var spawnRotation = _playerSpawnPoints[spawnPointIndex - 1].rotation;
            var objectView = PhotonNetwork.Instantiate(_viewPath.PathResource, spawnPosition, spawnRotation, 0);

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

        public void RegisterPlayer(CharacterView characterView)
        {
            _charactersController.RegisterPlayer(characterView);

            if (characterView.photonView.IsMine)
            {
                onOwnerPlayerRegister?.Invoke(characterView);
            }
            else
            {
                _characterTransformList.Add(characterView.transform);
            }
        }

        public void OnOwnerPlayerDead()
        {
            onOwnerPlayerDead?.Invoke();
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            for (int i = (_characterTransformList.Count - 1); i >= 0; i--)
            {
                var characterOwner = _characterTransformList[i].GetComponent<CharacterView>().photonView.Owner;
                if (characterOwner == other)
                {
                    _characterTransformList.RemoveAt(i);
                    break;
                }
            }

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



