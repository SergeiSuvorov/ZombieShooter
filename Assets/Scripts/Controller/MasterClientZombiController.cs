using Photon.Pun;
using Tools;
using UnityEngine;


namespace Controller
{
    public class MasterClientZombiController : ZombieControllerBase
    {
        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = ViewPathLists.ZombieView };

        private StalkerAIController _stalkerAIController;
        private Rigidbody _rigidbody;
        public MasterClientZombiController(Transform targetTransform)
        {
            _view = LoadZombiView();
            AddGameObjects(_view.gameObject);
            _view.onPhotonSerializeView += OnPhotonSerializeView;
            _rigidbody = _view.Rigidbody;
            _stalkerAIController = new StalkerAIController(_view, targetTransform);
        }

        public MasterClientZombiController(ZombieControllerBase removeZombie, Transform targetTransform)
        {
            _view = removeZombie.View;
            removeZombie.RemoveGameObjectFromList();
            removeZombie.Dispose();
            AddGameObjects(_view.gameObject);
            _stalkerAIController = new StalkerAIController(_view, targetTransform);
            _view.onPhotonSerializeView += OnPhotonSerializeView;
        }

        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_rigidbody.position);
                stream.SendNext(_rigidbody.rotation);
                stream.SendNext(_rigidbody.velocity);
                stream.SendNext(_rigidbody.angularVelocity);
            }
        }

        public ZombieView LoadZombiView()
        {
            var objectView = PhotonNetwork.InstantiateRoomObject(_viewPath.PathResource, new Vector3(2f, 1f, 2f), Quaternion.identity, 0);

            return objectView.GetComponent<ZombieView>();
        }

        public override void FixUpdateExecute()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            _stalkerAIController.FixUpdateExecute();
        }
    }
}



