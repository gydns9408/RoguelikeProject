using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_Etc : Singleton<SpawnManager_Etc>
{
    ObjectPool_DropItem _objectPool_dropItem;
    ObjectPool_ItemIcon _objectPool_itemIcon;
    ObjectPool_Wall_BlackTombstone _objectPool_wall_blackTombstone;
    protected override void RunOnlyOnce_Initialize()
    {
        if (_initialized == false)
        {
            _objectPool_dropItem = GetComponentInChildren<ObjectPool_DropItem>();
            _objectPool_itemIcon = GetComponentInChildren<ObjectPool_ItemIcon>();
            _objectPool_wall_blackTombstone = GetComponentInChildren<ObjectPool_Wall_BlackTombstone>();
        }
    }

    protected override void Initialize()
    {
        if (_objectPool_dropItem != null) 
        {
            _objectPool_dropItem.Initialize();
        }
        if (_objectPool_itemIcon != null)
        {
            _objectPool_itemIcon.Initialize();
        }
        if (_objectPool_wall_blackTombstone != null)
        {
            _objectPool_wall_blackTombstone.Initialize();
        }
    }

    public DropItem GetObject_DropItem(ItemCode itemCode, uint itemAmount)
    {
        DropItem dropItem = _objectPool_dropItem.GetObject();
        dropItem.DropItemSetting(itemCode, itemAmount);
        return dropItem;
    }

    public ItemIcon GetObject_ItemIcon()
    {
        return _objectPool_itemIcon.GetObject();
    }

    public Wall_BlackTombstone GetObject_Wall_BlackTombstone()
    {
        return _objectPool_wall_blackTombstone.GetObject();
    }

}
