using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using static TowerSO;

public class BuildingPlayer : MonoBehaviour
{
    public static event Action GameOver;

    [SerializeField] private Camera _camera;

    [SerializeField] private LayerMask _layerMask;

    private Tower _selectedObject;

    [SerializeField] private Tower _previewTower;

    [SerializeField] private TowerCatalog _towerCatalog;

    [SerializeField] private Transform _MainTowerPoint;
    
    private TowerType _selectedTowerType = TowerType.None;

    private bool _IsGameOver = false;


    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        PlayerInput._onPlays -= onPlaysTower;

        if (UITracker.Instance != null)
            UITracker.Instance.OnTowerSelected -= SwitchTower;
    }

    private void SwitchTower(TowerType towertype)
    {
        if (_towerCatalog == null)
        {
            Debug.LogError("Assign a TowerCatalog to BuildingPlayer.", this);
            return;
        }

        _selectedTowerType = towertype;

        _selectedObject = _towerCatalog.GetTowerPrefab(towertype);

        if (_selectedObject == null)
        {
            Debug.LogError($"No prefab configured for {towertype} in TowerCatalog.", this);
            return;
        }

        if (_previewTower != null)
        {
          Destroy(_previewTower.gameObject);  
        }
        _previewTower = Instantiate(_selectedObject);
        _previewTower.enabled = false;

       //Debug.Log("sasasa");
    }

    private void Update()
    {
        if (_IsGameOver)
            return;

        UpdatePreviewTower();

        if (_selectedTowerType != TowerType.None &&
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            onPlaysTower();
        }
    }

    private void Start()
    {
        if (_towerCatalog != null)
            InitializeMainTower();
        else
            Debug.LogError("Assign a TowerCatalog to BuildingPlayer.", this);

        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        PlayerInput._onPlays -= onPlaysTower;
        PlayerInput._onPlays += onPlaysTower;

        if (UITracker.Instance != null)
        {
            UITracker.Instance.OnTowerSelected -= SwitchTower;
            UITracker.Instance.OnTowerSelected += SwitchTower;
        }
    }


    private void InitializePreviewTower()
    {
        if (_selectedObject != null)
        {
            _previewTower = Instantiate(_selectedObject);
            _previewTower.enabled = false;
        }
    }

    private void UpdatePreviewTower()
    {
        if (_previewTower == null || _camera == null)
            return;

        if (Mouse.current == null)
            return;

        Vector2 pointerPosition = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(pointerPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask))
        {
            _previewTower.transform.position = hit.point;
        }
        
    }
    private void onPlaysTower()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (_previewTower != null && _selectedObject != null)
        {
            if (_IsGameOver) 
            {
                return;
            }
            var placedTower = Instantiate(_selectedObject, _previewTower.transform.position, Quaternion.identity);

            var towerStats = _towerCatalog.GetTowerStats(_selectedTowerType);

            placedTower.InitialiseTower(towerStats);
        }
    }

    public void SelectCannonTower() => SwitchTower(TowerType.TowerCannon);

    public void SelectCatapultTower() => SwitchTower(TowerType.TowerCatapult);

    public void SelectMortarTower() => SwitchTower(TowerType.TowerMortar);

    private void InitializeMainTower()
    {
        var mainTowerPrefab = _towerCatalog.GetTowerPrefab(TowerType.MainTower);
        if (mainTowerPrefab == null || _MainTowerPoint == null)
        {
            Debug.LogError("Main tower prefab or spawn point is not assigned.", this);
            return;
        }

        var mainTower = Instantiate(mainTowerPrefab, _MainTowerPoint.position, Quaternion.identity);

        var towerStats = _towerCatalog.GetTowerStats(TowerType.MainTower);

        mainTower.InitialiseTower(towerStats);

        mainTower.MakeTowerMain(this);
    }

    public void OnGameOver()
    {
        if (_IsGameOver)
            return;

        _IsGameOver = true;
        GameOver?.Invoke();
    }
}
