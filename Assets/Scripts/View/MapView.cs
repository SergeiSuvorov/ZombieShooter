using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    [SerializeField] List<Transform> _playerSpawnPoints;
    [SerializeField] List<Transform> _enemySpawnPoints;

    public List<Transform> PlayerSpawnPoints => _playerSpawnPoints;
    public List <Transform> EnemySpawnPoints => _enemySpawnPoints;
}

