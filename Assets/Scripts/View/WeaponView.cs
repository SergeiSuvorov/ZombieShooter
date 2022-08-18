using UnityEngine;

public class WeaponView : MonoBehaviour
{
    [SerializeField]
    private GameObject _redDotDecal;

    [SerializeField]
    private Transform _visualEffectRoot;


    private void Awake()
    {
        _redDotDecal.SetActive(false);
        _visualEffectRoot.gameObject.SetActive(false);
    }

    public void Action( int damage)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            var damageReceiver = hit.transform.GetComponent<IDamageReceiver>();
            if (damageReceiver != null)
                damageReceiver.GetDamage(damage);
        }
    }
    public void SetActiveVisualEffect(bool setActive)
    {
        _visualEffectRoot.gameObject.SetActive(setActive);
    }

    public void RedDotAiming()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
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

