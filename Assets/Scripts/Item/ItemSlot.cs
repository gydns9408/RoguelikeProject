using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    uint _slotNum;
    public uint SlotNum => _slotNum;
    ItemData _itemData;
    public ItemData ItemData => _itemData;
    uint _itemAmount;
    public uint ItemAmount => _itemAmount;

    public ItemSlot(uint slotNum)
    { 
        _slotNum = slotNum;
    }

    public void SlotSetting(ItemData itemData, uint itemAmount)
    {
        if (ItemData != itemData)
        { 
            _itemData = itemData;
        }
        if (itemAmount != 0)
        {
            if (ItemAmount != itemAmount)
            {
                _itemAmount = itemAmount;
            }
        }
        else
        {
            _itemData = null;
            _itemAmount = 0;
        }
    }
}
