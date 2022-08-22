using Model;
using PlayFab;
using PlayFab.ClientModels;
using Tools;
using UnityEngine;

namespace Controller
{
    public class MainMenuController : BaseController
    {
        private MainMenuView  _view;
        private PhotonGameConnectionController _photonGameConnectionController;
        private PlayFabStatisticController _playFabStatisticController;

        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = ViewPathLists.MainMenuView };
        private readonly ProfilePlayer _profilePlayer;

        public MainMenuController (Transform placeForUi, ProfilePlayer profilePlayer)
        {
            _profilePlayer = profilePlayer;
            _view = LoadView(placeForUi);
            _photonGameConnectionController = new PhotonGameConnectionController(4);
            AddController(_photonGameConnectionController);

            _playFabStatisticController = new PlayFabStatisticController();
            AddController(_playFabStatisticController);

            SubscriptionsOnCallBack();
            _playFabStatisticController.GetStatistics();
        }

        private void onPlayFabError(PlayFabError error)
        {
            LogFeedback(error.ErrorMessage);
        }

        private void OnGetStatistic(StatisticValue result)
        {
            if(result!=null && result.StatisticName==StatisticParametrNamePlayerLists.ExperiencePoints)
            {
                if(_profilePlayer.LastXPCount!=0)
                {
                    var nextValue = result.Value + _profilePlayer.LastXPCount;
                    _view.ShowXPInfo(nextValue, _profilePlayer.LastXPCount);
                    _profilePlayer.LastXPCount = 0;
                    _playFabStatisticController.UpdateStatistics(StatisticParametrNamePlayerLists.ExperiencePoints, nextValue);
                }
                else
                    _view.ShowXPInfo(result.Value, _profilePlayer.LastXPCount);
            }
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
            _playFabStatisticController.onGetStatistic += OnGetStatistic;
            _playFabStatisticController.onPlayFabError += onPlayFabError;
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
            _playFabStatisticController.onGetStatistic -= OnGetStatistic;
            _playFabStatisticController.onPlayFabError -= onPlayFabError;
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


