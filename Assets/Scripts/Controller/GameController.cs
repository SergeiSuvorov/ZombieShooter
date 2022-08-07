using Tools;
using UnityEngine;


namespace Controller
{
    public class GameController : BaseController
    {
        private MapController _mapController;
        private InputController _inputController;
        private CharacterController _characterController;
        private UpdateManager _updateManager;
        public GameController(UpdateManager updateManager)
        {
            _updateManager = updateManager;

            var moveDiff = new SubscriptionProperty<Vector2>();
            var rotateDiff = new SubscriptionProperty<Vector2>();
            var isFire = new SubscriptionProperty<bool>();

            _mapController = new MapController();
            AddController(_mapController);

            _inputController = new InputController(moveDiff, rotateDiff, isFire);
            AddController(_inputController);

            _characterController = new CharacterController(moveDiff, rotateDiff);
            AddController(_characterController);

            _updateManager.UpdateList.Add(_inputController);
            _updateManager.UpdateList.Add(_characterController);
            _updateManager.LateUpdateList.Add(_characterController);
            _updateManager.FixUpdateList.Add(_characterController);
        }

        protected override void OnDispose()
        {
            _updateManager.UpdateList.Remove(_inputController);
            _updateManager.UpdateList.Remove(_characterController);
            _updateManager.LateUpdateList.Remove(_characterController);
            _updateManager.FixUpdateList.Remove(_characterController);
            base.OnDispose();
        }
    }
}

