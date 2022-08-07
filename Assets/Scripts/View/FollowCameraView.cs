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
    private float _smoothSpeed = 0.125f;

    Transform _cameraTransform;
    Transform _targetTransform;

    bool isFollowing;

    Vector3 cameraOffset = Vector3.zero;

    public void Init(Transform targetTransform)
    {
        _cameraTransform = Camera.main.transform;
        _targetTransform = targetTransform;
        isFollowing = true;

        Cut();
    }

    private void LateUpdate()
    {
        if (isFollowing)
        {
            Follow();
        }
    }

    void Follow()
    {
        cameraOffset.z = -_distance;
        cameraOffset.y = _height;

        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _targetTransform.position + _targetTransform.TransformVector(cameraOffset), _smoothSpeed * Time.deltaTime);

        _cameraTransform.LookAt(_targetTransform.position + _centerOffset);
    }


    void Cut()
    {
        cameraOffset.z = -_distance;
        cameraOffset.y = _height;

        _cameraTransform.position = _targetTransform.position + _targetTransform.TransformVector(cameraOffset);

        _cameraTransform.LookAt(_targetTransform.position + _centerOffset);
    }
}

