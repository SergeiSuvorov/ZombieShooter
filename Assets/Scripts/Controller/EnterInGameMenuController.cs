using Model;
using PlayFab;
using PlayFab.ClientModels;
using Tools;
using UnityEngine;


namespace Controller
{
    public class EnterInGameMenuController : BaseController
    {
        private readonly ResourcePath _viewPath = new ResourcePath { PathResource =  PathLists.AccountEnterMenuView };
        private readonly ProfilePlayer _profilePlayer;

        private EnterMenuView _view;
        private EnterInGameFormView _enterInGameFormView;
        private SignInWindowView _signInWindow;
        private CreateAccountWindowView _createAccountWindow;
        private EnterInAccauntBaseView _currentForm;

        private PlayFabAccountContoller _playFabAccountContoller;

        private FeedbackText _feedbackLog;

        public EnterInGameMenuController(Transform placeForUi, ProfilePlayer profilePlayer)
        {
            _profilePlayer = profilePlayer;

            _view = LoadView(placeForUi);

            _signInWindow = _view.SignInWindow;
            _createAccountWindow = _view.CreateAccountWindow;
            _enterInGameFormView = _view.EnterInGameView;
            _feedbackLog = _view.FeedbackLog;
            _playFabAccountContoller = new PlayFabAccountContoller();
            AddController(_playFabAccountContoller);
            SubscriptionsOnCallBack();
        }

        private void SubscriptionsOnCallBack()
        {
            _enterInGameFormView.onSignInButtonClick += OpenSignInWindow;
            _enterInGameFormView.onCreateAccountButtonClick += OpenCreateAccauntWindow;

            _signInWindow.onBackButtonClick += OpenEnterMenuForm;
            _signInWindow.onSignInButtonClick += _playFabAccountContoller.SignIn;
            _signInWindow.onUserNameInputFieldUpdate += _playFabAccountContoller.UpdateUsername;
            _signInWindow.onPasswordInputFieldUpdate += _playFabAccountContoller.UpdatePassword;

            _createAccountWindow.onCreateAccountButtonClick += _playFabAccountContoller.CreateAccount;
            _createAccountWindow.onBackButtonClick += OpenEnterMenuForm;
            _createAccountWindow.onUserNameInputFieldUpdate += _playFabAccountContoller.UpdateUsername;
            _createAccountWindow.onPasswordInputFieldUpdate += _playFabAccountContoller.UpdatePassword;
            _createAccountWindow.onEmailInputFieldUpdate += _playFabAccountContoller.UpdateEmail;

            _playFabAccountContoller.onPlayFabConnect += OnConnect;
            _playFabAccountContoller.onPlayFabConnectError += OnError;
            _playFabAccountContoller.onPlayFabStartConnection += OnStartConnection;
            _playFabAccountContoller.onPlayFabGetAccountSuccess += OnGetAccountInfo;

        }

        private void OpenEnterMenuForm()
        {
            _currentForm.SetActive(false);
            _enterInGameFormView.SetActive(true);
        }

        private void OpenCreateAccauntWindow()
        {
            _enterInGameFormView.SetActive(false);
            _currentForm = _createAccountWindow;
            _currentForm.SetActive(true);
        }

        private void OpenSignInWindow()
        {
            _enterInGameFormView.SetActive(false);
            _currentForm = _signInWindow;
            _currentForm.SetActive(true);
        }

        private void OnStartConnection()
        {
            _feedbackLog.LogFeedback($"Connection to game server");
            _currentForm.SetActive(false);
            _enterInGameFormView.SetActive(false);
        }

        private void OnConnect()
        {
            _feedbackLog.LogFeedback($"Getting Account Info");
        }

        private void OnError(PlayFabError error)
        {
            _feedbackLog.LogFeedback($"Something went wrong: {error.GenerateErrorReport()}");
            _enterInGameFormView.SetActive(true);
        }


        private void OnGetAccountInfo(UserAccountInfo userInfo)
        {
            _profilePlayer.UpdateUserAccountInfo(userInfo);
            _profilePlayer.CurrentState.Value = GameState.Menu;

            _feedbackLog.LogFeedback($" Welcome {_profilePlayer.UserName}");
        }

        private EnterMenuView LoadView(Transform placeForUi)
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath), placeForUi, false);
            AddGameObjects(objectView);

            return objectView.GetComponent<EnterMenuView>();
        }
        protected override void OnDispose()
        {
            UnsubscriptionsFromCallBack();
            base.OnDispose();
        }
        private void UnsubscriptionsFromCallBack()
        {
            _enterInGameFormView.onSignInButtonClick -= OpenSignInWindow;
            _enterInGameFormView.onCreateAccountButtonClick -= OpenCreateAccauntWindow;

            _signInWindow.onBackButtonClick -= OpenEnterMenuForm;
            _signInWindow.onSignInButtonClick -= _playFabAccountContoller.SignIn;
            _signInWindow.onUserNameInputFieldUpdate -= _playFabAccountContoller.UpdateUsername;
            _signInWindow.onPasswordInputFieldUpdate -= _playFabAccountContoller.UpdatePassword;

            _createAccountWindow.onCreateAccountButtonClick -= _playFabAccountContoller.CreateAccount;
            _createAccountWindow.onBackButtonClick -= OpenEnterMenuForm;
            _createAccountWindow.onUserNameInputFieldUpdate -= _playFabAccountContoller.UpdateUsername;
            _createAccountWindow.onPasswordInputFieldUpdate -= _playFabAccountContoller.UpdatePassword;
            _createAccountWindow.onEmailInputFieldUpdate -= _playFabAccountContoller.UpdateEmail;

            _playFabAccountContoller.onPlayFabConnect -= OnConnect;
            _playFabAccountContoller.onPlayFabConnectError -= OnError;
            _playFabAccountContoller.onPlayFabStartConnection -= OnStartConnection;
            _playFabAccountContoller.onPlayFabGetAccountSuccess -= OnGetAccountInfo;
        }
    }
}


