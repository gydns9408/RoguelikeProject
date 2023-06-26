using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_Etc : Singleton<SpawnManager_Etc>
{
    ObjectPool_DropItem _objectPool_dropItem;
    protected override void RunOnlyOnce_Initialize()
    {
        if (_initialized == false)
        {
            _objectPool_dropItem = GetComponentInChildren<ObjectPool_DropItem>();
        }
    }

    protected override void Initialize()
    {
        if (_objectPool_dropItem != null) 
        {
            _objectPool_dropItem.Initialize();
        }
    }

    public DropItem GetObject_DropItem()
    {
        return _objectPool_dropItem.GetObject();
    }

}
