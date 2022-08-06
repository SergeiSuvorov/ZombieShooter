using Tools;
using UnityEngine;


namespace Controller
{
    public class GameController : BaseController
    {
        private MapController _mapController;
        public GameController()
        {
            var MoveDiff = new SubscriptionProperty<Vector2>();
            _mapController = new MapController();
        }
    }
}

