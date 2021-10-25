using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private GameObject _miniMapPanel;
    [SerializeField] private GameObject _openMiniMap;
    [SerializeField] private GameObject _levelUpButton;
    [SerializeField] private GameObject _clearPanel;
    [SerializeField] private Text _infoText;
    [SerializeField] private Text _infoStats;
    [SerializeField] private Text _infoName;
    [SerializeField] private Text _moneyText;
    [SerializeField] private Text _moneyPerTimeText;
    [SerializeField] private Image _infoImage;
    [SerializeField] private Image _fillMoneyImage;

    public bool UiOpen()
    {
        return _shopPanel.activeSelf || _infoPanel.activeSelf || _miniMapPanel.activeSelf || _clearPanel.activeSelf;
    }

    public void ShowShop(bool value)
    {
        _shopPanel.SetActive(value);
        RefreshFillItem();
    }

    public void ShowInfo(bool value)
    {
        _infoPanel.SetActive(value);
    }

    public void ShowMiniMap(bool value)
    {
        if (UiOpen() && value) return;
        _miniMapPanel.SetActive(value);
    }

    public void ShowClearMessage(bool value)
    {
        if (UiOpen() && value) return;
        _clearPanel.SetActive(value);
    }

    public void ShowMoney(int value)
    {
        _moneyText.text = value.ToString();
    }

    public void FillMoney(float value)
    {
        _fillMoneyImage.fillAmount = value;
    }

    public void FillMoneyPerTime(int value)
    {
        _moneyPerTimeText.text = "+" + value;
    }

    public void FillInfo(Building building, OnBuilding onBuilding)
    {
        _infoImage.sprite = building.sprite;
        _infoName.text = building.name;
        _infoStats.text = "Level  -  " + onBuilding.level + " \nIncome  -  " + onBuilding.level * building.income;
        _infoText.text = building.info;
        _levelUpButton.SetActive(onBuilding.level < onBuilding.maxLevel);
    }
    public void ChangeLevel(GameObject building)
    {
        building.GetComponentInChildren<TextMeshPro>().text = building.GetComponent<OnBuilding>().level.ToString();
    }

    private GameObject _currentFill;
    public void SelectItem(GameObject fill)
    {
        fill.SetActive(true);
        if (_currentFill == fill) return;
        RefreshFillItem();
        _currentFill = fill;
    }

    public void RefreshFillItem()
    {
        if (_currentFill != null)
            _currentFill.SetActive(false);
    }

}
