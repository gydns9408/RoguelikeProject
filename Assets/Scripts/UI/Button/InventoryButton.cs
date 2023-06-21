using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : Button_Base
{
    ItemInventoryUI _itemInventoryUI;
    private void Start()
    {
        _itemInventoryUI = FindObjectOfType<ItemInventoryUI>();
    }

    protected override void ButtonClickEvent()
    {
        if (_itemInventoryUI.gameObject.activeSelf == false)
        {
            _itemInventoryUI.Open();
        }
        else
        {
            _itemInventoryUI.Close();
        }
    }
}
