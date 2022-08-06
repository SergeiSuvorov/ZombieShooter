using PlayFab;
using PlayFab.ClientModels;
using System;


namespace Controller
{
    public class PlayFabAccountContoller : BaseController
    {
        protected string _username;
        protected string _password;
        protected string _userId;
        protected string _email;

        public Action onPlayFabStartConnection;
        public Action onPlayFabConnect;

        public Action<PlayFabError> onPlayFabConnectError;
        public Action<UserAccountInfo> onPlayFabGetAccountSuccess;


        public void CreateAccount()
        {
            OnStartConnection();

            PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
            {
                Username = _username,
                Email = _email,
                Password = _password,
                RequireBothUsernameAndEmail = true
            }, result =>
            {
                OnConnect();
            }, error =>
            {
                OnError(error);
            });
        }

        public void SignIn()
        {
            OnStartConnection();

            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
            {
                Username = _username,
                Password = _password
            }, result =>
            {
                OnConnect();
            }, error =>
            {
                OnError(error);
            });
        }

        public void UpdateEmail(string email)
        {
            _email = email;
        }
        public void UpdatePassword(string password)
        {
            _password = password;
        }
        public void UpdateUsername(string username)
        {
            _username = username;
        }

        private void GetUserData()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountSuccess, OnError);
        }

        private void OnError(PlayFabError error)
        {
            onPlayFabConnectError?.Invoke(error);
        }

        private void OnStartConnection()
        {
            onPlayFabStartConnection?.Invoke();
        }

        private void OnConnect()
        {
            onPlayFabConnect?.Invoke();
            GetUserData();
        }
        private void OnGetAccountSuccess(GetAccountInfoResult result)
        {
            var accountInfo = result.AccountInfo;
            onPlayFabGetAccountSuccess?.Invoke(accountInfo);
        }

    }
}


