using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInventoryUI : UI_Window_HaveOpenCloseAnim
{
    ItemSlotUI[] _slots;
    public ItemSlotUI[] ItemSlotUI => _slots;

    ItemInventory _inven;
    public ItemInventory Inven => _inven;

    ItemSpliterUI _spliter;
    public ItemSpliterUI Spliter => _spliter;
    ItemExplanWindowUI _explanWindow;
    public ItemExplanWindowUI ExplanWindow => _explanWindow;
    ItemSortOptionWindowUI _sortWindow;
    public ItemSortOptionWindowUI SortWindow => _sortWindow;

    public GameObject slotPrefab;
    
    protected override void Awake()
    {
        base.Awake();
        _slots = GetComponentsInChildren<ItemSlotUI>();
        _spliter = GetComponentInChildren<ItemSpliterUI>();
        _explanWindow = GetComponentInChildren<ItemExplanWindowUI>();
        _sortWindow = GetComponentInChildren<ItemSortOptionWindowUI>();
    }

    protected override void Start()
    {

    }

        public void Initialize(ItemInventory inven)
    {
        _inven = inven;
        if (_slots.Length != inven.InventorySize)
        {
            Transform slots = transform.GetChild(0);
            GridLayoutGroup gridLayout = slots.GetComponent<GridLayoutGroup>(); 
            foreach (ItemSlotUI slot in _slots)
            {
                Destroy(slot.gameObject);
            }

            RectTransform slotsRect = (RectTransform)slots;

            float invenSizeSqrt = Mathf.Sqrt(inven.InventorySize);
            if ((int)invenSizeSqrt * (int)invenSizeSqrt != inven.InventorySize) 
            {
                invenSizeSqrt = Mathf.Ceil(invenSizeSqrt);
            }
            float oneSlotSideLength = Mathf.Floor((slotsRect.rect.width - gridLayout.padding.left * 2.5f) / invenSizeSqrt);
            gridLayout.cellSize = new Vector2(oneSlotSideLength, oneSlotSideLength);

            _slots = new ItemSlotUI[inven.InventorySize];
            for (int i = 0; i < _slots.Length; i++)
            {
                GameObject obj = Instantiate(slotPrefab, slots);
                obj.name = $"{slotPrefab.name}{i + 1}";
                _slots[i] = obj.GetComponent<ItemSlotUI>();
            }
        }

        for (uint i = 0; i < _slots.Length; i++)
        {
            _slots[i].Initialize(inven[i]);
        }
        Close();
    }

    public override void FullOpen()
    {
        for (uint i = 0; i < _slots.Length; i++)
        {
            _slots[i].SetRaycastTarget(true);
        }
        SortWindow.AllButtonActivate();
        base.FullOpen();
    }

    public override void StartClose()
    {
        for (uint i = 0; i < _slots.Length; i++)
        {
            _slots[i].SetRaycastTarget(false);
        }
        SortWindow.AllButtonDeactivate();
        base.StartClose();
    }

    public void EndClose()
    {
        Spliter.Close();
        Close();
    }
}
