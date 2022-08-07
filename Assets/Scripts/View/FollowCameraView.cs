using UnityEngine;

public class FollowCameraView: MonoBehaviour
{
    [SerializeField]
    private float _distance = 7.0f;

    [SerializeField]
    private float _height = 3.0f;

    [SerializeField]
    private Vector3 _centerOffset = Vector3.zero;

    [SerializeField]
    private float _smoothSpeed = 0.75f;

    private Transform _cameraTransform;
    private Transform _targetTransform;

    private bool _isFollowing;

    private Vector3 _cameraOffset = Vector3.zero;

    public void Init(Transform targetTransform)
    {
        _cameraTransform = Camera.main.transform;
        _targetTransform = targetTransform;
        _isFollowing = true;

        Cut();
    }

    public void Follow()
    {
        if (!_isFollowing)
            return;

        _cameraOffset.z = -_distance;
        _cameraOffset.y = _height;

        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _targetTransform.position + _targetTransform.TransformVector(_cameraOffset), _smoothSpeed * Time.deltaTime);

        _cameraTransform.LookAt(_targetTransform.position + _centerOffset);
    }


    public void Cut()
    {
        _cameraOffset.z = -_distance;
        _cameraOffset.y = _height;

        _cameraTransform.position = _targetTransform.position + _targetTransform.TransformVector(_cameraOffset);

        _cameraTransform.LookAt(_targetTransform.position + _centerOffset);
    }
}

