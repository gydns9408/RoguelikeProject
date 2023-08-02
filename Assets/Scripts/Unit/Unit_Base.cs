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
    protected float _knock_back_maxSpeed = 8f;
    protected float _knock_back_speed = 0f;
    protected float _knock_back_speed_reduceSpeed = 32f;

    protected const float trueValue = 0f;
    protected const float falseValue = 1f;

    protected bool _isAttack;
    protected float _isKnock_back = falseValue;
    protected float _isKnock_back_reverse => (int)_isKnock_back ^ 0b_1;
    protected float _moveSpeedChangeElement = 1f;
    protected bool _isStageStart;

    protected bool _isAlive = false;
    public float _attackPower = 3;
    public float _defencePower = 0;
    public float _hit_invincibleTime = 0.6f;
    protected float _hit_invincibleTime_value = 0f;
    public float _hit_blinking_interval = 0.1f;

    protected float _stunTime;
    public float StunTime
    {
        get => _stunTime;
        protected set 
        {
            if (value > 0)
            {
                _stunTime = value;
                _stunEffect.gameObject.SetActive(true);
            }
            else if(_stunTime > 0)
            {
                _stunTime = 0;
                _stunEffect.gameObject.SetActive(false);
                CureStun();
            }
            
        }
    }
    [SerializeField]
    protected float _stunTime_resistanceValue = 1f;
    StunEffect _stunEffect;
    protected const float _stunTime_max = 100000f;

    protected float _damageTextHeight = 1f;

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
        _stunEffect = GetComponentInChildren<StunEffect>(true);

        _position = transform.GetChild(0);
        _position_sprite = _position.GetComponent<SpriteRenderer>();
    }
    public void HPChange(float value)
    {
        HP = value;
    }

    public void SufferDamage(float damage)
    {
        if (_hit_invincibleTime_value < 0)
        {
            _hit_invincibleTime_value = _hit_invincibleTime;
            float final_Damage = damage * (1 - (_defencePower / (100f + _defencePower)));
            final_Damage = Mathf.Ceil(final_Damage);
            HPChange(HP - final_Damage);
            OnSufferDamage((int)final_Damage);
        }
    }
    protected virtual void OnSufferDamage(int damage)
    {
    }

    public void SufferStun(float timeDuration)
    {
        if (StunTime < timeDuration * _stunTime_resistanceValue)
        {
            StunTime = timeDuration * _stunTime_resistanceValue;
            OnSufferStun();
        }
    }

    protected virtual void OnSufferStun()
    {
    }

    protected virtual void CureStun()
    { 
    }


    protected virtual void FixedUpdate()
    {
        OnFixedUpdate();
        _rigid.transform.position = _rigid.transform.position + Time.fixedDeltaTime * (_moveSpeed * _isKnock_back + _knock_back_speed * _isKnock_back_reverse) * _moveSpeedChangeElement *(Vector3)_moveDir ;
        _rigid.velocity = Vector2.zero;
    }

    protected virtual void OnFixedUpdate()
    {
    }

    protected virtual void Update()
    {
        _hit_invincibleTime_value -= Time.deltaTime;
        _hit_invincibleTime_value %= _hit_invincibleTime;
        StunTime -= Time.deltaTime;
    }

    protected virtual void LateUpdate()
    {
        _sprite.sortingOrder = (int)(_position.position.y * -100);
    }

}
