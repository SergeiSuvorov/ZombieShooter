using Tools;
using UnityEngine;


namespace Controller
{
    public class CharacterController : BaseController
    {
        private CharacterView _view;
        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = "Prefabs/Character0" };
        private SubscriptionProperty<Vector2> _moveDiff;
        private SubscriptionProperty<Vector2> _rotateDiff;

        private FollowCameraController _cameraController;

        public CharacterController(SubscriptionProperty<Vector2> moveDiff, SubscriptionProperty<Vector2> rotateDiff)
        {
            _view = LoadView();
            _view.Init();

            _cameraController = new FollowCameraController(_view.transform);
            AddController(_cameraController);

            _moveDiff = moveDiff;
            _moveDiff.SubscribeOnChange(Move);
            _rotateDiff = rotateDiff;
            _rotateDiff.SubscribeOnChange(Rotate);
        }

        private void Rotate(Vector2 rotateDiff)
        {
            _view.SetRotate(rotateDiff);
        }

        private void Move(Vector2 moveDif)
        {
            _view.SetMove(moveDif);
        }

        public CharacterView LoadView()
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath));
            AddGameObjects(objectView);

            return objectView.GetComponent<CharacterView>();
        }

        protected override void OnDispose()
        {
            _moveDiff.UnSubscriptionOnChange(Move);
            _rotateDiff.UnSubscriptionOnChange(Rotate);
            base.OnDispose();
        }
    }
}

