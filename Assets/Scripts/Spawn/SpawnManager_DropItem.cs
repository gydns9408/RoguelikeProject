using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_DropItem : Singleton<SpawnManager_DropItem>
{
    ObjectPool_DropItem[] _objectPool;
    protected override void RunOnlyOnce_Initialize()
    {
        if (_initialized == false)
        {
            _objectPool = GetComponentsInChildren<ObjectPool_DropItem>();
        }
    }

    protected override void Initialize()
    {
        for (int i = 0; i < _objectPool.Length; i++)
        {
            _objectPool[i].Initialize();
        }

    }

    public DropItem GetObject(ItemType item_type)
    {
        return _objectPool[(int)item_type].GetObject();
    }

}
