using UnityEngine;

public class FollowCameraView: MonoBehaviour
{
    private float _distance;

    private float _height;

    private float _smoothSpeed;

    private Transform _cameraTransform;
    private Transform _targetTransform;

    private bool _isFollowing;

    private Vector3 _cameraOffset = Vector3.zero;


    public void Init(Transform targetTransform, float distance, float height, float smoothSpeed)
    {
        _cameraTransform = Camera.main.transform;
        _targetTransform = targetTransform;

        _distance=distance;
        _height=height; 
        _smoothSpeed=smoothSpeed;

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

        _cameraTransform.LookAt(_targetTransform.position /*+ _centerOffset*/);
    }


    public void Cut()
    {
        _cameraOffset.z = -_distance;
        _cameraOffset.y = _height;

        _cameraTransform.position = _targetTransform.position + _targetTransform.TransformVector(_cameraOffset);

        _cameraTransform.LookAt(_targetTransform.position /*+ _centerOffset*/);
    }
}

