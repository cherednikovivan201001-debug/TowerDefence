//using UnityEngine;

//public class ProjectTie : MonoBehaviour
//{
//    private float _projecttileSpeed = 4f;


//    [SerializeField] private int _indexInList;

//    private void Start()
//    {
//        var projData = projData.ProjectilleData(_indexInList);

//        _projecttileSpeed = projData.Speed;

//        Instantiate(projData.Model, transform);
//    }
//    private void FixedUpdate()
//    {
//        transform.Translate(Vector3.forward * _projecttileSpeed * Time.fixedDeltaTime);
//    }
//}
