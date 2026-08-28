using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCatalog", menuName = "CreateEnemyCatalog", order = 2)]
public class EnemyCatalog : ScriptableObject
{
    [SerializeField] private List<EnemySO> _enemyList;

    [SerializeField] private GameObject _enemyPrefab;

    public EnemyController GetEnemyPrefab
    {
        get
        {
            if (_enemyPrefab == null)
                return null;

            return _enemyPrefab.GetComponent<EnemyController>();
        }
    }

    public EnemySO GetEnemyStats(int index)
    {
        if (_enemyList == null || _enemyList.Count == 0)
            return null;

        if (index < 0 || index >= _enemyList.Count)
            index = 0;

        return _enemyList[index];
    }
}
