using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "Data", menuName = "Models/FollowCameraModel")]
    public class FollowCameraModel : ScriptableObject
    {
        [SerializeField]
        private float _distance;

        [SerializeField]
        private float _height;

        [SerializeField]
        private float _smoothSpeed;

        public float Distance => _distance;
        public float Height => _height;
        public float SmoothSpeed => _smoothSpeed;
    }
}
