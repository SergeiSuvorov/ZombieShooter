using Model;
using System;
using Tools;
using UnityEngine;


namespace Controller
{
    public class OwnerPlayerCharacterController : BasePlayerCharacterController
    {
        private SubscriptionProperty<Vector2> _moveDiff;
        private SubscriptionProperty<Vector2> _rotateDiff;
        private SubscriptionProperty<bool> _isFire;
        private SubscriptionProperty<int> _currentHealth=new SubscriptionProperty<int>();
        private PlayerModel _playerModel;
        public Action onPlayerDied;
        public OwnerPlayerCharacterController(SubscriptionProperty<Vector2> moveDiff, SubscriptionProperty<Vector2> rotateDiff, SubscriptionProperty<bool> isFire, CharacterView view, PlayerModel playerModel)
        {
            InitView(view);
            _view.onGetDamage += GetDamage;

            _weaponController = new WeaponController(view.WeaponTransformRoot, isFire);
            AddController(_weaponController);

            _isFire = isFire;
            _playerModel = playerModel;
            _photonCharacterController = new OwnerPhotonCharacterSynchronizeController(view, isFire);
            AddController(_photonCharacterController);

            _moveDiff = moveDiff;
            _moveDiff.SubscribeOnChange(Move);
            _rotateDiff = rotateDiff;
            _rotateDiff.SubscribeOnChange(Rotate);
            _currentHealth.Value = _playerModel.PlayerHealth;
        }

        private void GetDamage(int damage)
        {
            _currentHealth.Value -= damage;
            if (_currentHealth.Value <= 0)
                onPlayerDied?.Invoke();
        }

        protected virtual void Rotate(Vector2 rotateDiff)
        {
            _view.SetRotate(rotateDiff);
        }

        protected virtual void Move(Vector2 moveDif)
        {
            _view.SetMove(moveDif);
        }

        public override void UpdateExecute()
        {
            _view.UpdateExecute();
            _weaponController.Aiming();
            _weaponController.Timer();

            if (_isFire.Value)
                _weaponController.CheckReadyToAction();
        }

        public override void FixUpdateExecute()
        {
            _view.FixUpdateExecute();
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _moveDiff.UnSubscriptionOnChange(Move);
            _rotateDiff.UnSubscriptionOnChange(Rotate);

        }
    }
}

