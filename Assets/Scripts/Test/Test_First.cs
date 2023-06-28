using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_First : Test_Base
{
    public Player player;
    public Monster_Monster1 monster_Monster1;
    public ItemInventoryUI invenUI;

    public ItemSlotUI slotUI;
    ItemIcon icon;
    public ItemSlotUI slotUI2;
    ItemIcon icon2;
    public ItemSlotUI slotUI3;
    ItemIcon icon3;

    // Start is called before the first frame update
    protected override void Test_Action1(InputAction.CallbackContext _)
    {
        player.Test_HPChange(10);
    }

    protected override void Test_Action2(InputAction.CallbackContext _)
    {
        player.Test_HPChange(-10);
    }

    protected override void Test_Action3(InputAction.CallbackContext _)
    {
        //invenUI.Inven[0].SlotSetting(itemDataManager[ItemCode.BeastMeat], 10);
        //invenUI.ItemSlotUI[0].Refresh();
        icon = SpawnManager_Etc.Instance.GetObject_ItemIcon();
        slotUI.SetChild(icon);
        icon.SetParent(slotUI, true);
        icon.IconSetting(ItemCode.BeastMeat, 1);
        icon2 = SpawnManager_Etc.Instance.GetObject_ItemIcon();
        slotUI2.SetChild(icon2);
        icon2.SetParent(slotUI2, true);
        icon2.IconSetting(ItemCode.BeastMeat, 2);
        icon3 = SpawnManager_Etc.Instance.GetObject_ItemIcon();
        slotUI3.SetChild(icon3);
        icon3.SetParent(slotUI3, true);
        icon3.IconSetting(ItemCode.BeastMeat, 10);
        
    }

}
