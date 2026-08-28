using UnityEngine;
//using static EnemySO;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Create enemy", order = 2)]

public class EnemySO : ScriptableObject
{
    //[SerializeField] private int EnemyHP;

    //[SerializeField] private EnemyType _enemyType;

    //[SerializeField] private float _enemySpeed;

    //[SerializeField] private int _damage;

    //[SerializeField] private int _attackDamage;

    //public int AttackDamage => _attackDamage;

    //public int HP => EnemyHP;

    //public float Speed => _enemySpeed;

    //public EnemyType Type => _enemyType;

    [SerializeField] private int _hp;
    [SerializeField] private float _speed;
    [SerializeField] private int _attackDamage;
    [SerializeField] private EnemyType _enemyType;

    public int HP => _hp;
    public float Speed => _speed;
    public int AttackDamage => _attackDamage;
    public EnemyType Type => _enemyType;

    public enum EnemyType
    {
        Ork = 0,
        Dragon = 1,
        Mage = 2,
    }
}
