using Tools;
using UnityEngine;


namespace Controller
{
    public class FollowCameraController:BaseController
    {
        private FollowCameraView _view;
        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = "Prefabs/FollowCameraView" };

        public FollowCameraController(Transform characterTransform)
        {
            _view = LoadView();
            _view.Init(characterTransform);
        }

        private FollowCameraView LoadView()
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath));
            AddGameObjects(objectView);

            return objectView.GetComponent<FollowCameraView>();
        }
    }
}

