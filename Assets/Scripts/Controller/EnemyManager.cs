using Photon.Pun;
using UnityEngine;


namespace Controller
{
    public class EnemyManager:BaseController
    {
        private UpdateManager _updateManager;
        private ZombieControllerBase _zombiController;
        private Transform _characterTransform;

        public EnemyManager(UpdateManager updateManager, Transform characterTransform)
        {
            _updateManager = updateManager;
            _characterTransform = characterTransform;
            if (PhotonNetwork.IsMasterClient)
            {
                _updateManager.FixUpdateList.Remove(_zombiController);
                var zombiController = new MasterClientZombiController(characterTransform);
                _zombiController = zombiController;
                AddController(_zombiController);

                _updateManager.FixUpdateList.Add(zombiController);
            }
            
        }

        public void RegisterEnemy(ZombieView view)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                _zombiController = new RemoveZombieController(view);
                AddController(_zombiController);
                _updateManager.FixUpdateList.Add(_zombiController);
            }
        }

        protected override void OnDispose()
        {
            _updateManager.FixUpdateList.Remove(_zombiController);
            base.OnDispose();
        }

        public void OnMasterClientSwitched()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var zombiController = new MasterClientZombiController(_zombiController,_characterTransform);
                RemoveController(_zombiController);
                _zombiController = zombiController;
                AddController(_zombiController);
                _updateManager.FixUpdateList.Add(zombiController);
            }
        }
    }
}



