using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyCatalog _enemyCatalog;

    [SerializeField] private int _maxEnemyCount;

    [SerializeField] List<Transform> _spawnPoints;

    [SerializeField] private AppleController controller;

    [SerializeField] private Transform _spawnPointsContainer;

    private List<int> _usedSlots = new List<int>();

    private void OnEnable()
    {
        FindSpawnPoint();
        Debug.Log($"EnemySpawner: Found {_spawnPoints?.Count ?? 0} spawn points; EnemyCatalog assigned: {_enemyCatalog != null}", this);
        SpawnEnemies();
    }

    //private void SpawnEnemies()
    //{
    //    for (int i = 0; i < _maxEnemyCount; i++)
    //    {
    //        int index = GetSlotForEnemy();

    //        var Enemy = _enemyCatalog.GetEnemyPrefab;



    //        var EnemyObj = Instantiate(Enemy, _spawnPoints[index].position, Quaternion.identity, _spawnPoints[index]);

    //        EnemyObj.Initialize(_enemyCatalog.GetEnemyStats(0));


    //    }

    //}
    private void SpawnEnemies()
    {
        if (_enemyCatalog == null)
        {
            Debug.LogWarning("EnemySpawner: _enemyCatalog is null", this);
            return;
        }

        if (_spawnPoints == null || _spawnPoints.Count == 0)
        {
            FindSpawnPoint();
            if (_spawnPoints == null || _spawnPoints.Count == 0)
            {
                Debug.LogWarning("EnemySpawner: no spawn points found", this);
                return;
            }
        }

        _usedSlots.Clear();
        int spawnCount = Mathf.Min(_maxEnemyCount, _spawnPoints.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            int index = GetSlotForEnemy();
            if (index < 0 || index >= _spawnPoints.Count) continue;

            var enemyPrefab = _enemyCatalog.GetEnemyPrefab;
            if (enemyPrefab == null) { Debug.LogWarning("EnemySpawner: enemy prefab null"); continue; }

            Vector3 spawnPosition = _spawnPoints[index].position;
            if (!NavMesh.SamplePosition(spawnPosition, out NavMeshHit navHit, 10f, NavMesh.AllAreas))
            {
                Debug.LogWarning($"EnemySpawner: no NavMesh near spawn point {_spawnPoints[index].name}", this);
                continue;
            }

            var enemyObj = Instantiate(enemyPrefab, navHit.position, Quaternion.identity, _spawnPoints[index]);
            enemyObj.Initialize(_enemyCatalog.GetEnemyStats(0));
        }
    }

    //private int GetSlotForEnemy()
    //{
    //    int slot = 0;

    //    for (int i = 0; i < _spawnPoints.Count; i++)
    //    {
    //        int randomSlot = Random.Range(0, _spawnPoints.Count);

    //        if (!_usedSlots.Contains(randomSlot))
    //        {
    //            slot = randomSlot;
    //            _usedSlots.Add(slot);
    //            return slot;
    //        }

    //    }
    //    return slot;
    //}
    private int GetSlotForEnemy()
    {
        if (_spawnPoints == null || _spawnPoints.Count == 0) return -1;

        var available = new List<int>();
        for (int i = 0; i < _spawnPoints.Count; i++)
            if (!_usedSlots.Contains(i)) available.Add(i);

        if (available.Count == 0) return -1;

        int pick = available[Random.Range(0, available.Count)];
        _usedSlots.Add(pick);
        return pick;
    }

    public void SpawnPoints()
    {

    }
    //public void FindSpawnPoint()
    //{
    //    //_spawnPoints.Clear();

    //    if (_spawnPoints == null) _spawnPoints = new List<Transform>();
    //    _spawnPoints.Clear();

    //    if (_spawnPointsContainer == null)
    //    {
    //        Debug.LogWarning("EnemySpawner: _spawnPointsContainer is null", this);
    //        return;
    //    }

    //    foreach (Transform child in _spawnPointsContainer)
    //    {
    //        _spawnPoints.Add(child);
    //    }
    //}
    public void FindSpawnPoint()
    {
        if (_spawnPoints == null) _spawnPoints = new List<Transform>();
        _spawnPoints.Clear();

        if (_spawnPointsContainer == null)
        {
            Debug.LogWarning("EnemySpawner: _spawnPointsContainer is null", this);
            return;
        }

        foreach (Transform child in _spawnPointsContainer)
        {
            if (child == null) continue;
            if (!child.gameObject.activeInHierarchy) continue;
            _spawnPoints.Add(child);
        }

        Debug.Log($"EnemySpawner: populated {_spawnPoints.Count} spawn points", this);
    }
}
#if UNITY_EDITOR

    [CustomEditor(typeof(EnemySpawner))]
    public class EnemySpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawDefaultInspector();

            EnemySpawner spawner = (EnemySpawner)target;

            if (GUILayout.Button("Find spawn points", GUILayout.Height(30)))
            {
                spawner.FindSpawnPoint();
            }
        }
    }

   



#endif
