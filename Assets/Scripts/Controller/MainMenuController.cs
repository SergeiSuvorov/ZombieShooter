using Model;
using Tools;
using UnityEngine;

namespace Controller
{
    public class MainMenuController : BaseController
    {
        private MainMenuView  _view;
        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = ViewPathLists.MainMenuView };
        private readonly ProfilePlayer _profilePlayer;


        public MainMenuController (Transform placeForUi, ProfilePlayer profilePlayer)
        {
            _profilePlayer = profilePlayer;

            _view = LoadView(placeForUi);

            SubscriptionsOnCallBack();
        }

        private void OnStartButtonClick()
        {
            _profilePlayer.CurrentState.Value = GameState.Game;
        }

        private void SubscriptionsOnCallBack()
        {
            _view.onStartButtonClick +=OnStartButtonClick;
        }

        private void UnsubscriptionsFromCallBack()
        {
            _view.onStartButtonClick -= OnStartButtonClick;
        }

        protected override void OnDispose()
        {
            UnsubscriptionsFromCallBack();
            base.OnDispose();
        }

        private MainMenuView LoadView(Transform placeForUi)
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath), placeForUi, false);
            AddGameObjects(objectView);

            return objectView.GetComponent<MainMenuView>();
        }
    }
}


