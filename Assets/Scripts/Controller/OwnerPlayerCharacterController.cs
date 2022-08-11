using Photon.Pun;
using Tools;
using UnityEngine;


namespace Controller
{
    public class OwnerPlayerCharacterController : BasePlayerCharacterController, IFixUpdateable
    {
        private SubscriptionProperty<Vector2> _moveDiff;
        private SubscriptionProperty<Vector2> _rotateDiff;

        public OwnerPlayerCharacterController(SubscriptionProperty<Vector2> moveDiff, SubscriptionProperty<Vector2> rotateDiff, CharacterView view)
        {
            InitView(view);

            _moveDiff = moveDiff;
            _moveDiff.SubscribeOnChange(Move);
            _rotateDiff = rotateDiff;
            _rotateDiff.SubscribeOnChange(Rotate);

            _photonCharacterController = new OwnerPhotonCharacterSynchronizeController(view);
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
        }

        public virtual void FixUpdateExecute()
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

