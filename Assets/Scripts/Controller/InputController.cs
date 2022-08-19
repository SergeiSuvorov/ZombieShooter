using Tools;
using UnityEngine;


namespace Controller
{
    public class InputController:BaseController, IUpdateable
    {
        private InputView _view;

        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = ViewPathLists.InputView};

        public bool IsActive { get; set; }

        public SubscriptionProperty<Vector2> MoveDiff { get; private set; }
        public SubscriptionProperty<Vector2> RotateDiff { get; private set; }
        public SubscriptionProperty<bool> IsFire { get; private set; }
        public InputController(SubscriptionProperty<Vector2> moveDiff, SubscriptionProperty<Vector2> rotateDiff, SubscriptionProperty<bool> isFire)
        {
            _view = LoadView();
            _view.Init(moveDiff, rotateDiff, isFire);

            MoveDiff = moveDiff;
            RotateDiff = rotateDiff;
            IsFire = isFire;
        }

        public InputView LoadView()
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath));
            AddGameObjects(objectView);

            return objectView.GetComponent<InputView>();
        }

        public void UpdateExecute()
        {
            _view.CheckInput();
        }
    }
}

