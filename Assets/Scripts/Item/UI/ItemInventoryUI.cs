using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryUI : MonoBehaviour
{
    ItemSlotUI[] _slots;

    public GameObject slotPrefab;
    
    // Start is called before the first frame update
    private void Awake()
    {
       _slots = GetComponentsInChildren<ItemSlotUI>();
    }

    public void Initialize(ItemInventory inven)
    {
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
            float oneSlotSideLength = Mathf.Floor((slotsRect.rect.width - gridLayout.padding.left * 2f) / invenSizeSqrt);
            gridLayout.cellSize = new Vector2(oneSlotSideLength, oneSlotSideLength);

            _slots = new ItemSlotUI[inven.InventorySize];
            for (int i = 0; i < _slots.Length; i++)
            {
                GameObject obj = Instantiate(slotPrefab, slots);
                obj.name = $"{slotPrefab.name}{i + 1}";
                _slots[i] = obj.GetComponent<ItemSlotUI>();
            }
        }
    }
}
