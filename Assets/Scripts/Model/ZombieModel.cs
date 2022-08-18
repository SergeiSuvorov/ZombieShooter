using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "ZombieModel", menuName = "Models/ZombieModel")]
    public class ZombieModel  : ScriptableObject
    {
        [SerializeField]
        private int _health;
        [SerializeField]
        private int _damage;
        [SerializeField]
        private float _coolDawnTime;
        public int Health => _health;
        public int Damage => _damage;

        public float CoolDawnTime => _coolDawnTime;
    }
}
