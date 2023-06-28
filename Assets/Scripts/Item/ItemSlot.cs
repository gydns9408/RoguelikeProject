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
