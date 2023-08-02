using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackRange_Base : MonoBehaviour
{
    protected bool _detectPlayer = false;
    public Action onPlayerAttack;
    public bool DetectPlayer => _detectPlayer;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _detectPlayer = true;
            onPlayerAttack?.Invoke();
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _detectPlayer = false;
        }
    }
}
