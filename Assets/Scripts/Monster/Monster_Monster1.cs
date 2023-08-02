using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Monster1 : Monster_Base
{
    MonsterAttackEffect1 _attackEffect1;
    Animator _attackEffect1_anim;
    WaitForSeconds _attack_wait2;
    protected override void Awake()
    {
        base.Awake();
        _attackEffect1 = GetComponentInChildren<MonsterAttackEffect1>();
        _attackEffect1_anim = _attackEffect1.gameObject.GetComponentInChildren<Animator>();
        _attack_wait2 = new WaitForSeconds(_attackEffect1_anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        _attackEffect1.gameObject.SetActive(false);
    }
    protected override IEnumerator Attack()
    {
        _anim.SetBool(_isMoveHash, false);
        _attackRange1_anim.SetTrigger(_activeHash);
        yield return _attack_wait;
        _attackEffect1.gameObject.SetActive(true);
        _attack_active = true;
        if (_attackRange1.DetectPlayer)
        {
            CauseDamage_Attack();
        }
        yield return _attack_wait2;
        _attack_active = false;
        _attackEffect1.gameObject.SetActive(false);
        _NowState = EnemyState.Chase;
    }

    protected override void OnStateChange_Hit()
    {
        _attack_active = false;
        _attackRange1_anim.SetTrigger(_inactiveHash);
        _attackEffect1.gameObject.SetActive(false);
    }

    protected override void OnHit()
    {
        _attackRange1_anim.ResetTrigger(_inactiveHash);
    }

    protected override void CauseDamage_Attack()
    {
        GameManager.Instance.Player.SufferDamage(_attackPower + UnityEngine.Random.Range(0f, _attackPower * 0.3f), DamageSkin.Player);
    }

    protected override void OnSufferStun()
    {
        _attack_active = false;
        _attackRange1_anim.SetTrigger(_inactiveHash);
        _attackEffect1.gameObject.SetActive(false);
    }

    protected override void CureStun()
    {
        if (_NowState == EnemyState.Idle)
        {
            _afterHit_chasingTime_value = _afterHit_chasingTime;
            OnHit();
            Ready_Chase();
            _NowState = EnemyState.Chase;
        }
    }
}
