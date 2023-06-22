using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : PoolObjectShape
{
    ItemData _itemData;
    public ItemData ItemData
    {
        get => _itemData;
        set
        {
            if (_itemData == null)
            {
                _itemData = value;
            }
        }
    }

    uint _itemAmount;
    public uint ItemAmount
    {
        get => _itemAmount;
        set
        {
            if (_itemAmount == 0)
            {
                _itemAmount = value;
            }
        }
    }

    Transform _child;
    public float _rotSpeed = 360f;

    public float _remainTime = 10.0f;
    private void Awake()
    {
        _child = transform.GetChild(0);
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        if (gameObject.activeSelf == true)
        {
            StartCoroutine(LifeOver(_remainTime));
        }
    }

    private void Update()
    {
        _child.Rotate(0f, _rotSpeed * Time.deltaTime, 0f);
    }
}
