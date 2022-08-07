using Tools;
using UnityEngine;


namespace Controller
{
    public class MapController : BaseController
    {
        private MapView _view;
        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = ViewPathLists.MapView };

        public MapController()
        {
            _view = LoadView();
        }

        private MapView LoadView()
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath));
            AddGameObjects(objectView);

            return objectView.GetComponent<MapView>();
        }
    }
}

