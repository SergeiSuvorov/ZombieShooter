using Model;
using UnityEngine;


namespace Controller
{
    public class MainController : BaseController
    {


        private MainMenuController _mainMenuController;
        private EnterInGameMenuController _enterInGameMenuController;

        private BaseController _currentController;

        private readonly Transform _placeForUi;
        private readonly ProfilePlayer _profilePlayer;

        public MainController(Transform placeForUi, ProfilePlayer profilePlayer)
        {
            _profilePlayer = profilePlayer;
            _placeForUi = placeForUi;
            OnChangeGameState(_profilePlayer.CurrentState.Value);
            profilePlayer.CurrentState.SubscribeOnChange(OnChangeGameState);
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
                    Debug.Log("Start Game");
                    break;
                default:
                    //_mainMenuController?.Dispose();
                    break;
            }
        }
    }
}

