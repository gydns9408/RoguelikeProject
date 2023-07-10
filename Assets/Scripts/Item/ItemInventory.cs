using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public enum ItemSortingWay
{
    ItemName,
    ItemCode
}
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

    public void InventorySort(ItemSortingWay sortWay, bool isAscendingOrder = true)
    {
        List<ItemSlot> sort_slots = new List<ItemSlot>((int)InventorySize);
        foreach (var slot in _slots)
        {
            sort_slots.Add(slot);
        }

        switch (sortWay)
        {
            case ItemSortingWay.ItemCode:
                sort_slots.Sort(
                    (x, y) => 
                    {
                        if (x.ItemCode == ItemCode.None)
                        {
                            return 1;
                        }
                        if (y.ItemCode == ItemCode.None)
                        {
                            return -1;
                        }
                        if (isAscendingOrder)
                        {
                            return x.ItemCode.CompareTo(y.ItemCode);
                        }
                        else
                        {
                            return y.ItemCode.CompareTo(x.ItemCode);
                        }
                    }
                    );
                break;
            case ItemSortingWay.ItemName:
            default:
                sort_slots.Sort(
                    (x, y) =>
                    {
                        if (x.ItemCode == ItemCode.None)
                        { 
                            return 1;
                        }
                        if (y.ItemCode == ItemCode.None)
                        {
                            return -1;
                        }
                        if (isAscendingOrder)
                        {
                            return GameManager.Instance.ItemData[x.ItemCode].itemName.CompareTo(GameManager.Instance.ItemData[y.ItemCode].itemName);
                        }
                        else
                        {
                            return GameManager.Instance.ItemData[y.ItemCode].itemName.CompareTo(GameManager.Instance.ItemData[x.ItemCode].itemName);
                        }
                    }
                    );
                break;
        }

        List<(ItemCode,uint)> sort_datas = new List<(ItemCode, uint)>((int)InventorySize);
        foreach (var slot in sort_slots)
        {
            sort_datas.Add((slot.ItemCode, slot.ItemAmount));
        }
        int checkRange = sort_datas.Count - 1;
        for (int i = 0; i < checkRange; i++)
        {
            if (sort_datas[i].Item1 == sort_datas[i + 1].Item1 && sort_datas[i].Item2 < GameManager.Instance.ItemData[sort_datas[i].Item1].maxAmount)
            {
                int overValue = Mathf.Min((int)(GameManager.Instance.ItemData[sort_datas[i].Item1].maxAmount - sort_datas[i].Item2), (int)sort_datas[i + 1].Item2);
                sort_datas[i] = (sort_datas[i].Item1, sort_datas[i].Item2 + (uint)overValue);
                sort_datas[i + 1] = (sort_datas[i + 1].Item1, sort_datas[i + 1].Item2 - (uint)overValue);
                if (sort_datas[i + 1].Item2 < 1)
                {
                    sort_datas.RemoveAt(i + 1);
                    sort_datas.Add((ItemCode.None, 0));
                    checkRange--;
                    i--;
                }
            }
        }

        int index = 0;
        foreach (var data in sort_datas)
        {
            _slots[index].SlotSetting(data.Item1, data.Item2 ,true);
            index++;
        }

    }

}
