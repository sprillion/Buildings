using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private GameManager _gameManager;
    private UIManager _uiManager;
    private CameraManager _cameraManager;
    private ShopManager _shopManager;
    private MiniMap _miniMap;
    public MoneyManager moneyManager;
    public GenerationGround generationGround;


    [HideInInspector] public List<Cell> cells = new List<Cell>();
    [HideInInspector] public Dictionary<int, GameObject> cellWithBuilding = new Dictionary<int, GameObject>();

    [SerializeField] private GameObject _selectedCell;

    public int currentCellID;
    public Building[] buildingsList;

    public void Initialize(GameManager gameManager, UIManager uiManager, ShopManager shopManager, MiniMap miniMap)
    {
        _gameManager = gameManager;
        _uiManager = uiManager;
        _shopManager = shopManager;
        _miniMap = miniMap;
        generationGround.Initialize(_gameManager, this, _miniMap);
        moneyManager.Initialize(this, uiManager);
    }

    public bool CellContainsBuilding => cellWithBuilding.ContainsKey(currentCellID);
    public OnBuilding CurrentOnBuilding(int id) => cellWithBuilding[id].GetComponent<OnBuilding>();
    public GameObject CurrentBuilding => cellWithBuilding[currentCellID];


    public void SelectCell(int id)
    {
        if (_uiManager.UiOpen()) return;
        currentCellID = id;
        _selectedCell.transform.position = cells[currentCellID].transform.position + new Vector3(0, 0.1f, 0);
        _uiManager.ShowShop(!CellContainsBuilding);
        if (!CellContainsBuilding || !CurrentOnBuilding(currentCellID).interactive) return;
        _uiManager.ShowInfo(true);
        _uiManager.FillInfo(buildingsList[CurrentOnBuilding(currentCellID).id], CurrentOnBuilding(currentCellID));
    }

    public void SetBuilding(bool isLoad)
    {
        if (_shopManager.ItemIsNull()) return;
        if (!isLoad)
        {
            if (!moneyManager.ChangeMoney(buildingsList[_shopManager.currentItem].price)) return;
        }
        var currentCell = cells[currentCellID].transform;
        cellWithBuilding[currentCellID] = Instantiate(buildingsList[_shopManager.currentItem].prefab, currentCell.position, new Quaternion(0, 180, 0, 0), currentCell);
        _shopManager.Refresh();
        if (CurrentOnBuilding(currentCellID).interactive)
            _uiManager.ChangeLevel(CurrentBuilding);
        _uiManager.ShowShop(false);
        Save();
        _uiManager.FillMoneyPerTime(moneyManager.MoneyPerTime());
    }

    public void LevelUpBuilding()
    {
        if (!moneyManager.ChangeMoney(moneyManager.MoneyForLevelUp(CurrentOnBuilding(currentCellID).level))) return;

        CurrentOnBuilding(currentCellID).level++;
        _uiManager.ChangeLevel(CurrentBuilding);
        _uiManager.ShowInfo(false);
        Save();
    }

    public void ClearBuildings()
    {
        foreach (var obj in cellWithBuilding.Values)
        {
            obj.SetActive(false);
        }
        cellWithBuilding.Clear();
        moneyManager.Refresh();
        PlayerPrefs.DeleteKey("Save");
    }

    public void Save()
    {
        var saveJson = new SaveJson {sizeMap = _gameManager.SizeMap};
        foreach (var key in cellWithBuilding.Keys)
        {
            saveJson.save.Add(new SaveBuildings(key, CurrentOnBuilding(key).id, CurrentOnBuilding(key).level));
        }

        PlayerPrefs.SetString("Save",saveJson.SaveToString());
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey("Save")) return;
        var json = PlayerPrefs.GetString("Save");
        var save = JsonUtility.FromJson<SaveJson>(json);
        _gameManager.SizeMap = save.sizeMap;
        foreach (var i in save.save)
        {
            currentCellID = i.idCell;
            _shopManager.currentItem = i.idBuilding;
            SetBuilding(true);
            if (!CurrentOnBuilding(currentCellID).interactive) continue;
            CurrentOnBuilding(currentCellID).level = i.level;
            _uiManager.ChangeLevel(CurrentBuilding);
        }
        _uiManager.FillMoneyPerTime(moneyManager.MoneyPerTime());
    }
}

[Serializable]
public class SaveJson
{
    public int sizeMap;
    public List<SaveBuildings> save = new List<SaveBuildings>();
    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}
[Serializable]
public class SaveBuildings
{
    public int idCell;
    public int idBuilding;
    public int level;

    public SaveBuildings(int idCell, int idBuilding, int level)
    {
        this.idCell = idCell;
        this.idBuilding = idBuilding;
        this.level = level;
    }

}


