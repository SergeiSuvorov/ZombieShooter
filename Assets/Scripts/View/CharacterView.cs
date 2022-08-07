using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField]
    private Transform _lookTransform;

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

    private void FixedUpdate()
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

    private void Update()
    {
        smoothX = Mathf.SmoothDamp(smoothX, _xRotate, ref smoothXVelocity, 0.1f);
        smoothY = Mathf.SmoothDamp(smoothY, _yRotate, ref smoothYVelocity, 0.1f);

        lookAngleX += smoothX*3;
        Quaternion targetRot = Quaternion.Euler(0, lookAngleX, 0);
        transform.rotation = targetRot;

        lookAngleY += smoothY * 3;
        Quaternion targetLookRot = Quaternion.Euler(-lookAngleY, lookAngleX, 0);
        _lookTransform.rotation = targetLookRot;
    }

}

