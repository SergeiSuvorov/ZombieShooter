using Photon.Pun;
using Tools;
using UnityEngine;


namespace Controller
{
    public class RemotePhotonCharacterSynchronizeController : BaseController, IPhotonCharacterController
    {
        private CharacterView _view;
        private Transform _transform;
        private Vector3 _direction;


        private float _distance;
        private float _angle;

        private Vector3 _networkPosition;
        private Quaternion _networkRotation;
        private bool _firstTake = true;
        
        public RemotePhotonCharacterSynchronizeController(CharacterView view)
        {
            _view = view;
            _transform = _view.transform;
            _view.onPhotonSerializeView += OnPhotonSerializeView;

            Debug.Log("RemotePlayerController");

            _networkPosition = Vector3.zero;
            _networkRotation = Quaternion.identity;
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
                _networkPosition = (Vector3)stream.ReceiveNext();
                _direction = (Vector3)stream.ReceiveNext();
                _networkRotation = (Quaternion)stream.ReceiveNext();

                if (_firstTake)
                {
                    _view.SetWordPositionAndRotation(_networkPosition, _networkRotation);
                    _distance = 0f;
                    _angle = 0f;

                    _firstTake=false;
                }
                else
                {
                    float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                    _networkPosition += _direction * lag;
                    _distance = Vector3.Distance(_transform.position, _networkPosition);
                    _angle = Quaternion.Angle(_transform.rotation, _networkRotation);
                }
            }
        }

        public void SynhronizeExecute()
        {
            var position = Vector3.MoveTowards(_transform.position, _networkPosition, _distance * Time.deltaTime * PhotonNetwork.SerializationRate);
            var rotation = Quaternion.RotateTowards(_transform.rotation, _networkRotation, _angle * Time.deltaTime * PhotonNetwork.SerializationRate);
            _view.SetWordPositionAndRotation(position,rotation);
        }
    }
}

