using Controller;
using Photon.Pun;
using System;
using UnityEngine;
using Pathfinding;

public class ZombieView: MonoBehaviourPunCallbacks, IPunObservable, IDamageReceiver
{
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private AIPath _aIPath;
    [SerializeField]
    private Seeker _seeker;

    public Transform Transform => transform;
    public Rigidbody Rigidbody => _rigidbody;

    public Seeker Seeker => _seeker;

    public Action<PhotonStream, PhotonMessageInfo> onPhotonSerializeView;

    public event Action<int> onGetDamage;

    public Action<IDamageReceiver> onCollisionStay;

    private void Awake()
    {
        GameController.Instance?.RegisterEnemy(this);
        _aIPath.enabled = false;
    }
    public void Init()
    {
        _aIPath.enabled = true;
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

    public void GetDamage( int damage)
    {
        //Action onDeathCallback,
        onGetDamage?.Invoke(damage);
    }

    public void OnCollisionStay(Collision collision)
    {
        var damageReceiver = (collision.gameObject.GetComponent<CharacterView>());
        if (damageReceiver != null)
        {
            Debug.Log(collision.gameObject.name);
            onCollisionStay?.Invoke(damageReceiver);
        }
    }
}

