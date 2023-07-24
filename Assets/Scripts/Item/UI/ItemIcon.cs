using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemIcon : PoolObjectShape, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    Image _itemIcon;
    TextMeshProUGUI _itemAmountText;
    Transform _firstParent;
    Transform _parentParent;

    static uint _spiltItem_minItemAmount = 2;
    static Vector3 _position_modify_value = new Vector3(0, 12.9f, 0);

    ItemCode _itemCode;
    public ItemCode ItemCode => _itemCode;
    uint _itemAmount;
    public uint ItemAmount => _itemAmount;

    public ItemSlotUI OrgParent
    {
        get; private set;
    }
    Vector2 _dragOffset;

    bool _isPointerEnter = false;

    private void Awake()
    {
        _itemIcon = GetComponentInChildren<Image>();
        _itemAmountText = GetComponentInChildren<TextMeshProUGUI>();
        _firstParent = transform.parent;
        _parentParent = GameManager.Instance.InvenUI.transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.InvenUI.Spliter.IsOpen)
        {
            _dragOffset = (Vector2)transform.position - eventData.position;
            transform.SetParent(_parentParent);
            _itemIcon.raycastTarget = false;
            Color color1 = _itemIcon.color;
            color1.a = 0.5f;
            _itemIcon.color = color1;
            Color color2 = _itemAmountText.color;
            color2.a = 0.5f;
            _itemAmountText.color = color2;

            GameManager.Instance.InvenUI.ExplanWindow.IsOff = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.InvenUI.Spliter.IsOpen)
        {
            transform.position = eventData.position + _dragOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.InvenUI.Spliter.IsOpen)
        {
            bool isParentChange = true;
            _itemIcon.raycastTarget = true;
            if (transform.parent == _parentParent)
            {
                transform.SetParent(OrgParent.transform);
                isParentChange = false;
            }
            transform.localPosition = _position_modify_value;

            Color color1 = _itemIcon.color;
            color1.a = 1f;
            _itemIcon.color = color1;
            Color color2 = _itemAmountText.color;
            color2.a = 1f;
            _itemAmountText.color = color2;

            GameManager.Instance.InvenUI.ExplanWindow.IsOff = false;
            if (isParentChange)
            {
                GameManager.Instance.InvenUI.ExplanWindow.Open(_itemCode);
            }
        }
    }

    public void SetParent(ItemSlotUI parent, bool rePos = false)
    {
        transform.SetParent(parent.transform);
        transform.localScale = Vector3.one;
        if(rePos)
        {
            transform.localPosition = _position_modify_value;
        }
        OrgParent = parent;
        OrgParent.Slot.SlotSetting(_itemCode, _itemAmount);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.InvenUIItemSplitMode)
        {
            if (_itemAmount >= _spiltItem_minItemAmount) 
            {
                GameManager.Instance.InvenUI.Spliter.Open(OrgParent.Slot);
            }
        }
        else if(eventData.clickCount >= 2)
        {
            OrgParent.Slot.UseItem();
        }
    }

    public void IconSetting(ItemCode itemCode, uint itemAmount, bool isSlotChange = false)
    {
        if (!isSlotChange)
        {
            OrgParent.Slot.SlotSetting(itemCode, itemAmount);
        }
        if (itemAmount != 0)
        {
            _itemCode = itemCode;
            _itemAmount = itemAmount;
            _itemIcon.sprite = GameManager.Instance.ItemData[_itemCode].itemIcon;
            _itemAmountText.text = itemAmount.ToString();
            
        }
        else
        {
            Before_OnDisable();
        }
    }

    public void SetRaycastTarget(bool value)
    {
        _itemIcon.raycastTarget = value;
    }

    public void Before_OnDisable()
    {
        if (OrgParent != null)
        {
            OrgParent.SetChild(null);
        }
        transform.SetParent(_firstParent);
        if(_isPointerEnter)
        {
            ExplanWindowClose();
        }
        gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.InvenUI.ExplanWindow.Open(_itemCode);
        _isPointerEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ExplanWindowClose();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        GameManager.Instance.InvenUI.ExplanWindow.PositionSetting(eventData.position);
    }

    private void ExplanWindowClose()
    {
        GameManager.Instance.InvenUI.ExplanWindow.Close();
        _isPointerEnter = false;
    }
}
