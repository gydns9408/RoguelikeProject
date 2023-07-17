using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Base : PoolObjectShape
{
    protected SpriteRenderer[] _sprites;

    public float X_Correction_Value = 0f;
    public float Y_Correction_Value = 0.2f;

    protected void Awake()
    {
        _sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    public void Sprite_SortingOrderSetting()
    {
        foreach (var sprite in _sprites)
        {
            sprite.sortingOrder = (int)((transform.position.y - Y_Correction_Value) * -100);
        }
    }
}
