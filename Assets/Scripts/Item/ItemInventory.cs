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

    public bool CheckIsPossibleAddItem(ItemCode itemCode, uint itemAmount, out uint overAmount)
    {
        bool result = false;
        uint startIndex = 0;
        ItemSlot targetSlot = null;
        uint first_itemAmount = itemAmount;
        overAmount = itemAmount;

        while (true)
        {
            targetSlot = FindSameItemHaveSlot(itemCode, startIndex);
            if (targetSlot != null)
            {
                uint remainAmount = GameManager.Instance.ItemData[itemCode].maxAmount - targetSlot.ItemAmount;
                if (itemAmount > remainAmount)
                {
                    itemAmount -= remainAmount;
                    startIndex = targetSlot.SlotNum + 1;
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

        if (!result)
        { 
            startIndex = 0;
            while (true)
            {
                targetSlot = FindSameItemHaveSlot(ItemCode.None, startIndex);
                if (targetSlot != null)
                {
                    if (itemAmount > GameManager.Instance.ItemData[itemCode].maxAmount)
                    {
                        itemAmount -= GameManager.Instance.ItemData[itemCode].maxAmount;
                        startIndex = targetSlot.SlotNum + 1;
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
        if (!result && first_itemAmount > itemAmount)
        {
            overAmount = first_itemAmount - itemAmount;
        }
        return result;
    }

    public bool AddItem(ItemCode itemCode, uint itemAmount, out uint overCount, bool firstRun = true)
    {
        bool result = false;
        bool checkPass = !firstRun;
        overCount = itemAmount;

        if (!checkPass)
        {
            if (!CheckIsPossibleAddItem(itemCode, itemAmount, out uint overAmount))
            {
                if (overAmount == itemAmount)
                {
                    return false;
                }
                else
                {
                    overCount = overAmount;
                    result = AddItem(itemCode, itemAmount - overAmount, out uint overC, false);
                }
            }
            else
            {
                overCount = 0;
            }
        }

        if (result) return true;

        ItemSlot targetSlot = FindSameItemHaveSlot(itemCode);
        if (targetSlot != null)
        {
            uint remainAmount = GameManager.Instance.ItemData[itemCode].maxAmount - targetSlot.ItemAmount;
            if (remainAmount < itemAmount)
            {
                targetSlot.SlotSetting(itemCode, GameManager.Instance.ItemData[itemCode].maxAmount, true);
                result = AddItem(itemCode, itemAmount - remainAmount, out uint overC, false);
            }
            else
            {
                targetSlot.SlotSetting(itemCode, targetSlot.ItemAmount + itemAmount, true);
                result = true;
            }
        }
        else
        {
            targetSlot = FindSameItemHaveSlot(ItemCode.None);
            if (targetSlot != null)
            {
                if (GameManager.Instance.ItemData[itemCode].maxAmount < itemAmount)
                {
                    targetSlot.SlotSetting(itemCode, GameManager.Instance.ItemData[itemCode].maxAmount, true);
                    result = AddItem(itemCode, itemAmount - GameManager.Instance.ItemData[itemCode].maxAmount, out uint overC, false);
                }
                else
                {
                    targetSlot.SlotSetting(itemCode, itemAmount, true);
                    result = true;
                }
            }
        }
   
        return result;
    }

}
