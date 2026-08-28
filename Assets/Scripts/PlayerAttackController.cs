using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private bool _aaaaaaaattack = false;

    private float _maxRayLenght = 30f;

    [SerializeField] private Transform _shotPoint;

    [SerializeField] private int _plyDMG;
    public void OnShootPerformed(bool atck)
    {
        _aaaaaaaattack = atck;
        if (_aaaaaaaattack)
            ShotCommand();
    }

    private void ShotCommand()
    {
        if (_shotPoint == null)
            return;

        Ray shotRay = new Ray(_shotPoint.position, _shotPoint.forward);


       #if UNITY_EDITOR

        Debug.DrawRay(
           _shotPoint.position,
           shotRay.direction * _maxRayLenght,
           Color.red,
           10f
        );
        #endif
        if (Physics.Raycast(shotRay, out RaycastHit hitObj, _maxRayLenght))
        {
            //Debug.Log("hit");
            if(hitObj 
                     .collider
                     .gameObject
                     .TryGetComponent<EnemyController>(out var enemy))

            {
                enemy.TakeDamage(_plyDMG);
            }
        }
    }
}
