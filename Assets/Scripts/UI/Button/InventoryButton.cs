using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : Button_Base
{
    protected override void ButtonClickEvent()
    {
        if (GameManager.Instance.InvenUI.gameObject.activeSelf == false)
        {
            GameManager.Instance.InvenUI.Open();
        }
        else
        {
            GameManager.Instance.InvenUI.Close();
        }
    }
}
