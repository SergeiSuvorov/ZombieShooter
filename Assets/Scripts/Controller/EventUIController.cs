using Tools;
using UnityEngine;


namespace Controller
{
    public class EventUIController : BaseController, IUpdateable
    {
        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = PathLists.EventUIView };
        private EventUIView _view;
        private UpdateManager _updateManager;
        private float _curentMessageTime;
        private float _messageTime = 3;
        private bool _hasMessage;

        public EventUIController(Transform placeForUi, UpdateManager updateManager)
        {
            _view = LoadView(placeForUi);
            _updateManager = updateManager;
            _updateManager.UpdateList.Add(this);
        }

        public bool IsActive => throw new System.NotImplementedException();

        public void ShowMessage(string message, Color color)
        {
            _view.ShowMessage(message, color);
            _curentMessageTime = _messageTime;
            _hasMessage = true;
        }

        public void UpdateExecute()
        {
            if (!_hasMessage)
                return;
            _curentMessageTime -= Time.deltaTime;
            if (_curentMessageTime<=0)
            {
                _hasMessage = false;
                _view.Clear();
            }
        }

        private EventUIView LoadView(Transform placeForUi)
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath), placeForUi, false);
            AddGameObjects(objectView);

            return objectView.GetComponent<EventUIView>();
        }

        protected override void OnDispose()
        {
            _updateManager.UpdateList.Remove(this);
            base.OnDispose();
        }

    }
}



