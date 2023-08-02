using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCoroutine_Player : UnitCoroutine_Base
{
    Player _owner;
    public Player Owner
    {
        get => _owner;
        set => _owner = value;
    }

    WaitForSeconds _wait_mpRecover;

    private void Awake()
    {
        _wait_mpRecover = new WaitForSeconds(10f);
    }

    public void MPRecoverCoroutineStart()
    {
        StartCoroutine(MPRecoverCoroutine());
    }

    IEnumerator MPRecoverCoroutine()
    {
        while (true)
        {
            yield return _wait_mpRecover;
            Owner.MPChange(Owner.MP + Owner.MpRecoverCoroutine_RecoverAmount);
        }
    }

    protected override IEnumerator HitCoroutine()
    {
        Color color = Sprite.material.color;
        while (Owner.Hit_InvincibleTime_Value > 0)
        {
            color.a = 0.5f;
            Sprite.material.color = color;
            yield return Wait;
            if (Owner.Hit_InvincibleTime_Value <= 0)
            {
                break;
            }
            color.a = 1f;
            Sprite.material.color = color;
            yield return Wait;
        }
        color.a = 1f;
        Sprite.material.color = color;
    }



}
