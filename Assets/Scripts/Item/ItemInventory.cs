using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory
{
    ItemSlot[] _slots;
    public ItemSlot this[uint index] => _slots[index];
    public uint InventorySize => (uint)_slots.Length;

    const uint _tempSlotNum = 987654321;
    ItemSlot _tempSlot;
    public ItemSlot TempSlot => _tempSlot; 

    Player _owner;
    public Player Owner => _owner;

    public ItemInventory(uint size, Player owner)
    {
        _slots = new ItemSlot[size];
        for (uint i = 0; i < size; i++)
        {
            _slots[i] = new ItemSlot(i);
        }
        _tempSlot = new ItemSlot(_tempSlotNum);
        _owner = owner;
    }
}
