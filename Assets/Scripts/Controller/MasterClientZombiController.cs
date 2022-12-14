using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using Tools;
using UnityEngine;


namespace Controller
{
    public class MasterClientZombiController : ZombieControllerBase
    {
        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = PathLists.ZombieView };

        private StalkerAIController _stalkerAIController;
        private Rigidbody _rigidbody;
        private int _currentHealth;
        private Transform _spawnPlace;
        public Action<ZombieControllerBase> onZombieNeedNewTarget;

        public MasterClientZombiController() { }


        public MasterClientZombiController(ZombieControllerBase removeZombie, Transform targetTransform, Transform spawnPlace, Action<ZombieControllerBase> SetNewTarget)
        {
            _spawnPlace = spawnPlace;
            if (removeZombie.View == null)
            {
                _view = LoadZombiView();
            }
            else
            {
                _view = removeZombie.View;
                removeZombie.RemoveGameObjectFromList();
            }

            
            _rigidbody = _view.Rigidbody;
            AddGameObjects(_view.gameObject);

            _model = LoadZombiModel();
            _currentHealth = _model.Health;
            _isLife = true;

            _stalkerAIController = new StalkerAIController(_view, targetTransform);
            
            AddController(_stalkerAIController);

            onZombieNeedNewTarget += SetNewTarget;
            Subscribe();
        }

        private void Subscribe()
        {
            _view.onCollisionStay += SetDamage;
            _view.onPhotonSerializeView += OnPhotonSerializeView;
            _view.onGetDamage += GetDamage;

            _stalkerAIController.needNewTarget += NeedNewTarget;
        }

        private void UnSubscribe()
        {
            if (_view != null)
            {
                _view.onPhotonSerializeView -= OnPhotonSerializeView;
                _view.onGetDamage -= GetDamage;
                _view.onCollisionStay -= SetDamage;
            }

            if (_stalkerAIController != null)
            {
                _stalkerAIController.needNewTarget -= NeedNewTarget;
                _stalkerAIController.Dispose();
            }
            onZombieNeedNewTarget = null;
        }

        private void NeedNewTarget()
        {
            onZombieNeedNewTarget?.Invoke(this);
        }

        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_rigidbody.position);
                stream.SendNext(_rigidbody.rotation);
                stream.SendNext(_rigidbody.velocity);
                stream.SendNext(_rigidbody.angularVelocity);
                stream.SendNext(_isLife);
                stream.SendNext(_isAttack);
                stream.SendNext(_currentHealth);
            if (!_isLife)
            {
                stream.SendNext(_spawnPlace.position);
                Died();
                    _view.SetWordPositionAndRotation(_spawnPlace.position, _rigidbody.rotation);
                }
            }
        }

        public ZombieView LoadZombiView()
        {
            var objectView = PhotonNetwork.InstantiateRoomObject(_viewPath.PathResource, _spawnPlace.position, _spawnPlace.rotation, 0);

            return objectView.GetComponent<ZombieView>();
        }

        public override void FixUpdateExecute()
        {
            _stalkerAIController.FixUpdateExecute();

            if (!_isAttack)
                return;

            _currentCoolDawnTime -= Time.fixedDeltaTime;
            if( _currentCoolDawnTime <= 0 )
            {
                _isAttack = false;
                _stalkerAIController.IsActive = true;
            }
        }

        public override void ResurrectZombies()
        {
            _isLife = true;

            if (_model == null)
                _model = LoadZombiModel();
            _currentHealth = _model.Health;
            _view.gameObject.SetActive(true);

            _stalkerAIController.IsActive = true;
            _currentCoolDawnTime = 0;
        }

        protected override void SetDamage(IDamageReceiver damageReceiver)
        {
            if (_isAttack)
                return;

            damageReceiver.GetDamage(_model.Damage, null);
            _isAttack = true;
            _stalkerAIController.IsActive = false;
            _currentCoolDawnTime = _model.CoolDawnTime;
        }

        public void SetNewTarget(Transform target)
        {
            _stalkerAIController.ChangeTarget(target);
        }
        private void GetDamage(int damage, Player player)
        {
            _currentHealth -= damage;
            if (_currentHealth < 0)
            {
                _isLife = false;
                _stalkerAIController.IsActive = false;
                PlayerKillZombieEvent(player);
                if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
                {
                    Died();
                    _view.SetWordPositionAndRotation(_spawnPlace.position, _rigidbody.rotation);
                }
            }
        }

        protected override void OnDispose()
        {
            UnSubscribe();
            base.OnDispose();
        }

        private void PlayerKillZombieEvent(Player player)
        {
            object[] content = new object[] { player };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(PhotonEvenCodeList.PlayerKillsZombieCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}



