using Tools;
using UnityEngine;


namespace Controller
{
    public class ResultTableUIController : BaseController
    {
        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = ViewPathLists.ResultTableView };
        private ResultTableView _view;

        public ResultTableUIController(Transform placeForUi)
        {
            _view = LoadView(placeForUi);
        }

        private ResultTableView LoadView(Transform placeForUi)
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath), placeForUi, false);
            AddGameObjects(objectView);

            return objectView.GetComponent<ResultTableView>();
        }

        public void ShowTable(string table)
        {
            _view.ShowResult(table);
        }
    }
}



