using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerCatalog", menuName = "CreateTowerCatalog", order = 4)]
public class TowerCatalog : ScriptableObject
{
    [SerializeField] private List<TowerSO> _towerList;

    [SerializeField] private EnemyController _enemyPrefab;

    [SerializeField] private List<GameObject> _towerPrefabs;

    public EnemyController GetEnemyPrefab => _enemyPrefab;
    public Tower GetTowerPrefab(TowerType type)
    {
        if (_towerPrefabs == null)
            return null;

        foreach (GameObject prefab in _towerPrefabs)
        {
            if (prefab == null || !prefab.TryGetComponent(out Tower tower))
                continue;

            if (tower.Type == type)
                return tower;
        }

        return null;
    }
    public TowerSO GetEnemyStats(TowerType type)
    {
        TowerSO tower = _towerList[0];

        foreach(TowerSO t in _towerList) 
        {
            if(t.Type == type)
            {
                tower = t;

                break;
            }   
        }
        return tower;
    }

    internal TowerSO GetTowerStats(TowerType towerCannon)
    {
        if (_towerList == null || _towerList.Count == 0)
            return null;

        TowerSO tower = _towerList[0];

        foreach (TowerSO t in _towerList)
        {
            if (t.Type == towerCannon)
            {
                tower = t;
                break;
            }
        }
        return tower;
    }
}
