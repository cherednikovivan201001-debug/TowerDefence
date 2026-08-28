using System;
using UnityEngine;
using UnityEngine.UI;

public class UITracker : MonoBehaviour
{
    public static UITracker Instance { get; private set; }

    public event Action<TowerType> OnTowerSelected;

    [SerializeField] private Button TowerButton1;

    [SerializeField] private Button TowerButton2;

    [SerializeField] private Button TowerButton3;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        TowerButton1.onClick.AddListener(CallTowerCanon);
        TowerButton2.onClick.AddListener(CallTowerTurret);
        TowerButton3.onClick.AddListener(CallTowerMortar);
    }
    private void OnDestroy()
    {
        TowerButton1.onClick.RemoveListener(CallTowerCanon);
        TowerButton2.onClick.RemoveListener(CallTowerTurret);
        TowerButton3.onClick.RemoveListener(CallTowerMortar);
    }

    public void CallTowerCanon()
    {
        OnTowerSelected?.Invoke(TowerType.TowerCannon);
    }

    public void CallTowerTurret()
    {
        OnTowerSelected?.Invoke(TowerType.TowerCatapult);
    }

    public void CallTowerMortar()
    {
        OnTowerSelected?.Invoke(TowerType.TowerMortar);
    }



}
