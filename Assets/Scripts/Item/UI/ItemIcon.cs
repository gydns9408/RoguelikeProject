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
    public Transform OrgParent
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
        OrgParent = transform.parent;
        _dragOffset = (Vector2)transform.position - eventData.position;
        transform.SetParent(_parentParent);
        _itemIcon.raycastTarget = false;
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
            transform.SetParent(OrgParent);
        }
    }

    public void SetParent(Transform parent, bool rePos = false)
    {
        transform.SetParent(parent);
        _orgPos = parent.position + _position_modify_value;
        if(rePos)
        {
            transform.position = parent.position + _position_modify_value;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

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
        if (_itemData != null)
        {
            _itemIcon.sprite = _itemData.itemIcon;
            _itemAmountText.text = itemAmount.ToString();
        }
    }
}
