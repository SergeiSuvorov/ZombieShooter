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
        private List<ZombieControllerBase> _zombiControllerList = new List<ZombieControllerBase>();
        private List<ZombieControllerBase> _waitingToResurrectZombiesList = new List<ZombieControllerBase>();
        private List<Transform> _characterTransformList;
        private List<Transform> _enemySpawnPoints;

        private float _curentResurrectTime;
        private const float _resurrectTime =3;
        public bool IsActive => throw new System.NotImplementedException();

        public EnemyManager(UpdateManager updateManager, List<Transform> characterTransformList, List<Transform> enemySpawnPoints)
        {
            _updateManager = updateManager;
            _characterTransformList = characterTransformList;
            _enemySpawnPoints = enemySpawnPoints;
            _updateManager.UpdateList.Add(this);
            CreateZombie();
        }

        private void CreateZombie()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var randomTarget = GetRandomTarget();
                var randomSpawnIndex = Random.Range(0, _enemySpawnPoints.Count);

                _zombieController = new MasterClientZombiController(randomTarget, _enemySpawnPoints[randomSpawnIndex], GetNewTargetForZombie);
                AddController(_zombieController);

                _zombieController.onZombieDie += OnZombieDie;
                _updateManager.FixUpdateList.Add(_zombieController);
                _zombiControllerList.Add(_zombieController);
            }
        }

        private Transform GetRandomTarget()
        {
            var randomTargetIndex = Random.Range(0, _characterTransformList.Count);
            while (_characterTransformList[randomTargetIndex] == null)
            {
                _characterTransformList.RemoveAt(randomTargetIndex);
                randomTargetIndex = Random.Range(0, _characterTransformList.Count);
            }

            return _characterTransformList[randomTargetIndex];
        }

        private void OnZombieDie(ZombieControllerBase zombieController)
        {
            _waitingToResurrectZombiesList.Add(zombieController);
            if(_curentResurrectTime<=0)
            _curentResurrectTime = _resurrectTime;
        }

        public void RegisterEnemy(ZombieView view)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                _zombieController = new RemoteZombieController(view);
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
                var randomTarget = GetRandomTarget();
                _updateManager.FixUpdateList.Remove(_zombieController);
                var randomSpawnIndex = Random.Range(0, _enemySpawnPoints.Count);
                var zombieController = new MasterClientZombiController(_zombieController, randomTarget, _enemySpawnPoints[randomSpawnIndex], GetNewTargetForZombie);
                RemoveController(_zombieController);
                _zombieController.onZombieDie -= OnZombieDie;

                if (_waitingToResurrectZombiesList.Contains(_zombieController))
                {
                    _waitingToResurrectZombiesList.Remove(_zombieController);
                    _waitingToResurrectZombiesList.Add(zombieController);
                }
                _zombieController.Dispose();

                _zombieController = zombieController;
                AddController(_zombieController);
                _updateManager.FixUpdateList.Add(_zombieController);
                _zombieController.onZombieDie += OnZombieDie;
            }
        }
        public void GetNewTargetForZombie(ZombieControllerBase zombieControllerBase)
        {
            var randomTarget = GetRandomTarget();
            var controller = zombieControllerBase as MasterClientZombiController;
            if(controller!=null)
                controller.SetNewTarget(randomTarget);
        }
        public void UpdateExecute()
        {
            if (_waitingToResurrectZombiesList.Count <1)
                return;
            _curentResurrectTime -= Time.deltaTime;
            if (_curentResurrectTime <= 0)
            {
                var zombieController = _waitingToResurrectZombiesList[0];
                var randomTarget = GetRandomTarget();
                zombieController.ResurrectZombies(randomTarget);
                _waitingToResurrectZombiesList.Remove(zombieController);

                if (_waitingToResurrectZombiesList.Count > 0)
                    _curentResurrectTime = _resurrectTime;
            }                
        }

    }
}



