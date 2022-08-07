using Tools;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField]
    private Transform _lookTransform;
    [SerializeField]
    private float _turnSmooth;
    [SerializeField]
    private float _turnSpeed;

    private float _xMove;
    private float _zMove;
    private float _xRotate;
    private float _yRotate;

    private float smoothX;
    private float smoothY;
    private float smoothXVelocity;
    private float smoothYVelocity;
    private float lookAngleY;
    private float lookAngleX;

    private Rigidbody _rigidbody;

    public bool IsActive { get; set; }

    public void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetMove(Vector2 moveDif)
    {
        _xMove = moveDif.x;
        _zMove = moveDif.y; 
    }

    public void SetRotate(Vector2 rotateDif)
    {
        _xRotate= rotateDif.x;
        _yRotate= rotateDif.y;
    }

    public void UpdateExecute()
    {
        smoothX = Mathf.SmoothDamp(smoothX, _xRotate, ref smoothXVelocity, _turnSmooth);
        smoothY = Mathf.SmoothDamp(smoothY, _yRotate, ref smoothYVelocity, _turnSmooth);

        lookAngleX += smoothX * _turnSpeed;
        Quaternion targetRot = Quaternion.Euler(0, lookAngleX, 0);
        transform.rotation = targetRot;

        lookAngleY += smoothY * _turnSpeed;
        Quaternion targetLookRot = Quaternion.Euler(-lookAngleY, lookAngleX, 0);
        _lookTransform.rotation = targetLookRot;
    }
    public void FixUpdateExecute()
    {
        if (_xMove != 0f || _zMove != 0f)
        {
            var movement = transform.forward * _xMove * Time.fixedDeltaTime;
            movement += transform.right * _zMove * Time.fixedDeltaTime;

            _rigidbody.MovePosition(_rigidbody.position + movement);

            _xMove = 0f;
            _zMove = 0f;
        }
    }
}

