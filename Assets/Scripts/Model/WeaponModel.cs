using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "WeaponModel", menuName = "Models/WeaponModel")]
    public class WeaponModel : ScriptableObject
    {
        [SerializeField]
        private int _weaponID;
        [SerializeField]
        private int _ammoClipSize;
        [SerializeField]
        private int _damage;
        [SerializeField]
        private float _shootDelayTime;
        [SerializeField]
        private float _reloadTime;

        public int Damage => _damage;
        public int AmmoClipSize => _ammoClipSize;
        public float ShootDelayTime => _shootDelayTime;
        public float ReloadTime => _reloadTime;

    }
}
