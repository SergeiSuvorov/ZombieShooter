namespace Model
{
    public class PlayerModel
    {
        private string _userName;
        private int _playerHealth;

        public string UserName => _userName;
        public int PlayerHealth => _playerHealth;

        public PlayerModel(string userName, int playerHealth)
        {
            _userName = userName;
            _playerHealth = playerHealth;
        }


    }
}

