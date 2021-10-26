using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GenerationGround : MonoBehaviour
{
    [SerializeField] private GameObject _prefabCell;
    [SerializeField] private int _sizeCell;

    private BuildingManager _buildingManager;
    private GameManager _gameManager;
    private MiniMap _miniMap;

    public void Initialize(GameManager gameManager, BuildingManager buildingManager, MiniMap miniMap)
    {
        _gameManager = gameManager;
        _buildingManager = buildingManager;
        _miniMap = miniMap;
    }

    public void Generation()
    {
        var sizeMap = _gameManager.SizeMap;
        var startPos = sizeMap * _sizeCell / -2 + _sizeCell/2;
        for (int i = 0; i < sizeMap; i++)
            for (int j = 0; j < sizeMap; j++)
            {
                var posCell = new Vector3(i * _sizeCell + startPos, 0, j * _sizeCell + startPos);
                _buildingManager.cells.Add(Instantiate(_prefabCell, posCell, Quaternion.identity, transform).GetComponent<Cell>());
                _buildingManager.cells[_buildingManager.cells.Count - 1].Initialize(_buildingManager, _miniMap);
                _buildingManager.cells[_buildingManager.cells.Count - 1].id = _buildingManager.cells.Count - 1;
            }
    }
}
