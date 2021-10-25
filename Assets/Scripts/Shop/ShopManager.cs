using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private BuildingManager _buildingManager;
    private UIManager _uiManager;

    [SerializeField] private Transform _interactiveContent;
    [SerializeField] private Transform _decorativeContent;
    [SerializeField] private GameObject _prefabItemShop;

    [HideInInspector] public int currentItem = NullItem;

    private const int NullItem = -1;
    public void Initialize(BuildingManager buildingManager, UIManager uiManager)
    {
        _buildingManager = buildingManager;
        _uiManager = uiManager;
    }

    private void Start()
    {
        FillShop();
    }

    private void FillShop()
    {
        var i = 0;
        foreach (var building in _buildingManager.buildingsList)
        {
            var item = Instantiate(_prefabItemShop, Vector3.zero, Quaternion.identity,
                building.interactive ? _interactiveContent : _decorativeContent);

            item.GetComponent<Image>().sprite = building.sprite;
            var texts = item.GetComponentsInChildren<Text>();
            texts[0].text = building.name;
            texts[1].text = building.price.ToString();

            var shopItem = item.GetComponent<ShopItem>();
            shopItem.Initialize(this, _uiManager);
            shopItem.id = i;
            i++;
        }
    }

    public void Refresh()
    {
        currentItem = NullItem;
    }

    public bool ItemIsNull()
    {
        return currentItem == NullItem;
    }
}
