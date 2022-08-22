using PlayFab.ClientModels;
using Tools;


namespace Model
{
    public class ProfilePlayer
    {
        public SubscriptionProperty<GameState> CurrentState { get; }

        private  UserAccountInfo _userAccountInfo;

        public string UserID => _userAccountInfo.PlayFabId;
        public string UserName => _userAccountInfo.Username;

        public int LastXPCount;
        public ProfilePlayer()
        {
            CurrentState = new SubscriptionProperty<GameState>();
        }

        public void UpdateUserAccountInfo(UserAccountInfo userAccountInfo)
        {
            _userAccountInfo=userAccountInfo;
        }
    }
}

