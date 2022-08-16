using Model;
using UnityEngine;


namespace Controller
{
    public class MainController : BaseController
    {
        private UpdateManager _updateManager;

        private MainMenuController _mainMenuController;
        private EnterInGameMenuController _enterInGameMenuController;
        private GameController _gameController;

        private BaseController _currentController;

        private readonly Transform _placeForUi;
        private readonly ProfilePlayer _profilePlayer;

        public MainController(Transform placeForUi, ProfilePlayer profilePlayer, UpdateManager updateManager)
        {
            _profilePlayer = profilePlayer;
            _placeForUi = placeForUi;
            OnChangeGameState(_profilePlayer.CurrentState.Value);
            profilePlayer.CurrentState.SubscribeOnChange(OnChangeGameState);
            _updateManager = updateManager;
        }

        protected override void OnDispose()
        {
            _currentController?.Dispose();
            _mainMenuController?.Dispose();
            _enterInGameMenuController.Dispose();
            _profilePlayer.CurrentState.UnSubscriptionOnChange(OnChangeGameState);


            base.OnDispose();
        }

        private void OnChangeGameState(GameState state)
        {
            switch (state)
            {
                case GameState.Start:
                    _currentController?.Dispose();
                    _enterInGameMenuController = new EnterInGameMenuController(_placeForUi, _profilePlayer);
                    _currentController = _enterInGameMenuController;
                    break;
                case GameState.Menu:
                    _currentController?.Dispose();
                    _mainMenuController = new MainMenuController(_placeForUi, _profilePlayer);
                    _currentController = _mainMenuController;
                    break;
                case GameState.Game:
                    _currentController?.Dispose();
                    _gameController = new GameController(_updateManager,_profilePlayer);
                    _currentController = _gameController;
                    Debug.Log("Start Game");
                    break;
                default:
                    //_mainMenuController?.Dispose();
                    break;
            }
        }
    }
}

