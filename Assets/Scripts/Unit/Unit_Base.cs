using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Base : MonoBehaviour
{
    [Header("유닛 기본 데이터")]
    [SerializeField]
    protected float _maxHp = 25f;
    public float MaxHP => _maxHp;
    protected float _hp;
    public virtual float HP
    {
        get => _hp;
        protected set
        {
            if (_isAlive)
            {
                float refine_value = Mathf.Clamp(value, 0, _maxHp);
                if (refine_value < _hp && refine_value <= 0)
                {
                    _isAlive = false;
                }
                _hp = refine_value;
            }
        }
    }

    public float _moveSpeed = 1f;
    protected Vector2 _moveDir;

    protected static float trueValue = 0f;
    protected static float falseValue = 1f;

    protected float _isAttack = falseValue;

    protected bool _isAlive = false;
    public float _attackPower = 3;
    public float _defencePower = 0;
    public float _hit_invincibleTime = 0.6f;
    protected float _hit_invincibleTime_value = 0f;
    public float _hit_blinking_interval = 0.1f;

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

        //Color color = _sprite.material.color;
        //color.a = 0f;
        //_sprite.material.color = color;
        //color = _position_sprite.color;
        //color.a = 0f;
        //_position_sprite.color = color;
        //_collider.enabled = false;
    }
    public void HPChange(float value)
    {
        HP += value;
    }

    public void SufferDamage(float damage)
    {
        if (_hit_invincibleTime_value < 0)
        {
            _hit_invincibleTime_value = _hit_invincibleTime;
            float final_Damage = damage * (1 - (_defencePower / (100f + _defencePower)));
            HPChange(-final_Damage);
        }
    }



    protected virtual void FixedUpdate()
    {
        _rigid.transform.position = _rigid.transform.position + Time.fixedDeltaTime * _moveSpeed * _isAttack * (Vector3)_moveDir;
        _rigid.velocity = Vector2.zero;
    }

    protected void LateUpdate()
    {
        _sprite.sortingOrder = (int)(_position.position.y * -100);
    }

}
