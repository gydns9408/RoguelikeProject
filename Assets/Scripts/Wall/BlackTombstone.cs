using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlackTombstone : PoolObjectShape
{

    SpriteRenderer[] _sprites;

    const float x_correction_value = 0f;
    public float X_Correction_Value => x_correction_value;
    const float y_correction_value = 0.2f;
    public float Y_Correction_Value => y_correction_value;
    private void Awake()
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
