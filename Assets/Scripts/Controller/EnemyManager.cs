using Photon.Pun;
using System.Collections.Generic;
using Tools;
using UnityEngine;


namespace Controller
{
    public class EnemyManager:BaseController, IUpdateable
    {
        private UpdateManager _updateManager;
        private List<ZombieControllerBase> _liveZombiControllerList = new List<ZombieControllerBase>();
        private List<ZombieControllerBase> _zombiePool = new List<ZombieControllerBase>();
        private List<Transform> _characterTransformList;
        private List<Transform> _enemySpawnPoints;
        private List<Transform> _usedEnemySpawnPoints = new List<Transform>();

        private int _maxZombieCount = 18;
        private float _curentResurrectTime;
        private const float _resurrectTime = 2;
        public bool IsActive => throw new System.NotImplementedException();

        public EnemyManager(UpdateManager updateManager, List<Transform> characterTransformList, List<Transform> enemySpawnPoints)
        {
            _updateManager = updateManager;
            _characterTransformList = characterTransformList;
            _enemySpawnPoints = enemySpawnPoints;
            _updateManager.UpdateList.Add(this);
            for (int i = 0; i < _maxZombieCount / 2; i++)
            {
                ResurrectZombies();
            }
        }

        private void ResurrectZombies()
        {
            if (_zombiePool.Count > 0)
            {
                var zombieController = _zombiePool[0];
                var randomTarget = GetRandomTarget();

                if (PhotonNetwork.IsMasterClient)
                {
                    var randomSpawnPoint = GetRandomSpawnPoint();
                    var newZombieController = new MasterClientZombiController(zombieController, randomTarget, randomSpawnPoint, GetNewTargetForZombie);
                    zombieController.Dispose();
                    zombieController = newZombieController;
                }
                _zombiePool.RemoveAt(0);

                zombieController.ResurrectZombies();
                zombieController.onZombieDie += OnZombieDie;
                _updateManager.FixUpdateList.Add(zombieController);
                _liveZombiControllerList.Add(zombieController);

                if (_zombiePool.Count > 0)
                    _curentResurrectTime = _resurrectTime;
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                var randomTarget = GetRandomTarget();
                var randomSpawnPoint = GetRandomSpawnPoint();
                var newZombieController = new MasterClientZombiController(); //getting from pool
                var zombieController = new MasterClientZombiController(newZombieController, randomTarget, randomSpawnPoint, GetNewTargetForZombie);
                newZombieController.Dispose();

                zombieController.ResurrectZombies();
                zombieController.onZombieDie += OnZombieDie;
                _updateManager.FixUpdateList.Add(zombieController);
                _liveZombiControllerList.Add(zombieController);
            }

            if (_liveZombiControllerList.Count < _maxZombieCount)
                _curentResurrectTime = _resurrectTime;
        }
        private Transform GetRandomTarget()
        {
            if (_characterTransformList.Count == 0)
                return null;

            var randomTargetIndex = Random.Range(0, _characterTransformList.Count);
            while (_characterTransformList[randomTargetIndex] == null)
            {
                _characterTransformList.RemoveAt(randomTargetIndex);
                randomTargetIndex = Random.Range(0, _characterTransformList.Count);
            }

            return _characterTransformList[randomTargetIndex];
        }
        private Transform GetRandomSpawnPoint()
        {
            var randomIndex = Random.Range(0, _enemySpawnPoints.Count);
            var spawnPoint = _enemySpawnPoints[randomIndex];
            _enemySpawnPoints.Remove(spawnPoint);
            _usedEnemySpawnPoints.Add(spawnPoint);

            if (_usedEnemySpawnPoints.Count > _maxZombieCount / 2)
            {
                var oldSpawnPoint = _usedEnemySpawnPoints[0];
                _usedEnemySpawnPoints.RemoveAt(0);
                _enemySpawnPoints.Add(oldSpawnPoint);
            }

            return spawnPoint;
        }

        private void OnZombieDie(ZombieControllerBase zombieController)
        {
            _zombiePool.Add(zombieController);
            _liveZombiControllerList.Remove(zombieController);
            zombieController.onZombieDie -= OnZombieDie;
            _updateManager.FixUpdateList.Remove(zombieController);

            if (_curentResurrectTime<=0)
            _curentResurrectTime = _resurrectTime;
        }

        public void RegisterEnemy(ZombieView view)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                var zombieController = new RemoteZombieController(view);
                zombieController.onZombieDie += OnZombieDie;
                _updateManager.FixUpdateList.Add(zombieController);
                _liveZombiControllerList.Add(zombieController);
            }
        }

        protected override void OnDispose()
        {
            _updateManager.UpdateList.Remove(this);

            for (int i = 0; i < _liveZombiControllerList.Count; i++)
            {
                var zombieController = _liveZombiControllerList[i];
                _updateManager.FixUpdateList.Remove(zombieController);
                zombieController.onZombieDie -= OnZombieDie;
                zombieController.Dispose();
            }
            for (int i = 0; i < _zombiePool.Count; i++)
            {
                var zombieController = _zombiePool[i];
                zombieController.Dispose();
            }

            base.OnDispose();
        }

        public void OnMasterClientSwitched()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < _liveZombiControllerList.Count; i++)
                {
                    var zombieController = _liveZombiControllerList[i];
                    _updateManager.FixUpdateList.Remove(zombieController);
                    zombieController.onZombieDie -= OnZombieDie;

                    var randomTarget = GetRandomTarget();
                    var randomSpawnPoint = GetRandomSpawnPoint();

                    var newZombieController = new MasterClientZombiController(zombieController, randomTarget, randomSpawnPoint, GetNewTargetForZombie);
                    zombieController.Dispose();

                    _liveZombiControllerList[i] = newZombieController;
                    _updateManager.FixUpdateList.Add(newZombieController);
                    newZombieController.onZombieDie += OnZombieDie;
                }
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
            if (_zombiePool.Count == 0 && _liveZombiControllerList.Count == _maxZombieCount)
                return;
            _curentResurrectTime -= Time.deltaTime;
            if (_curentResurrectTime <= 0)
            {

                ResurrectZombies();
            }
        }
    }
}



