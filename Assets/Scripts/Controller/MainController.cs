using Model;
using UnityEngine;


namespace Controller
{
    public class MainController : BaseController
    {
        public MainController(Transform placeForUi, ProfilePlayer profilePlayer)
        {
            _profilePlayer = profilePlayer;
            _placeForUi = placeForUi;
            OnChangeGameState(_profilePlayer.CurrentState.Value);
            profilePlayer.CurrentState.SubscribeOnChange(OnChangeGameState);
        }

        //private MainMenuController _mainMenuController;
        private EnterInGameMenuController _enterInGameMenuController;

        private readonly Transform _placeForUi;
        private readonly ProfilePlayer _profilePlayer;

        protected override void OnDispose()
        {
            //_mainMenuController?.Dispose();
            _profilePlayer.CurrentState.UnSubscriptionOnChange(OnChangeGameState);

            _enterInGameMenuController.Dispose();
            base.OnDispose();
        }

        private void OnChangeGameState(GameState state)
        {
            switch (state)
            {
                case GameState.Start:
                    _enterInGameMenuController = new EnterInGameMenuController(_placeForUi, _profilePlayer);
                    //_mainMenuController = new MainMenuController(_placeForUi, _profilePlayer);
                    break;
                case GameState.Menu:
                    Debug.Log("Menu");
                    break;
                case GameState.Game:
                    //_mainMenuController?.Dispose();
                    break;
                default:
                    //_mainMenuController?.Dispose();
                    break;
            }
        }
    }
}

