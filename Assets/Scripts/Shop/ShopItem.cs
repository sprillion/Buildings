using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    private ShopManager _shopManager;
    private UIManager _uiManager;

    [SerializeField] private GameObject _fill;

    public int id;

    public void Initialize(ShopManager shopManager, UIManager uiManager)
    {
        _shopManager = shopManager;
        _uiManager = uiManager;
    }

    public void SelectItem()
    {
        _shopManager.currentItem = id;
        _uiManager.SelectItem(_fill);
    }
}
