using Controller;
using Photon.Pun;
using System;
using UnityEngine;

public class CharacterView : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    private Transform _lookTransformRoot;
    [SerializeField]
    private Transform _weaponTransformRoot;
    [SerializeField]
    private float _turnSmooth;
    [SerializeField]
    private float _turnSpeed;
    [SerializeField]
    private Rigidbody _rigidbody;


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

    public Transform WeaponTransformRoot => _weaponTransformRoot;
    public bool IsActive { get; set; }

    public Action<PhotonStream, PhotonMessageInfo> onPhotonSerializeView;

    private void Awake()
    {
        Debug.Log($"Awake { GameController.Instance!=null}");

       GameController.Instance?.RegisterPlayer(this);
    }
    public void Init()
    {
        gameObject.name = photonView.Owner.NickName;
    }

    public void SetMove(Vector3 moveDif)
    {
        _xMove = moveDif.x;
        _zMove = moveDif.y;
    }

    public void SetRotate(Vector3 rotateDif)
    {
        _xRotate = rotateDif.x;
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
        _lookTransformRoot.rotation = targetLookRot;

        //_weaponTransformRoot.rotation = Quaternion.Euler(lookAngleY, lookAngleX, 0);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        onPhotonSerializeView?.Invoke(stream, info);
    }

    public void SetWordPositionAndRotation(Vector3 position, Quaternion quaternion)
    {
        transform.position = position;
        transform.rotation = quaternion;
    }
}

