
using UnityEngine;


[CreateAssetMenu(fileName = "TowerSO", menuName = "Create tower", order = 3)]
public class TowerSO : ScriptableObject
{
    [SerializeField] private TowerType _towerType;

    [SerializeField] private int _towerHP;

    [SerializeField] private float _towerSpeed;

    [SerializeField] private int _towerDamage;

    [SerializeField] private int _cost;

    

    public int Cost => _cost;

    public int Damage => _towerDamage;


    public TowerType Type => _towerType;

    public int HP => _towerHP;

    public float Speed()
    {
        return _towerSpeed;
    }


}   
    public enum TowerType
    {
        None,

        TowerCannon,

        TowerCatapult,

        TowerMortar,

        MainTower,
    }

