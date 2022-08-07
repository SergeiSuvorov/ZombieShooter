using Tools;
using UnityEngine;


namespace Controller
{
    public class CharacterController : BaseController, IUpdateable, IFixUpdateable, ILateUpdateable
    {
        private CharacterView _view;
        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = ViewPathLists.CharacterView };
        private SubscriptionProperty<Vector2> _moveDiff;
        private SubscriptionProperty<Vector2> _rotateDiff;

        private FollowCameraController _cameraController;

        public bool IsActive { get; set; }

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

        public CharacterView LoadView()
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath));
            AddGameObjects(objectView);

            return objectView.GetComponent<CharacterView>();
        }

        private void Rotate(Vector2 rotateDiff)
        {
            _view.SetRotate(rotateDiff);
        }

        private void Move(Vector2 moveDif)
        {
            _view.SetMove(moveDif);
        }

        public void UpdateExecute()
        {
            _view.UpdateExecute();
        }

        public void FixUpdateExecute()
        {
            _view.FixUpdateExecute();
        }

        public void LateUpdateExecute()
        {
            _cameraController.LateUpdateExecute();
        }

        protected override void OnDispose()
        {
            _moveDiff.UnSubscriptionOnChange(Move);
            _rotateDiff.UnSubscriptionOnChange(Rotate);

            base.OnDispose();
        }

    }
}

