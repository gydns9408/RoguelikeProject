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

    public bool IsEmpty => _slot.ItemCode == ItemCode.None;

    public void Initialize(ItemSlot slot)
    {
        _slot = slot;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManager.Instance.InvenUI.Spliter.IsOpen)
        {
            ItemIcon dropItem = eventData.pointerDrag.GetComponent<ItemIcon>();
            if (dropItem == null) return;
            if (dropItem.OrgParent != null)
            {
                if (Slot.ItemCode != dropItem.ItemCode)
                {
                    if (_myItem != null)
                    {
                        _myItem.SetParent(dropItem.OrgParent, true);
                    }
                    else
                    {
                        dropItem.OrgParent.Slot.SlotSetting(ItemCode.None, 0);
                    }
                    dropItem.OrgParent.SetChild(_myItem);
                    _myItem = dropItem;
                    _myItem.SetParent(this);
                }
                else
                {
                    if (Slot.ItemAmount < GameManager.Instance.ItemData[Slot.ItemCode].maxAmount)
                    {
                        int overValue = Mathf.Min((int)(GameManager.Instance.ItemData[Slot.ItemCode].maxAmount - Slot.ItemAmount), (int)dropItem.ItemAmount);
                        _myItem.IconSetting(_myItem.ItemCode, (uint)(_myItem.ItemAmount + overValue));
                        dropItem.IconSetting(dropItem.ItemCode, (uint)(dropItem.ItemAmount - overValue));
                    }
                    else
                    {
                        uint temp = dropItem.ItemAmount;
                        dropItem.IconSetting(dropItem.ItemCode, _myItem.ItemAmount);
                        _myItem.IconSetting(_myItem.ItemCode, temp);
                    }
                }
            }
        }
    }

    public void SetChild(ItemIcon child)
    {
        _myItem = child;
    }

    public void MyItemRefresh()
    {
        if (_myItem != null)
        {
            _myItem.IconSetting(Slot.ItemCode, Slot.ItemAmount, true);
        }
    }

}
