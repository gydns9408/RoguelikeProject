using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCoroutine_Base : MonoBehaviour
{
    protected SpriteRenderer _sprite;
    public SpriteRenderer Sprite
    {
        get => _sprite;
        set => _sprite = value;
    }

    protected WaitForSeconds _wait;
    public WaitForSeconds Wait
    {
        get => _wait;
        set => _wait = value;
    }

    


    public void Hit()
    {
        StartCoroutine(HitCoroutine());
    }

    protected virtual IEnumerator HitCoroutine()
    {
        yield return null;
    }

    public void Die()
    {
        StopAllCoroutines();
        Color color = Sprite.material.color;
        color.a = 1f;
        Sprite.material.color = color;
    }
}
