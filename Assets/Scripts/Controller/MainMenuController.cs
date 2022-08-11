using Model;
using Tools;
using UnityEngine;

namespace Controller
{
    public class MainMenuController : BaseController
    {
        private MainMenuView  _view;
        private PhotonGameConnectionController _photonGameConnectionController;

        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = ViewPathLists.MainMenuView };
        private readonly ProfilePlayer _profilePlayer;

        public MainMenuController (Transform placeForUi, ProfilePlayer profilePlayer)
        {
            _profilePlayer = profilePlayer;
            _view = LoadView(placeForUi);
            _photonGameConnectionController = new PhotonGameConnectionController(4);

            SubscriptionsOnCallBack();
        }

        private void OnStartButtonClick()
        {
            LogFeedback(" Start Connection");
            var nickName = _profilePlayer.UserName;
            _photonGameConnectionController.SetNickName(nickName);
            _photonGameConnectionController.Connect();
        }

        private void SubscriptionsOnCallBack()
        {
            _view.onStartButtonClick +=OnStartButtonClick;
            _photonGameConnectionController.onPhotonConnect+= LogFeedback;
            _photonGameConnectionController.onPhotonRandomRoomJoinFailed+= LogFeedback;
            _photonGameConnectionController.onPhotonRoomJoin += EnterInRoom;
        }

        private void LogFeedback(string message)
        {
            _view.LogFeedback(message);
        }

        private void EnterInRoom()
        {
            _profilePlayer.CurrentState.Value = GameState.Game;
        }

        private void UnsubscriptionsFromCallBack()
        {
            _view.onStartButtonClick -= OnStartButtonClick;
            _photonGameConnectionController.onPhotonConnect -= LogFeedback;
            _photonGameConnectionController.onPhotonRandomRoomJoinFailed -= LogFeedback;
            _photonGameConnectionController.onPhotonRoomJoin -= EnterInRoom;
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


