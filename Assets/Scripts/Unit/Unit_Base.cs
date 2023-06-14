using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Base : MonoBehaviour
{
    public float _maxHp = 25f;
    protected float _hp;
    protected float HP
    {
        get => _hp;
        set
        {
            if (_isAlive)
            {
                float refine_value = Mathf.Clamp(value, 0, _maxHp);
                if (refine_value < _hp)
                {
                    if (refine_value > 0)
                    {
                        //_coroutine.Hit();
                        if (Mathf.Abs(_hp - refine_value) > _maxHp * 0.1f)
                        {
                            //_NowState = EnemyState.Hit;
                        }
                        else
                        {
                            //_afterHit_chasingTime_value = _afterHit_chasingTime;
                            //if (_NowState != EnemyState.Attack)
                            //{
                            //    Ready_Chase();
                            //    _NowState = EnemyState.Chase;
                            //}
                        }
                    }
                    else
                    {
                        _isAlive = false;
                        //_coroutine.Die();
                        //_NowState = EnemyState.Die;
                    }
                }
                _hp = refine_value;
                _onChangeHP?.Invoke(_hp);
            }
        }
    }

    protected bool _isAlive = false;
    protected float _attackPower = 3;
    protected float _defencePower = 0;
    public float _hit_invincibleTime = 0.6f;
    protected float _hit_invincibleTime_value = 0f;

    public Action<float> _onChangeHP;

    protected Transform _position;
    public Transform Position => _position;

    protected SpriteRenderer _sprite;
    protected SpriteRenderer _position_sprite;
    protected Collider2D _collider;
    protected Rigidbody2D _rigid;
    protected Animator _anim;

    protected virtual void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();


        _position = transform.GetChild(0);
        _position_sprite = _position.GetComponent<SpriteRenderer>();

        Color color = _sprite.material.color;
        color.a = 0f;
        _sprite.material.color = color;
        color = _position_sprite.color;
        color.a = 0f;
        _position_sprite.color = color;
        _collider.enabled = false;
    }

    public void SufferDamage(float damage)
    {
        if (_hit_invincibleTime_value < 0)
        {
            _hit_invincibleTime_value = _hit_invincibleTime;
            float fianl_Damage = damage * (1 - (_defencePower / (100f + _defencePower)));
            HP -= fianl_Damage;

        }
    }
}
