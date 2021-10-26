using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private GameObject _cell;

    private GameManager _gameManager;
    private BuildingManager _buildingManager;
    private UIManager _uiManager;

    private List<Image> _imagesOnCells = new List<Image>();

    public void Initialize(GameManager gameManager, BuildingManager buildingManager, UIManager uiManager)
    {
        _gameManager = gameManager;
        _buildingManager = buildingManager;
        _uiManager = uiManager;
    }

    private void Start()
    {
        FillCells();
        _uiManager.ShowMiniMap(false);
    }

    private void FillCells()
    {
        var rect = GetComponent<RectTransform>().rect;
        var group = GetComponent<GridLayoutGroup>();

        group.cellSize = new Vector2(rect.width/_gameManager.SizeMap, rect.height / _gameManager.SizeMap);
        group.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        group.constraintCount = _gameManager.SizeMap;
        for (int i = 0; i < Mathf.Pow(_gameManager.SizeMap, 2); i++)
        {
            var obj = Instantiate(_cell, transform.position, Quaternion.identity, transform);
            obj.GetComponent<Cell>().id = i;
            obj.GetComponent<Cell>().Initialize(_buildingManager, this);
            _imagesOnCells.Add(obj.GetComponentsInChildren<Image>()[1]);

        }
    }

    public void FillImage()
    {
        foreach (var id in _buildingManager.cellWithBuilding.Keys)
        {
            var spriteObj = _buildingManager.cellWithBuilding[id];
            var spriteId = spriteObj.GetComponent<OnBuilding>().id;
            _imagesOnCells[id].sprite = _buildingManager.buildingsList[spriteId].sprite;
            _imagesOnCells[id].color = new Color(1, 1, 1, 1);
        }
    }

    public void ClearImages()
    {
        foreach (var image in _imagesOnCells)
        {
            image.sprite = null;
            image.color = new Color(1, 1, 1, 0);
        }
    }

    public void InfoFromMiniMap(int id)
    {
        if (!_buildingManager.cellWithBuilding.ContainsKey(id)) return;
        var onBuilding = _buildingManager.cellWithBuilding[id].GetComponent<OnBuilding>();
        if (!onBuilding.interactive) return;
        _uiManager.ShowInfo(true);
        _uiManager.FillInfo(_buildingManager.buildingsList[onBuilding.id], onBuilding);
        _uiManager.ShowMiniMap(false);
    }
}
