using Tools;
using UnityEngine;


namespace Controller
{
    public class GameController : BaseController
    {
        private MapController _mapController;
        private InputController _inputController;
        private CharacterController _characterController;

        public GameController()
        {
            var moveDiff = new SubscriptionProperty<Vector2>();
            var rotateDiff = new SubscriptionProperty<Vector2>();
            var isFire = new SubscriptionProperty<bool>();

            _mapController = new MapController();
            AddController(_mapController);

            _inputController = new InputController(moveDiff, rotateDiff, isFire);
            AddController(_inputController);

            _characterController = new CharacterController(moveDiff, rotateDiff);
            AddController(_characterController);
        }
    }
}

