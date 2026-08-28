using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName =  "Projecttile50", menuName = "ProjectileData", order = 1)]
public class Project : ScriptableObject
{
    [SerializeField] private float _projSpeed;

    [SerializeField] private GameObject _model;

    public float Speed => _projSpeed;

    public GameObject Model => _model;

}
