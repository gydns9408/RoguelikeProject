using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    Image _itemIcon;
    TextMeshProUGUI _itemAmountText;
    Transform _parentParent;
    Vector3 _position_modify_value = new Vector3(0, 12.9f, 0);

    ItemData _itemData;
    public ItemData ItemData => _itemData;
    uint _itemAmount;
    public uint ItemAmount => _itemAmount;

    Vector3 _orgPos;
    public ItemSlotUI OrgParent
    {
        get; private set;
    }
    Vector2 _dragOffset;

    private void Awake()
    {
        _itemIcon = GetComponentInChildren<Image>();
        _itemAmountText = GetComponentInChildren<TextMeshProUGUI>();
        _parentParent = transform.parent.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _orgPos = transform.position;
        _dragOffset = (Vector2)transform.position - eventData.position;
        transform.SetParent(_parentParent);
        _itemIcon.raycastTarget = false;
        Color color1 = _itemIcon.color;
        color1.a = 0.5f;
        _itemIcon.color = color1;
        Color color2 = _itemAmountText.color;
        color2.a = 0.5f;
        _itemAmountText.color = color2;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + _dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _orgPos;
        _itemIcon.raycastTarget = true;
        if (transform.parent == _parentParent)
        {
            transform.SetParent(OrgParent.transform);
        }
        Color color1 = _itemIcon.color;
        color1.a = 1f;
        _itemIcon.color = color1;
        Color color2 = _itemAmountText.color;
        color2.a = 1f;
        _itemAmountText.color = color2;
    }

    public void SetParent(ItemSlotUI parent, bool rePos = false)
    {
        transform.SetParent(parent.transform);
        _orgPos = parent.transform.position + _position_modify_value;
        if(rePos)
        {
            transform.position = parent.transform.position + _position_modify_value;
        }
        OrgParent = parent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void IconSetting(ItemData itemData, uint itemAmount)
    {
        OrgParent.Slot.SlotSetting(itemData, itemAmount);
        if (itemAmount != 0)
        {
            _itemData = itemData;
            _itemAmount = itemAmount;
            _itemIcon.sprite = _itemData.itemIcon;
            _itemAmountText.text = itemAmount.ToString();
            
        }
        else
        {
            if(OrgParent != null)
            {
                OrgParent.SetChild(null);
            }
            gameObject.SetActive(false);
        }
    }
}