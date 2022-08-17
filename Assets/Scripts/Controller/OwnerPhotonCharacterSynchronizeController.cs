using Photon.Pun;
using Tools;
using UnityEngine;


namespace Controller
{
    public class OwnerPhotonCharacterSynchronizeController : BasePhotonPlayerCharacterController
    {
        private CharacterView _view;
        private Transform _transform;
        private Vector3 _direction;
        private Rigidbody _rigidbody;
        private Vector3 _storedPosition;

        private SubscriptionProperty<bool> _isFire;
        public OwnerPhotonCharacterSynchronizeController(CharacterView view, SubscriptionProperty<bool> isFire)
        {
            _view = view;
            _rigidbody = _view.GetComponent<Rigidbody>();
            _transform = _view.transform;
            _view.onPhotonSerializeView += OnPhotonSerializeView;
            _isFire = isFire;
            Debug.Log("OwnerPlayerController");
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            _view.onPhotonSerializeView -= OnPhotonSerializeView;
        }

        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_rigidbody.position);
                stream.SendNext(_rigidbody.rotation);
                stream.SendNext(_rigidbody.velocity);
                stream.SendNext(_rigidbody.angularVelocity);
                stream.SendNext(_isFire.Value);
            }
        }

        public override void SynhronizeExecute()
        {
        }
    }
}

