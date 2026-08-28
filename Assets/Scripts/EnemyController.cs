using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    private int _enemyHP = 10;

    private float _mySpeed;

    private Tower _target;

    private EnemyState _myState = EnemyState.Idle;

    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private int _health = 100;

    [SerializeField] private float _attackRange = 2f;

    private float _searchTime = 1f;

    private float _nextScan = 0f;

    private float _patrolRadius = 10f;

    private bool _isWaiting;

    private Vector3 _currentDestination;

    private float _AttackTimer = 2f;

    private float _AttackTime = 0f;

    private float _lookSpeed;

    private float _searchradious;

    private int _attackDamage;

    public bool IsDead => _health <= 0;

    private void OnEnable()
    {
        BuildingPlayer.GameOver += StopOnGameOver;
    }

    private void OnDisable()
    {
        BuildingPlayer.GameOver -= StopOnGameOver;
    }

    private void StopOnGameOver()
    {
        _target = null;
        _myState = EnemyState.Idle;

        if (_agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh)
            _agent.ResetPath();

        enabled = false;
    }


    //public void Initialize(EnemySO stats)
    //{
    //    _health = stats.HP;

    //    _mySpeed = stats.Speed;

    //    _attackDamage = stats.AttackDamage;

    //    _myState = EnemyState.Idle;

    //    _agent.speed = _mySpeed;
    //}
    public void Initialize(EnemySO stats)
    {
        if (stats == null) return;
        _health = stats.HP;
        _mySpeed = stats.Speed;
        _attackDamage = stats.AttackDamage;
        _myState = EnemyState.Idle;
        if (_agent != null) _agent.speed = _mySpeed;
    }
    private void Chase()
    {
        if (_agent == null || !_agent.isActiveAndEnabled || !_agent.isOnNavMesh || _target == null)
            return;

        _agent.SetDestination(_target.transform.position);

        if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
        {
            _agent.ResetPath(); 

            _myState = EnemyState.Attack;
        }
        else
        {
            _myState = EnemyState.Chase;
        }

    }
    private void Attack()
    {
        if (_target != null && _target.IsDead == false)
        {
            _target.TakeDamage(_attackDamage);
        }
    }

    private void Update()
    {
        _nextScan += Time.deltaTime;

        if (_nextScan > _searchTime && _target == null)
        {
            ScanForPlayer();

            _nextScan = 0f;
        }


        switch (_myState)
        {
            case EnemyState.Chase:
                Chase();
                LookAtTarget();
                break;  



            case EnemyState.Attack:
                if (_target == null || _target.IsDead)
                {
                    _target = null;
                    _myState = EnemyState.Idle;
                    break;
                }

                _AttackTime += Time.deltaTime;
                if (_AttackTime >= _AttackTimer)
                {
                    _AttackTime = 0f;
                    Attack();
                }
                LookAtTarget();
                break;

        }
    }

    //private void LookAtTarget()
    //{
    //    Vector3 direction = (_target.transform.position = transform.position);

    //    direction.y = 0;

    //    if(direction != Vector3.zero)
    //    {
    //        Quaternion lookRotation = Quaternion.LookRotation(direction);

    //        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _lookSpeed * Time.deltaTime);
    //    }
    //}
    private void LookAtTarget()
    {
        if (_target == null) return;

        Vector3 direction = _target.transform.position - transform.position;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _lookSpeed * Time.deltaTime);
        }
    }


    //public void ScanForPlayer()
    //{
    //    int searchLayer = 0;
    //    Collider[] hits = Physics.OverlapSphere(transform.position, _searchradious, searchLayer);

    //    if(hits.Length > 0 )
    //    {
    //        _target = hits[0].GetComponent<Tower>();
    //    }
    //    if (_target.Type == TowerType.None || _target.IsDead)
    //    {
    //        _myState = EnemyState.Idle;
    //        return;
    //    }
    //    else
    //    {
    //        _target = null;


    //    }
    //}
    [SerializeField] private LayerMask _targetLayerMask; // add near other serialized fields
    [SerializeField] private float _searchRadius = 10f;   // ensure a default

    public void ScanForPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _searchRadius, _targetLayerMask);
        if (hits.Length == 0)
        {
            _target = null;
            _myState = EnemyState.Idle;
            return;
        }

        _target = null;
        float closestDistance = float.PositiveInfinity;
        foreach (Collider hit in hits)
        {
            if (!hit.TryGetComponent(out Tower tower) || tower.Type == TowerType.None || tower.IsDead)
                continue;

            float distance = (tower.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                _target = tower;
            }
        }

        if (_target == null)
        {
            _myState = EnemyState.Idle;
            _target = null;
            return;
        }

        // otherwise the target is valid, switch state if needed
        _myState = EnemyState.Chase;
    }
    //public void TakeDamage(int dmg)
    //{
    //    if (_health <= 0)
    //    {
    //        Die();
    //    }
    //    if (_enemyHP >= 0)
    //    {
    //        _enemyHP -= dmg;
    //    }
    //}
    public void TakeDamage(int dmg)
    {
        _health -= dmg;
        if (_health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        _myState = EnemyState.Dead;

        this.gameObject.SetActive(false);
    }

    public enum EnemyState
    {
        Idle,

        Chase,

        Attack,

        Dead,
    }
}

