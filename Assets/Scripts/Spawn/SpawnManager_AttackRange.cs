using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_AttackRange : Singleton<SpawnManager_AttackRange>
{
    ObjectPool_AttackRange _objectPool;
    protected override void RunOnlyOnce_Initialize()
    {
        if (_initialized == false)
        {
            _objectPool = GetComponentInChildren<ObjectPool_AttackRange>();
        }
    }

    protected override void Initialize()
    {
        _objectPool?.Initialize();

    }

    public AttackRange GetObject()
    {
        return _objectPool.GetObject();
    }

    public AttackRange GetObject(Vector3 localScale,Transform goalTransfrom)
    {
        return _objectPool.GetObject(localScale, goalTransfrom);
    }
}
