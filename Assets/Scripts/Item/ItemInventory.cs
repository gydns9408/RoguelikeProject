using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory
{
    ItemSlot[] _slots;
    public ItemSlot this[uint index] => _slots[index];
    public uint InventorySize => (uint)_slots.Length;

    Player _owner;
    public Player Owner => _owner;

    public ItemInventory(uint size, Player owner)
    {
        _slots = new ItemSlot[size];
        for (int i = 0; i < size; i++)
        {
            _slots[i] = new ItemSlot((uint)i);
        }
        _owner = owner;
    }

    public ItemSlot FindSameItemHaveSlot(ItemCode itemCode, uint startIndex = 0)
    {
        ItemSlot result = null;
        for (uint i = startIndex; i < _slots.Length; i++)
        {
            if (_slots[i].ItemCode == itemCode)
            {
                if (itemCode != ItemCode.None)
                {
                    if (_slots[i].ItemAmount < GameManager.Instance.ItemData[itemCode].maxAmount)
                    {
                        result = _slots[i];
                        break;
                    }
                }
                else
                {
                    result = _slots[i];
                    break;
                }

            }
        }
        return result;
    }

    public bool CheckIsPossibleAddItem(ItemCode itemCode, uint itemAmount, uint startIndex = 0)
    {
        bool result = false;
        ItemSlot targetSlot = FindSameItemHaveSlot(itemCode, startIndex);
        if (targetSlot != null)
        {
            int remainSpace = (int)(GameManager.Instance.ItemData[itemCode].maxAmount - targetSlot.ItemAmount);
            if (itemAmount - remainSpace > 0)
            {
                if (CheckIsPossibleAddItem(itemCode, (uint)(itemAmount - remainSpace), targetSlot.SlotNum + 1))
                {
                    result = true;
                }
            }
            else
            {
                result = true;
            }
        }
        else
        {
            uint newStartIndex = 0;
            while (true)
            {
                targetSlot = FindSameItemHaveSlot(ItemCode.None, newStartIndex);
                if (targetSlot != null)
                {
                    if (itemAmount > GameManager.Instance.ItemData[itemCode].maxAmount)
                    {
                        itemAmount -= GameManager.Instance.ItemData[itemCode].maxAmount;
                        newStartIndex = targetSlot.SlotNum + 1;
                    }
                    else
                    {
                        result = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        return result;
    }

    public bool AddItem(ItemCode itemCode, uint itemAmount)
    {
        bool result = false;
        if (CheckIsPossibleAddItem(itemCode, itemAmount))
        {
            ItemSlot targetSlot = FindSameItemHaveSlot(itemCode);
            if (targetSlot != null)
            {

            }
            else
            {

            }
        }
        return result;
    }

}
