using Model;
using Photon.Pun;
using Tools;
using UnityEngine;


namespace Controller
{
    public class MasterClientZombiController : ZombieControllerBase
    {
        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = ViewPathLists.ZombieView };
        private readonly ResourcePath _modelPath = new ResourcePath { PathResource = ViewPathLists.ZombieModel };

        private StalkerAIController _stalkerAIController;
        private Rigidbody _rigidbody;
        private ZombieModel _model;
        private int _currentHealth;
        private float _currentCoolDawnTime;

        public MasterClientZombiController(Transform targetTransform)
        {
            _view = LoadZombiView();
            _rigidbody = _view.Rigidbody;
            _view.onPhotonSerializeView += OnPhotonSerializeView;
            _view.onGetDamage += GetDamage;
            _view.onCollisionStay += SetDamage;
            AddGameObjects(_view.gameObject);
            
            _model = LoadZombiModel();
            _currentHealth = _model.Health;
            _isLife = true;
            _stalkerAIController = new StalkerAIController(_view, targetTransform);

        }
        private ZombieModel LoadZombiModel()
        {
            var objectModel = Object.Instantiate(ResourceLoader.LoadScriptable(_modelPath));

            return (objectModel as ZombieModel);
        }

        private void GetDamage(int damage)
        {
            Debug.Log($"GetDamage {damage} CurrentHealth {_currentHealth}");
            _currentHealth -= damage;
            if (_currentHealth < 0)
            {
                _isLife = false;
                _stalkerAIController.IsActive = false;
            }

        }

        public MasterClientZombiController(ZombieControllerBase removeZombie, Transform targetTransform)
        {
            _view = removeZombie.View;
            removeZombie.RemoveGameObjectFromList();
            removeZombie.Dispose();
            AddGameObjects(_view.gameObject);
            _view.onGetDamage += GetDamage;

            _model = LoadZombiModel();
            _currentHealth = _model.Health;
            _isLife = true;

            _view.onPhotonSerializeView += OnPhotonSerializeView;

            _stalkerAIController = new StalkerAIController(_view, targetTransform);
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
            }

            if (!_isLife)
            {
                Died();
            }
        }

        public ZombieView LoadZombiView()
        {
            var objectView = PhotonNetwork.InstantiateRoomObject(_viewPath.PathResource, new Vector3(2f, 1f, 2f), Quaternion.identity, 0);

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

        public override void ResurrectZombies(Vector3 position)
        {
            _isLife = true;
            _currentHealth = _model.Health;
            _view.gameObject.SetActive(true);
            _view.SetWordPositionAndRotation(position, _rigidbody.rotation);
            _stalkerAIController.IsActive = true;
        }

        protected override void SetDamage(IDamageReceiver damageReceiver)
        {
            if (_isAttack)
                return;

            damageReceiver.GetDamage(_model.Damage);
            _isAttack = true;
            _stalkerAIController.IsActive = false;
            _currentCoolDawnTime = _model.CoolDawnTime;
        }
    }
}



