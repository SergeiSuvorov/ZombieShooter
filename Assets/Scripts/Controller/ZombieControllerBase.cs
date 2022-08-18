using System;
using Tools;
using UnityEngine;

namespace Controller
{
    abstract public class ZombieControllerBase : BaseController, IFixUpdateable
    {
        protected ZombieView _view;
        protected bool _isLife;
        protected bool _isAttack;
        public ZombieView View => _view;
        public Action<ZombieControllerBase> onZombieDie;
        public bool IsActive { get; set; }

        public void RemoveGameObjectFromList()
        {
            RemoveGameObjects(_view.gameObject);
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



