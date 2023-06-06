using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackRange_Base : MonoBehaviour
{
    bool _detectPlayer = false;
    public Action onPlayerAttack;
    public bool DetectPlayer => _detectPlayer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _detectPlayer = true;
            onPlayerAttack?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _detectPlayer = false;
        }
    }
}
