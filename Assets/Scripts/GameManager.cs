using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public ShopManager shopManager;
    public BuildingManager buildingManager;
    public CameraManager cameraManager;
    public UIManager uiManager;
    public MiniMap miniMap;

    [Header("Map *x*")]
    [Range(3, 15)]
        public int SizeMap = 5;

    private void Awake()
    {
        miniMap.Initialize(this, buildingManager, uiManager);
        shopManager.Initialize(buildingManager, uiManager);
        buildingManager.Initialize(this, uiManager, shopManager, miniMap);
        cameraManager.Initialize(this, uiManager);
    }

    private void Start()
    {
        LoadAll();
    }

    public void ClearAll()
    {
        buildingManager.ClearBuildings();
        miniMap.ClearImages();
    }

    public void SaveAll()
    {
        buildingManager.Save();
    }

    public void LoadAll()
    {
        buildingManager.Load();
    }

}
