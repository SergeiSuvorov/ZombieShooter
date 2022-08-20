using Photon.Pun;
using Tools;
using UnityEngine;


namespace Controller
{
    public class OwnerPhotonCharacterSynchronizeController : BasePhotonPlayerCharacterController
    {
        private CharacterView _view;
        private Rigidbody _rigidbody;


        private SubscriptionProperty<bool> _isFire;
        public OwnerPhotonCharacterSynchronizeController(CharacterView view, SubscriptionProperty<bool> isFire)
        {
            _view = view;
            _rigidbody = _view.GetComponent<Rigidbody>();
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

