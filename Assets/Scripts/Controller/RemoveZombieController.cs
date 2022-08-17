using Photon.Pun;
using UnityEngine;


namespace Controller
{
    public class RemoveZombieController : ZombieControllerBase
    {
        private Rigidbody _rigidbody;

        private float _distance;
        private float _angle;

        private Vector3 _networkPosition;
        private Quaternion _networkRotation;
        private bool _firstTake = true;
        public RemoveZombieController(ZombieView view)
        {
            _view = view;
            AddGameObjects(_view.gameObject);
            _view.onPhotonSerializeView += OnPhotonSerializeView;
            _rigidbody = _view.Rigidbody;
        }

        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsReading)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                _networkPosition = (Vector3)stream.ReceiveNext();
                _networkRotation = (Quaternion)stream.ReceiveNext();
                _rigidbody.velocity = (Vector3)stream.ReceiveNext();
                _rigidbody.angularVelocity = (Vector3)stream.ReceiveNext();

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

        public override void FixUpdateExecute()
        {
            _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, _networkPosition, _distance * (1.0f / PhotonNetwork.SerializationRate));
            _rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, _networkRotation, _angle * (1.0f / PhotonNetwork.SerializationRate));
        }
    }
}



