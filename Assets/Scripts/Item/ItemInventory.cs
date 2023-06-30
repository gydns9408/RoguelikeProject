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

    public bool AddItem(ItemCode itemCode, uint itemAmount, out uint overCount)
    {
        bool result = false;
        bool last = true;
        overCount = itemAmount;

        ItemSlot targetSlot = FindSameItemHaveSlot(itemCode);
        if (targetSlot != null)
        {
            uint remainAmount = GameManager.Instance.ItemData[itemCode].maxAmount - targetSlot.ItemAmount;
            if (remainAmount < itemAmount)
            {
                targetSlot.SlotSetting(itemCode, GameManager.Instance.ItemData[itemCode].maxAmount, true);
                result = AddItem(itemCode, itemAmount - remainAmount, out uint overC);
                last = false;
                overCount = overC;
            }
            else
            {
                targetSlot.SlotSetting(itemCode, targetSlot.ItemAmount + itemAmount, true);
                overCount = 0;
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
                    result = AddItem(itemCode, itemAmount - GameManager.Instance.ItemData[itemCode].maxAmount, out uint overC);
                    last = false;
                    overCount = overC;
                }
                else
                {
                    targetSlot.SlotSetting(itemCode, itemAmount, true);
                    overCount = 0;
                    result = true;
                }
            }
        }

        if (last)
        {
            overCount = itemAmount;
        }

        return result;
    }

    public bool AddItem_EmptySlot(ItemCode itemCode, uint itemAmount)
    {
        bool result = false;
        ItemSlot targetSlot = FindSameItemHaveSlot(ItemCode.None);
        if (targetSlot != null)
        {
            targetSlot.SlotSetting(itemCode, itemAmount, true);
            result = true;
        }
        return result;
    }

    }
