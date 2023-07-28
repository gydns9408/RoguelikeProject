using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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

    public MapManager mapManager;

    // Start is called before the first frame update
    protected override void Test_Action1(InputAction.CallbackContext _)
    {
        DamageText damageText = SpawnManager_Etc.Instance.GetObject_DamageText(GameManager.Instance.Player.Position.position + Vector3.up * 1f);
        damageText.DamageTextSetting("191", DamageSkin.Critical);
    }

    protected override void Test_Action2(InputAction.CallbackContext _)
    {
        player.transform.position = mapManager.GridMap.GridToWorld(GameManager.Instance.NowRoom.DoorInfos[3].x, GameManager.Instance.NowRoom.DoorInfos[3].y);
    }

    protected override void Test_Action3(InputAction.CallbackContext _)
    {
        //invenUI.Inven[0].SlotSetting(itemDataManager[ItemCode.BeastMeat], 10);
        //invenUI.ItemSlotUI[0].Refresh();
        //icon = SpawnManager_Etc.Instance.GetObject_ItemIcon();
        //slotUI.SetChild(icon);
        //icon.SetParent(slotUI, true);
        //icon.IconSetting(ItemCode.BeastMeat, 1);
        //icon2 = SpawnManager_Etc.Instance.GetObject_ItemIcon();
        //slotUI2.SetChild(icon2);
        //icon2.SetParent(slotUI2, true);
        //icon2.IconSetting(ItemCode.BeastMeat, 2);
        //icon3 = SpawnManager_Etc.Instance.GetObject_ItemIcon();
        //slotUI3.SetChild(icon3);
        //icon3.SetParent(slotUI3, true);
        //icon3.IconSetting(ItemCode.BeastMeat, 10);
        if (!GameManager.Instance.InvenUI.Inven.AddItem(ItemCode.BeastMeat, 1, out uint overCount))
        {
            Debug.Log($"아이템 {overCount}개 추가 실패");
        }
        if (!GameManager.Instance.InvenUI.Inven.AddItem(ItemCode.BeastMeat, 3, out uint overCount2))
        {
            Debug.Log($"아이템 {overCount2}개 추가 실패");
        }
        if (!GameManager.Instance.InvenUI.Inven.AddItem(ItemCode.BeastMeat, 10, out uint overCount3))
        {
            Debug.Log($"아이템 {overCount3}개 추가 실패");
        }
    }

    protected override void Test_Action4(InputAction.CallbackContext _)
    {
        if (!GameManager.Instance.InvenUI.Inven.AddItem(ItemCode.ApakiFruit, 3, out uint overCount))
        {
            Debug.Log($"아이템 {overCount}개 추가 실패");
        }
    }

}
