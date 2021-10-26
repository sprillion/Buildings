using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    private BuildingManager _buildingManager;
    private MiniMap _miniMap;
    public int id;
    public void Initialize(BuildingManager buildingManager, MiniMap miniMap)
    {
        _buildingManager = buildingManager;
        _miniMap = miniMap;
    }


    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            _buildingManager.SelectCell(id);
    }

    public void GoToInfo()
    {
        _miniMap.InfoFromMiniMap(id);
    }
}
