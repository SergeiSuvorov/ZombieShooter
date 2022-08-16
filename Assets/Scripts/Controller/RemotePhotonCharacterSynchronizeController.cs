using Photon.Pun;
using Tools;
using UnityEngine;


namespace Controller
{
    public class RemotePhotonCharacterSynchronizeController : BaseController, IPhotonCharacterController
    {
        private CharacterView _view;
        private Rigidbody _rigidbody;

        private float _distance;
        private float _angle;

        private Vector3 _networkPosition;
        private Quaternion _networkRotation;
        private bool _firstTake = true;
        private SubscriptionProperty<bool> _isFire;


        public RemotePhotonCharacterSynchronizeController(CharacterView view, SubscriptionProperty<bool> isFire)
        {
            _view = view;
            _rigidbody = _view.GetComponent<Rigidbody>();

            _view.onPhotonSerializeView += OnPhotonSerializeView;

            Debug.Log("RemotePlayerController");

            _networkPosition = Vector3.zero;
            _networkRotation = Quaternion.identity;
            _isFire = isFire;
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            _view.onPhotonSerializeView -= OnPhotonSerializeView;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsReading)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                _networkPosition = (Vector3)stream.ReceiveNext();
                _networkRotation = (Quaternion)stream.ReceiveNext();
                _rigidbody.velocity = (Vector3)stream.ReceiveNext();
                _rigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
                _isFire.Value = (bool)stream.ReceiveNext();

                _networkPosition += _rigidbody.velocity * lag;
                _distance = Vector3.Distance(_rigidbody.position, _networkPosition);
                
                if (_firstTake)
                {
                    _view.SetWordPositionAndRotation(_networkPosition, _networkRotation);
                    _distance = 0f;
                    _angle = 0f;

                    _firstTake = false;
                }
                _networkRotation = Quaternion.Euler(_rigidbody.angularVelocity * lag) * _networkRotation;
                _angle = Quaternion.Angle(_rigidbody.rotation, _networkRotation);                
            }
        }

        public void SynhronizeExecute()
        {
            _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, _networkPosition, _distance *  (1.0f / PhotonNetwork.SerializationRate));
            _rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, _networkRotation, _angle *  (1.0f / PhotonNetwork.SerializationRate));
        }
    }
}


