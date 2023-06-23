using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IDropHandler
{
    ItemSlot _slot;
    ItemIcon _myItem;

    public bool IsEmpty => _slot.ItemData == null;

    private void Awake()
    {
        //Transform child = transform.GetChild(0);
        //_itemIcon = child.GetComponent<Image>();
        //child = transform.GetChild(1);
        //_itemAmountText = child.GetComponent<TextMeshProUGUI>();
    }

    public void Initialize(ItemSlot slot)
    {
        //_slot = slot;
        //Refresh();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemIcon dropItem = eventData.pointerDrag.GetComponent<ItemIcon>();
        if (dropItem == null) return;
        if (_myItem != null)
        {
            _myItem.SetParent(dropItem.OrgParent, true);
        }

        ItemSlotUI dropItemParent = dropItem.OrgParent.GetComponent<ItemSlotUI>();
        if (dropItemParent != null)
        {
            dropItemParent.SetChild(_myItem);
        }
        _myItem = dropItem;
        _myItem.SetParent(transform);
    }

    public void Refresh()
    {
        //if (IsEmpty)
        //{
        //    _itemIcon.sprite = null;
        //    _itemIcon.color = Color.clear;
        //    _itemAmountText.text = string.Empty;
        //}
        //else
        //{
        //    _itemIcon.sprite = _slot.ItemData.itemIcon;
        //    _itemIcon.color = Color.white;
        //    _itemAmountText.text = _slot.ItemAmount.ToString();
        //}
    }

    public void SetChild(ItemIcon child)
    {
        _myItem = child;
    }

}
