using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCoroutine_Monster : UnitCoroutine_Base
{
    protected override IEnumerator HitCoroutine()
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

}
