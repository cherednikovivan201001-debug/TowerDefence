using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class AppleSpawner : MonoBehaviour
{
   [SerializeField] private Apple _applePrefab;

    [SerializeField] private int _maxAppleCount;

   [SerializeField] List<Transform> _spawnPoints;

    [SerializeField] private AppleController controller;

    private List<int> _usedSlots = new List<int>();
             
    private void OnEnable()
    {
        SpawnApples();
    }

    private void SpawnApples()
    {
        if (_applePrefab == null || controller == null || _spawnPoints == null || _spawnPoints.Count == 0)
        {
            Debug.LogWarning("AppleSpawner is not configured.", this);
            return;
        }

        _usedSlots.Clear();
        int spawnCount = Mathf.Min(_maxAppleCount, _spawnPoints.Count);
        for (int i = 0; i < spawnCount; i++)
        {
            int slot = GetSlotForApple();
            if (slot < 0)
                break;

            Apple apple = Instantiate(_applePrefab, _spawnPoints[slot].position, Quaternion.identity, _spawnPoints[slot]);
            apple.SetupApple(controller);
        }
    }
    
    private int GetSlotForApple()
    {
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            int randomSlot = Random.Range(0, _spawnPoints.Count);

            if (!_usedSlots.Contains(randomSlot))
            {
                _usedSlots.Add(randomSlot);
                return randomSlot;
            }
        
        }
        return -1;
    }   
}

