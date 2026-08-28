//using System.Runtime.CompilerServices;
using UnityEngine;
//using static UnityEngine.InputSystem.InputAction;

public class Tower : MonoBehaviour
{
    [SerializeField] private float _range = 10f;

    [SerializeField] private int  _damage = 1;

    [SerializeField] private float _searchTime = 1f;

    [SerializeField] private Transform _shootPoint;

    [SerializeField] private float _rotationSpeed = 3f;

    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private int health = 1;

    [SerializeField] private TowerData[] _towerData;

    [SerializeField] private ParticleSystem _shootefect;

    [SerializeField] private ParticleSystem _deathEffect;

    private TowerType _towerType;

    public TowerType Type;

    private BuildingPlayer _buildingPlayer;

    private bool _IsDead = false;

    public bool IsDead  => _IsDead;

    private bool _IsUsed;

    private EnemyController _target;

    private float _searchTimer;

    private float _nextFireTime = 0f;

    private float _kdFireTime = 2f;
    private void Update()
    {
        if (_IsDead) 
        return; 
        _searchTimer += Time.deltaTime;
        if(_searchTimer >= _searchTime)
        { 
            FindTarget();

            _searchTimer = 0f;
        }
        RotateTurret();

        _nextFireTime += Time.deltaTime;

        if (_nextFireTime >= _kdFireTime)
        {
            if (_target != null)
            {
                _target.TakeDamage(_damage);
            }
            _nextFireTime = 0f;
        }
    }

    public void InitialiseTower(TowerSO towerSO)
    {
        if (towerSO == null)
        {
            Debug.LogError("Tower stats are not assigned.", this);
            return;
        }

        _range = towerSO.Speed();

        _damage = towerSO.Damage;

        health = towerSO.HP;

        _towerType = towerSO.Type;

        Type = _towerType;

        SetModel();
    }

    public void MakeTowerMain(BuildingPlayer buildingPlayer)
    {
        _buildingPlayer = buildingPlayer;
    }

    private void SetModel()
    {
        foreach (var tower in _towerData)
        {
            tower.towerModel.SetActive(false);
            if (tower.towerType == _towerType)
            {
                tower.towerModel.SetActive(true);
            }
        }
    }


    private void RotateTurret()
    {
        if (_target == null)
        {
            return;
        }

        Vector3 direction = _target.transform.position - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _range * Time.deltaTime); 

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, _range);
    }

    private void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _range, _layerMask);

        _target = null;

        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (!collider.TryGetComponent(out EnemyController enemy) || enemy.IsDead)
                continue;

            float distance = (enemy.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                _target = enemy;
            }
        }

    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        if (_IsDead)
            return;

        _IsDead = true;

        if (_buildingPlayer != null)
        {   
            _buildingPlayer.OnGameOver();
        }
        if (_deathEffect != null)
        {
            _deathEffect.Play();
        }

        _target = null;

        foreach (Collider towerCollider in GetComponentsInChildren<Collider>())
            towerCollider.enabled = false;

        gameObject.SetActive(false);
    }
}
[System.Serializable]

public struct TowerData
{
    public TowerType towerType;

    public GameObject towerModel;


}
