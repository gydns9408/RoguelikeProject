using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCoroutine : MonoBehaviour
{
    SpriteRenderer _sprite;
    public SpriteRenderer Sprite
    {
        get => _sprite;
        set => _sprite = value;
    }

    WaitForSeconds _wait;
    public WaitForSeconds Wait
    {
        get => _wait;
        set => _wait = value;
    }

    public void Hit()
    {
        StartCoroutine(HitCoroutine());
    }

    IEnumerator HitCoroutine()
    {
        Color color = Sprite.material.color;
        for (int i = 0; i < 3; i++)
        {
            color.a = 0.5f;
            Sprite.material.color = color;
            yield return Wait;
            color.a = 1f;
            Sprite.material.color = color;
            yield return Wait;
        }
    }

    public void Die()
    {
        StopAllCoroutines();
        Color color = Sprite.material.color;
        color.a = 1f;
        Sprite.material.color = color;
    }
}
