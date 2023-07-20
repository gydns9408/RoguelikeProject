using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Monster_Type
{
    WildBoar
}

public class SpawnManager_Monster : Singleton<SpawnManager_Monster>
{
    ObjectPool_Monster_Monster[] _objectPool;
    protected override void RunOnlyOnce_Initialize()
    {
        if (_initialized == false)
        {
            _objectPool = GetComponentsInChildren<ObjectPool_Monster_Monster>();
        }
    }

    protected override void Initialize()
    {
        for (int i = 0; i < _objectPool.Length; i++)
        {
            _objectPool[i].Initialize();
        }
    }

    public Monster_Base GetObject(Monster_Type mon_type)
    {
        return _objectPool[(int)mon_type].GetObject();
    }

}
