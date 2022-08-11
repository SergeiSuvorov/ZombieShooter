using Photon.Pun;
using Tools;
using UnityEngine;


namespace Controller
{
    public class OwnerPhotonCharacterSynchronizeController : BaseController, IPhotonCharacterController
    {
        private CharacterView _view;
        private Transform _transform;
        private Vector3 _direction;

        private Vector3 _storedPosition;

        public OwnerPhotonCharacterSynchronizeController(CharacterView view)
        {
            _view = view;
            _transform = _view.transform;
            _view.onPhotonSerializeView += OnPhotonSerializeView;

            Debug.Log("OwnerPlayerController");
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            _view.onPhotonSerializeView -= OnPhotonSerializeView;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                _direction = _transform.position - _storedPosition;
                _storedPosition = _transform.position;
                stream.SendNext(_transform.position);
                stream.SendNext(_direction);
                stream.SendNext(_transform.rotation);
            }
        }

        public void SynhronizeExecute()
        {
        }
    }
}

