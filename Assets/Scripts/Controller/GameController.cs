using Model;
using Photon.Pun;
using System;
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
        private GameTimerController _gameTimer;
        private Transform _placeForUi;
        private SubscriptionProperty<Vector2> _inputMoveDiff = new SubscriptionProperty<Vector2>();
        private SubscriptionProperty<Vector2> _inputRotateDiff = new SubscriptionProperty<Vector2>();
        private SubscriptionProperty<bool> _inputIsFire = new SubscriptionProperty<bool>();

        private UpdateManager _updateManager;
        private ProfilePlayer _profilePlayer;

        private PhotonMovableObjectManager _photonMovableObjectManager;
        private bool _playerIsDied;

        public GameController(UpdateManager updateManager, ProfilePlayer profilePlayer, Transform placeForUi)
        {
            _placeForUi = placeForUi;
            _updateManager = updateManager;
            _profilePlayer = profilePlayer;

            _gameTimer = new GameTimerController(_updateManager, _placeForUi);
            _gameTimer.MatchEndTimerFinish += onMatchEnd;
            _gameTimer.GameEndTimerFinish += onGameEnd;
            AddController(_gameTimer);

            _eventManager = new EventManager(_placeForUi, _updateManager);
            AddController(_eventManager);
            _eventManager.onOwnerPlayerDead += OnOwnerPlayerDead;

            _mapController = new MapController();
            AddController(_mapController);

            _inputController = new InputController(_inputMoveDiff, _inputRotateDiff, _inputIsFire);
            AddController(_inputController);

            var spawnPoints = _mapController.GetSpawnPoint();
            _photonMovableObjectManager = new PhotonMovableObjectManager(_updateManager, _inputController, _profilePlayer, spawnPoints, _placeForUi);
            AddController(_photonMovableObjectManager);
            _photonMovableObjectManager.onOwnerPlayerRegister += OnOwnerCharacterRegistrator;
            _photonMovableObjectManager.onOwnerPlayerDead += OnOwnerPlayerDead;

            _updateManager.UpdateList.Add(_inputController);
            
        }

        private void onGameEnd()
        {
            PhotonNetwork.Disconnect();

            _profilePlayer.CurrentState.Value = GameState.Menu;
        }

        private void onMatchEnd()
        {
            _gameTimer.StartEndGameCountdown();
            if(!_playerIsDied)
            {
                _photonMovableObjectManager.Dispose();
                _photonMovableObjectManager.onOwnerPlayerRegister -= OnOwnerCharacterRegistrator;
                _photonMovableObjectManager.onOwnerPlayerDead -= OnOwnerPlayerDead;
            }
            _updateManager.LateUpdateList.Remove(_cameraController);
            _eventManager.EndGame();

            var gameResult = _eventManager.GetResult(PhotonNetwork.LocalPlayer);
            _profilePlayer.LastXPCount = gameResult.ScorePointResult;
        }

        private void OnOwnerCharacterRegistrator(CharacterView characterView)
        {
            _cameraController = new FollowCameraController(characterView.transform);
            AddController(_cameraController);
            _updateManager.LateUpdateList.Add(_cameraController);
        }

        private void OnOwnerPlayerDead()
        {
            _playerIsDied = true;
            onMatchEnd();
        }

        protected override void OnDispose()
        {
            if (_photonMovableObjectManager != null)
            {
                _photonMovableObjectManager.onOwnerPlayerRegister -= OnOwnerCharacterRegistrator;
                _photonMovableObjectManager.onOwnerPlayerDead -= OnOwnerPlayerDead;
            }
            _updateManager.UpdateList.Remove(_inputController);
            _updateManager.LateUpdateList.Remove(_cameraController);
            _eventManager.onOwnerPlayerDead -= OnOwnerPlayerDead;
            _gameTimer.MatchEndTimerFinish -= onMatchEnd;
            _gameTimer.GameEndTimerFinish -= onGameEnd;

            base.OnDispose();
        }
    }
}



