using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "Data", menuName = "Models/AIZombiModel")]
    public class AIZombieModelConfig : ScriptableObject
    {
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _minSqrDistanceToTarget;
        public float Speed => _speed;
        public float MinSqrDistanceToTarget=> _minSqrDistanceToTarget;
    }
}
