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
            GameManager.Instance.Player.SufferDamage(_attackPower + UnityEngine.Random.Range(0f, _attackPower * 0.1f));
        }
        yield return _attack_wait2;
        _attack_active = false;
        _attackEffect1.gameObject.SetActive(false);
        _NowState = EnemyState.Chase;
    }
}
