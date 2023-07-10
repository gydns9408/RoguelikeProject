using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSortOptionWindowUI : MonoBehaviour, IHaveButtonUI
{
    Button name_ascendingOrderSort_button;
    Button name_descendingOrderSort_button;
    Button code_ascendingOrderSort_button;
    Button code_descendingOrderSort_button;

    Image name_ascendingOrderSort_button_image;
    Image name_descendingOrderSort_button_image;
    Image code_ascendingOrderSort_button_image;
    Image code_descendingOrderSort_button_image;
    private void Awake()
    { 
        Transform child = transform.GetChild(0);
        name_ascendingOrderSort_button = child.GetComponent<Button>();
        name_ascendingOrderSort_button.onClick.AddListener(() => GameManager.Instance.InvenUI.Inven.InventorySort(ItemSortingWay.ItemName));
        name_ascendingOrderSort_button_image = child.GetComponent<Image>();
        child = transform.GetChild(1);
        name_descendingOrderSort_button = child.GetComponent<Button>();
        name_descendingOrderSort_button.onClick.AddListener(() => GameManager.Instance.InvenUI.Inven.InventorySort(ItemSortingWay.ItemName, false));
        name_descendingOrderSort_button_image = child.GetComponent<Image>();
        child = transform.GetChild(2);
        code_ascendingOrderSort_button = child.GetComponent<Button>();
        code_ascendingOrderSort_button.onClick.AddListener(() => GameManager.Instance.InvenUI.Inven.InventorySort(ItemSortingWay.ItemCode));
        code_ascendingOrderSort_button_image = child.GetComponent <Image>();
        child = transform.GetChild(3);
        code_descendingOrderSort_button = child.GetComponent<Button>();
        code_descendingOrderSort_button.onClick.AddListener(() => GameManager.Instance.InvenUI.Inven.InventorySort(ItemSortingWay.ItemCode, false));
        code_descendingOrderSort_button_image = child.GetComponent<Image>();
        AllButtonDeactivate();
    }

    public void AllButtonActivate()
    {
        name_ascendingOrderSort_button.enabled = true;
        name_ascendingOrderSort_button_image.raycastTarget = true;
        name_descendingOrderSort_button.enabled = true;
        name_descendingOrderSort_button_image.raycastTarget = true;
        code_ascendingOrderSort_button.enabled = true;
        code_ascendingOrderSort_button_image.raycastTarget = true;
        code_descendingOrderSort_button.enabled = true;
        code_descendingOrderSort_button_image.raycastTarget = true;
    }

    public void AllButtonDeactivate()
    {
        name_ascendingOrderSort_button.enabled = false;
        name_ascendingOrderSort_button_image.raycastTarget = false;
        name_descendingOrderSort_button.enabled = false;
        name_descendingOrderSort_button_image.raycastTarget = false;
        code_ascendingOrderSort_button.enabled = false;
        code_ascendingOrderSort_button_image.raycastTarget = false;
        code_descendingOrderSort_button.enabled = false;
        code_descendingOrderSort_button_image.raycastTarget = false;
    }
}
