using Model;
using System;
using Tools;
using UnityEngine;

namespace Controller
{
    abstract public class ZombieControllerBase : BaseController, IFixUpdateable
    {
        private readonly ResourcePath _modelPath = new ResourcePath { PathResource = ViewPathLists.ZombieModel };

        protected ZombieModel _model;
        protected ZombieView _view;
        protected bool _isLife;
        protected bool _isAttack;
        protected float _currentCoolDawnTime;
        public ZombieView View => _view;
        public Action<ZombieControllerBase> onZombieDie;
        public bool IsActive { get; set; }


        public void RemoveGameObjectFromList()
        {
            RemoveGameObjects(_view.gameObject);
        }

        protected ZombieModel LoadZombiModel()
        {
            var objectModel = UnityEngine.Object.Instantiate(ResourceLoader.LoadScriptable(_modelPath));

            return (objectModel as ZombieModel);
        }

        protected void Died()
        {
            Debug.Log("Die");
            onZombieDie?.Invoke(this);
            _view.gameObject.SetActive(false);
        }

        abstract protected void SetDamage(IDamageReceiver damageReceiver);

        abstract public void ResurrectZombies(Vector3 position);

        abstract public void FixUpdateExecute();
    }
}



