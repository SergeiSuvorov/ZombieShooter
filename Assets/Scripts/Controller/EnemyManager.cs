using Photon.Pun;
using System.Collections.Generic;
using Tools;
using UnityEngine;


namespace Controller
{
    public class EnemyManager:BaseController, IUpdateable
    {
        private UpdateManager _updateManager;
        private ZombieControllerBase _zombieController;
        private Transform _characterTransform;

        private List<ZombieControllerBase> _zombiControllerList = new List<ZombieControllerBase>();
        private List<ZombieControllerBase> _waitingToResurrectZombiesList = new List<ZombieControllerBase>();
        private float _curentResurrectTime;
        private const float _resurrectTime =3;
        public bool IsActive => throw new System.NotImplementedException();

        public EnemyManager(UpdateManager updateManager, Transform characterTransform)
        {
            _updateManager = updateManager;
            _characterTransform = characterTransform;
            _updateManager.UpdateList.Add(this);
            if (PhotonNetwork.IsMasterClient)
            {
                _updateManager.FixUpdateList.Remove(_zombieController);
                _zombieController = new MasterClientZombiController(characterTransform);
                AddController(_zombieController);

                _zombieController.onZombieDie += OnZombieDie ;
                _updateManager.FixUpdateList.Add(_zombieController);
                _zombiControllerList.Add(_zombieController);
            }
            
        }

        private void OnZombieDie(ZombieControllerBase zombieController)
        {
            Debug.Log("Die Zombie Die");
            _waitingToResurrectZombiesList.Add(zombieController);
            if(_curentResurrectTime<=0)
            _curentResurrectTime = _resurrectTime;
        }

        public void RegisterEnemy(ZombieView view)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                _zombieController = new RemoveZombieController(view);
                AddController(_zombieController);
                _zombieController.onZombieDie += OnZombieDie;
                _updateManager.FixUpdateList.Add(_zombieController);
            }
        }

        protected override void OnDispose()
        {
            _updateManager.UpdateList.Remove(this);
            _updateManager.FixUpdateList.Remove(_zombieController);
            _zombieController.onZombieDie -= OnZombieDie;
            base.OnDispose();
        }

        public void OnMasterClientSwitched()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var zombiController = new MasterClientZombiController(_zombieController,_characterTransform);
                RemoveController(_zombieController);
                _zombieController = zombiController;
                AddController(_zombieController);
                _updateManager.FixUpdateList.Add(zombiController);
            }
        }

        public void UpdateExecute()
        {
            if (_waitingToResurrectZombiesList.Count <1)
                return;
            _curentResurrectTime -= Time.deltaTime;
            if (_curentResurrectTime <= 0)
            {
                var zombieController = _waitingToResurrectZombiesList[0];
                int debugValue = Random.Range(2, 7);
                zombieController.ResurrectZombies(new Vector3(debugValue, 1f, debugValue));
                _waitingToResurrectZombiesList.Remove(zombieController);

                if (_waitingToResurrectZombiesList.Count > 0)
                    _curentResurrectTime = _resurrectTime;
                Debug.Log("ResurrectTime");
            }                
        }
    }
}



