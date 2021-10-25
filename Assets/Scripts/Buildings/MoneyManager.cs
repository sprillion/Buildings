using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private BuildingManager _buildingManager;
    private UIManager _uiManager;

    public const int StartMoney = 3;

    public int money;
    public float time;

    public void Initialize(BuildingManager buildingManager, UIManager uiManager)
    {
        _buildingManager = buildingManager;
        _uiManager = uiManager;
        Load();
        StartCoroutine(PlusMoney());
        StartCoroutine(FillMoney());
    }

    public int MoneyPerTime()
    {
        var sum = 0;
        foreach (var obj in _buildingManager.cellWithBuilding.Values)
        {
            var level = obj.GetComponent<OnBuilding>().level;
            var income = _buildingManager.buildingsList[obj.GetComponent<OnBuilding>().id].income;
            sum += level * income;
        }
        _uiManager.FillMoneyPerTime(sum);
        return sum;
    }

    private IEnumerator PlusMoney()
    {
        while (true)
        {
            _uiManager.FillMoneyPerTime(MoneyPerTime());
            money += MoneyPerTime();
            _uiManager.ShowMoney(money);
            Save();
            yield return new WaitForSeconds(time);
        }
    }
    private IEnumerator FillMoney()
    {
        var timeLeft = 0f;
        while (true)
        {
            timeLeft += Time.deltaTime;
            if (timeLeft >= time)
                timeLeft = 0;
            _uiManager.FillMoney(timeLeft / time);
            yield return null;
        }
    }

    public bool ChangeMoney(int value)
    {
        if (value > money) return false;
        money -= value;
        _uiManager.ShowMoney(money);
        Save();
        return true;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("Money", money);
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetInt("Money");
        }
    }

    public void Refresh()
    {
        PlayerPrefs.DeleteKey("Money");
        money = StartMoney;
        _uiManager.ShowMoney(money);
    }
    public int MoneyForLevelUp(int level)
    {
        return (int)Mathf.Pow(level + 1, 2);
    }
}
