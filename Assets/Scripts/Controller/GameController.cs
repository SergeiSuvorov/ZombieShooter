using Model;
using Photon.Pun;
using Tools;
using UnityEngine;


namespace Controller
{
    public class GameController : BaseController
    {
        private MapController _mapController;
        private InputController _inputController;
        private FollowCameraController _cameraController;
        private EventManager _eventManager;

        private SubscriptionProperty<Vector2> _inputMoveDiff = new SubscriptionProperty<Vector2>();
        private SubscriptionProperty<Vector2> _inputRotateDiff = new SubscriptionProperty<Vector2>();
        private SubscriptionProperty<bool> _inputIsFire = new SubscriptionProperty<bool>();

        private UpdateManager _updateManager;
        private ProfilePlayer _profilePlayer;

        private PhotonMovableObjectManager _photonMovableObjectManager;

        public GameController(UpdateManager updateManager, ProfilePlayer profilePlayer)
        {
            _updateManager = updateManager;
            _profilePlayer = profilePlayer;

            _eventManager = new EventManager();
            AddController(_eventManager);
            _eventManager.onOwnerPlayerDead += OnOwnerPlayerDead;

            _mapController = new MapController();
            AddController(_mapController);

            _inputController = new InputController(_inputMoveDiff, _inputRotateDiff, _inputIsFire);
            AddController(_inputController);

            var spawnPoints = _mapController.GetSpawnPoint();
            _photonMovableObjectManager = new PhotonMovableObjectManager(_updateManager, _inputController, _profilePlayer, spawnPoints);
            AddController(_photonMovableObjectManager);
            _photonMovableObjectManager.onOwnerPlayerRegister += OnOwnerCharacterRegistrator;
            _photonMovableObjectManager.onOwnerPlayerDead += OnOwnerPlayerDead;

            _updateManager.UpdateList.Add(_inputController);
        }

        private void OnOwnerCharacterRegistrator(CharacterView characterView)
        {
            _cameraController = new FollowCameraController(characterView.transform);
            AddController(_cameraController);
            _updateManager.LateUpdateList.Add(_cameraController);
        }

        private void OnOwnerPlayerDead()
        {
            Debug.Log("Player is deads");
            _profilePlayer.LastPhotonRoom = PhotonNetwork.CurrentRoom.Name;
            PhotonNetwork.Disconnect();

            _profilePlayer.CurrentState.Value = GameState.Menu;
        }

        protected override void OnDispose()
        {
            _photonMovableObjectManager.onOwnerPlayerRegister -= OnOwnerCharacterRegistrator;
            _photonMovableObjectManager.onOwnerPlayerDead -= OnOwnerPlayerDead;
            _updateManager.UpdateList.Remove(_inputController);
            _updateManager.LateUpdateList.Remove(_cameraController);
            _eventManager.onOwnerPlayerDead -= OnOwnerPlayerDead;
           
            base.OnDispose();
        }
    }
}



