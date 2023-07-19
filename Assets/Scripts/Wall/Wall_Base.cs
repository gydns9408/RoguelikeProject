using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallCode
{
    BlackTombstone = 0,
    WhiteTombstone,
    TauStatue,
    ReaperStatue,
    DevilStatue
}

public class Wall_Base : PoolObjectShape
{
    protected SpriteRenderer[] _sprites;

    public float X_Correction_Value = 0f;
    public float Y_Correction_Value = 0.2f;

    protected void Awake()
    {
        _sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    protected void OnEnable()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Sprite_SortingOrderSetting()
    {
        foreach (var sprite in _sprites)
        {
            sprite.sortingOrder = (int)((transform.position.y - Y_Correction_Value) * -100);
        }
    }
}
