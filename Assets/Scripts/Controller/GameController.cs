using Model;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Tools;
using UnityEngine;


namespace Controller
{
    public class GameController : BaseController
    {
        private static GameController instance;
        public static GameController Instance=>instance;

        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = ViewPathLists.CharacterView };

        private MapController _mapController;
        private InputController _inputController;
        private OwnerPlayerCharacterController _ownerCharacterController;
        private PhotonGameController _photonGameController;
        private FollowCameraController _cameraController;
        private Dictionary<Player, BasePlayerCharacterController> _playerControllerDictionary = new Dictionary<Player, BasePlayerCharacterController>();

        private SubscriptionProperty<Vector2> _inputMoveDiff = new SubscriptionProperty<Vector2>();
        private SubscriptionProperty<Vector2> _inputRotateDiff = new SubscriptionProperty<Vector2>();
        private SubscriptionProperty<bool> _inputIsFire = new SubscriptionProperty<bool>();

        private UpdateManager _updateManager;
        private ProfilePlayer _profilePlayer;

        private EnemyManager _enemyManager;
        public GameController(UpdateManager updateManager, ProfilePlayer profilePlayer)
        {
            if (Instance == null)
                instance = this;


            _updateManager = updateManager;
            _profilePlayer = profilePlayer;

            _mapController = new MapController();
            AddController(_mapController);

            _inputController = new InputController(_inputMoveDiff, _inputRotateDiff, _inputIsFire);
            AddController(_inputController);

            _photonGameController = new PhotonGameController();
            AddController(_photonGameController);

            var characterView = LoadCharacterView();

            _cameraController = new FollowCameraController(characterView.transform);
            AddController(_cameraController);

            _enemyManager = new EnemyManager(_updateManager, characterView.transform);
            _updateManager.UpdateList.Add(_inputController);
            _updateManager.LateUpdateList.Add(_cameraController);

            _photonGameController.onPlayerLeftRoom += OnPlayerLeftRoom;
            _photonGameController.onMasterClientSwitched += OnMasterClientSwitched;
        }

        private void OnMasterClientSwitched(Player newMasterClient)
        {
            _enemyManager.OnMasterClientSwitched();
        }

        public CharacterView LoadCharacterView()
        {
            var objectView = PhotonNetwork.Instantiate(_viewPath.PathResource, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
 
            return objectView.GetComponent<CharacterView>();
        }

        private void OnPlayerLeftRoom(Player other)
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
        public void RegisterEnemy(ZombieView view)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                _enemyManager.RegisterEnemy(view);
            }

        }
        public void RegisterPlayer(CharacterView view)
        {
            if (view.photonView.IsMine)
            {
                var debugPlayerModel = new PlayerModel(_profilePlayer.UserName, 100);
                _ownerCharacterController = new OwnerPlayerCharacterController(_inputMoveDiff, _inputRotateDiff, _inputIsFire, view, debugPlayerModel);
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

        protected override void OnDispose()
        {
            var tKey = _playerControllerDictionary.Keys;

            foreach(var key in tKey)
            {
                if(_playerControllerDictionary.ContainsKey(key))
                {
                    var characterController = _playerControllerDictionary[key];

                    if (characterController != null)
                    {
                        _updateManager.UpdateList.Remove(characterController);
                        _updateManager.FixUpdateList.Remove(characterController);
                    }
                }
            }

            _updateManager.UpdateList.Remove(_inputController);
            _updateManager.LateUpdateList.Remove(_cameraController);
            _photonGameController.onPlayerLeftRoom -= OnPlayerLeftRoom;

            base.OnDispose();
        }
    }
}



