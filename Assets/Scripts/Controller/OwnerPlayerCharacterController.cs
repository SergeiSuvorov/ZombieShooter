using Model;
using Tools;
using UnityEngine;


namespace Controller
{
    public class OwnerPlayerCharacterController : BasePlayerCharacterController
    {
        private SubscriptionProperty<Vector2> _moveDiff;
        private SubscriptionProperty<Vector2> _rotateDiff;
        private SubscriptionProperty<bool> _isFire;
        private PlayerModel _playerModel;
        public OwnerPlayerCharacterController(SubscriptionProperty<Vector2> moveDiff, SubscriptionProperty<Vector2> rotateDiff, SubscriptionProperty<bool> isFire, CharacterView view, PlayerModel playerModel)
        {
            InitView(view);

            var weaponView = LoadWeaponView(view.WeaponTransformRoot);
            _weaponController = new WeaponController(weaponView, isFire);
            AddController(_weaponController);

            _isFire = isFire;
            _playerModel = playerModel;
            _photonCharacterController = new OwnerPhotonCharacterSynchronizeController(view, isFire);

            _moveDiff = moveDiff;
            _moveDiff.SubscribeOnChange(Move);
            _rotateDiff = rotateDiff;
            _rotateDiff.SubscribeOnChange(Rotate);
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

