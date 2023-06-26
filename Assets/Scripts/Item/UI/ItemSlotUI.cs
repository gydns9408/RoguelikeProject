using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IDropHandler
{
    ItemSlot _slot;
    public ItemSlot Slot => _slot;
    ItemIcon _myItem;

    public bool IsEmpty => _slot.ItemData == null;

    public void Initialize(ItemSlot slot)
    {
        _slot = slot;
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemIcon dropItem = eventData.pointerDrag.GetComponent<ItemIcon>();
        if (dropItem == null) return;
        if (dropItem.OrgParent != null)
        {
            if (Slot.ItemData != dropItem.ItemData)
            {
                if (_myItem != null)
                {
                    _myItem.SetParent(dropItem.OrgParent, true);
                }
                dropItem.OrgParent.SetChild(_myItem);
                _myItem = dropItem;
                _myItem.SetParent(this);
            }
            else
            {
                if (Slot.ItemAmount < Slot.ItemData.maxAmount)
                { 
                }
            }
        }
    }

    public void SetChild(ItemIcon child)
    {
        _myItem = child;
    }

}
