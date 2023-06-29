using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    uint _slotNum;
    public uint SlotNum => _slotNum;
    ItemCode _itemCode;
    public ItemCode ItemCode => _itemCode;
    uint _itemAmount;
    public uint ItemAmount => _itemAmount;

    public ItemSlot(uint slotNum)
    { 
        _slotNum = slotNum;
    }

    public void SlotSetting(ItemCode itemCode, uint itemAmount, bool isSlotChange = false)
    {
        if (_itemAmount != itemAmount && _itemAmount == 0 && isSlotChange)
        {
            ItemIcon icon = SpawnManager_Etc.Instance.GetObject_ItemIcon();
            GameManager.Instance.InvenUI.ItemSlotUI[SlotNum].SetChild(icon);
            icon.SetParent(GameManager.Instance.InvenUI.ItemSlotUI[SlotNum], true);
            icon.IconSetting(itemCode, itemAmount, true);
        }
        if (itemAmount != 0)
        {
            _itemCode = itemCode;
            _itemAmount = itemAmount;
        }
        else
        {
            _itemCode = ItemCode.None;
            _itemAmount = 0;
        }
        if (isSlotChange)
        {
            GameManager.Instance.InvenUI.ItemSlotUI[SlotNum].MyItemRefresh();
        }
    }
}
