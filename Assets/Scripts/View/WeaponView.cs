using Photon.Realtime;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    [SerializeField]
    private GameObject _redDotDecal;

    [SerializeField]
    private Transform _visualEffectRoot;

    [SerializeField]
    private Transform _shootPoint;

    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private Vector3 shootVector;

    private Player _ownerPlayer;
    private void Awake()
    {
        _redDotDecal.SetActive(false);
        _visualEffectRoot.gameObject.SetActive(false);
    }

    public void SetOwner(Player ownerPlayer)
    {
        _ownerPlayer = ownerPlayer;
    }

    public void Action(int damage)
    {
        RaycastHit hit;
        if (Physics.Raycast(_shootPoint.position, transform.TransformDirection(shootVector), out hit))
        {
            var damageReceiver = hit.transform.GetComponent<IDamageReceiver>();
            if (damageReceiver != null)
                damageReceiver.GetDamage(damage, _ownerPlayer);
        }
        _audioSource.Play();
    }

    public void SetActiveVisualEffect(bool setActive)
    {
        _visualEffectRoot.gameObject.SetActive(setActive);
    }

    public void RedDotAiming()
    {
        RaycastHit hit;

        if (Physics.Raycast(_shootPoint.position, transform.TransformDirection(shootVector), out hit))
        {
            _redDotDecal.transform.position = hit.point + hit.normal * 0.01f;
            _redDotDecal.transform.rotation = Quaternion.LookRotation(-hit.normal);

            if (!_redDotDecal.activeInHierarchy)
            {
                _redDotDecal.SetActive(true);
            }
        }
        else
        {
            _redDotDecal.SetActive(false);
        }
    }
}

