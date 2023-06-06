using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRange_Base : MonoBehaviour
{
    public Action<Monster_Base> onMonsterAttack;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Monster_Base mob = collision.gameObject.GetComponent<Monster_Base>();
            if (mob != null)
            {
                onMonsterAttack?.Invoke(mob);
            }
        }
    }
}
