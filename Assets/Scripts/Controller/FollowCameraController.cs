using Model;
using Tools;
using UnityEngine;


namespace Controller
{
    public class FollowCameraController:BaseController, ILateUpdateable
    {
        private FollowCameraView _view;
        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = ViewPathLists.FollowCameraView };
        private readonly ResourcePath _modelPath = new ResourcePath { PathResource = ViewPathLists.FollowCameraModel };
        public bool IsActive { get; set; }

        public FollowCameraController(Transform characterTransform)
        {
            _view = LoadView();
            var model = LoadModel();
            if (model != null)
            {
                var distance = model.Distance;
                var height = model.Height;
                var smoothSpeed = model.SmoothSpeed;
                _view.Init(characterTransform, distance, height, smoothSpeed);
            }
        }

        private FollowCameraModel LoadModel()
        {
            var objectModel = Object.Instantiate(ResourceLoader.LoadScriptable(_modelPath));

            return (objectModel as FollowCameraModel);
        }

        private FollowCameraView LoadView()
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath));
            AddGameObjects(objectView);

            return objectView.GetComponent<FollowCameraView>();
        }

        public void LateUpdateExecute()
        {
            _view.Follow();
        }

    }
}

