using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpliterUI : UI_Window_Base
{
    bool _isFullOpen = false;

    ItemSlot _linked_itemSlot;

    Image _itemImage;
    Slider _itemAmount_slider;
    TMP_InputField _itemAmount_inputField;
    Button _itemAmount_plusButton;
    Button _itemAmount_minusButton;
    Button _okButton;
    Button _cancelButton;
    Animator _anim;

    const uint _itemSplitAmount_min = 1;

    uint _itemSplitAmount = _itemSplitAmount_min;
    uint ItemSplitAmount
    {
        get => _itemSplitAmount;
        set
        {
            if (_linked_itemSlot != null)
            {
                uint totalValue = (uint)Mathf.Clamp((int)value, (int)_itemSplitAmount_min, (int)_linked_itemSlot.ItemAmount - 1);
                _itemSplitAmount = totalValue;
                _itemAmount_slider.value = _itemSplitAmount;
                _itemAmount_inputField.text = _itemSplitAmount.ToString();
            }
        }
    }

    public bool IsOpen => gameObject.activeSelf;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        _itemImage = child.GetComponent<Image>();
        _itemAmount_slider = GetComponentInChildren<Slider>();
        _itemAmount_slider.onValueChanged.AddListener(ItemAmount_SliderChange);
        _itemAmount_slider.minValue = _itemSplitAmount_min;
        _itemAmount_inputField = GetComponentInChildren<TMP_InputField>();
        _itemAmount_inputField.onValueChanged.AddListener(ItemAmount_InputFieldChange);
        child = transform.GetChild(3);
        _itemAmount_plusButton = child.GetComponent<Button>();
        _itemAmount_plusButton.onClick.AddListener(() =>
        {
            if (_isFullOpen)
            {
                ItemSplitAmount++;
            }
        });
        child = transform.GetChild(4);
        _itemAmount_minusButton = child.GetComponent<Button>();
        _itemAmount_minusButton.onClick.AddListener(() => ItemSplitAmount--);
        child = transform.GetChild(5);
        _okButton = child.GetComponent<Button>();
        child = transform.GetChild(6);
        _okButton.onClick.AddListener(ItemSplit);
        _cancelButton = child.GetComponent<Button>();
        _cancelButton.onClick.AddListener(() => Close());
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        Close();
    }

    public void Open(ItemSlot itemSlot)
    {
        _linked_itemSlot = itemSlot;
        _itemImage.sprite = GameManager.Instance.ItemData[_linked_itemSlot.ItemCode].itemIcon;
        _itemAmount_slider.maxValue = _linked_itemSlot.ItemAmount - 1;
        ItemSplitAmount = _itemSplitAmount_min;
        gameObject.SetActive(true);
    }

    private void ItemSplit()
    {
        if (_isFullOpen)
        {
            if (!GameManager.Instance.InvenUI.Inven.AddItem_EmptySlot(_linked_itemSlot.ItemCode, ItemSplitAmount))
            {
                Debug.Log("ºóÄ­¾ø¾î");
            }
            else
            {
                _linked_itemSlot.SlotSetting(_linked_itemSlot.ItemCode, _linked_itemSlot.ItemAmount - ItemSplitAmount, true);
            }
            Close();
        }
    }


    private void ItemAmount_InputFieldChange(string value)
    {
        if (uint.TryParse(value, out uint result))
        {
            ItemSplitAmount = result;
        }
        else
        {
            ItemSplitAmount = _itemSplitAmount_min;
        }
    }

    private void ItemAmount_SliderChange(float value)
    {
        ItemSplitAmount = (uint)value;
    }

    public void FullOpen()
    {
        _isFullOpen = true;
    }

    public void StartClose()
    {
        _isFullOpen = false;
    }
}
