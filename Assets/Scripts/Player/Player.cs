using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    float _maxHp = 100;
    public float MaxHP => _maxHp;
    float _hp = 100;
    public float HP
    {
        get => _hp;
        private set
        {
            if (_isAlive)
            {
                float refine_value = Mathf.Clamp(value, 0, _maxHp);
                if (refine_value < _hp)
                {
                    if (refine_value > 0)
                    {
                        StartCoroutine(HitCoroutine());
                    }
                    else
                    {
                        
                    }
                }
                _hp = refine_value;
                _onChangeHP?.Invoke(MaxHP, _hp);
            }
        }
    }
    public Action<float, float> _onChangeHP;
    bool _isAlive = true;

    public float _hit_invincibleTime = 1.5f; 
    float _hit_invincibleTime_value = 0f;
    public float _hit_blinking_interval = 0.1f;
    WaitForSeconds _wait_hitCorotine;

    public static float trueValue = 0f;
    public static float falseValue = 1f;

    float _isAttack = falseValue;
    float _attackPower = 10;
    float _defencePower = 3;

    public float _moveSpeed = 1f;
    Vector2 _moveDir;

    Rigidbody2D _rigid;
    

    Animator _anim;
    readonly int _isMoveHash = Animator.StringToHash("IsMove");
    readonly int _isAttackHash = Animator.StringToHash("IsAttack");
    readonly int _activeHash = Animator.StringToHash("Active");

    PlayerInputActions _inputActions;

    SpriteRenderer _sprite;
    SlashEffect _slashEffect;
    PlayerAttackRange1 _attackRange1;
    Collider2D _attackRange1_trigger;
    Animator _attackRange1_anim;
    Transform _position;
    public Transform Position => _position;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _inputActions = new PlayerInputActions();
        _sprite = GetComponent<SpriteRenderer>();
        _slashEffect = GetComponentInChildren<SlashEffect>(true);
        _attackRange1 = GetComponentInChildren<PlayerAttackRange1>(true);
        _attackRange1_trigger = _attackRange1.gameObject.GetComponentInChildren<Collider2D>(true);
        _attackRange1_anim = _attackRange1.gameObject.GetComponent<Animator>();

        _position = transform.GetChild(0);
        HP = MaxHP;

        _wait_hitCorotine = new WaitForSeconds(_hit_blinking_interval);
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += OnMoveInput;
        _inputActions.Player.Move.canceled += OnMoveInput;
        _inputActions.Player.Attack.performed += OnAttackInput;

        _attackRange1.onMonsterAttack += CauseDamage;
    }

    private void OnDisable()
    {
        _attackRange1.onMonsterAttack -= CauseDamage;

        _inputActions.Player.Attack.performed -= OnAttackInput;
        _inputActions.Player.Move.canceled -= OnMoveInput;
        _inputActions.Player.Move.performed -= OnMoveInput;
        _inputActions.Player.Disable();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveDir = context.ReadValue<Vector2>();
        if (_isAttack == falseValue)
        {
            if (_moveDir.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (_moveDir.x < 0)
            {
                transform.localScale = Vector3.one;
            }
        }
        _anim.SetBool(_isMoveHash, !context.canceled);
    }

    private void OnAttackInput(InputAction.CallbackContext _)
    {
        if (_isAttack == falseValue)
        {
            _isAttack = trueValue;
            _anim.SetTrigger(_isAttackHash);
            _attackRange1_anim.SetTrigger(_activeHash);
        }
    }

    private void CreateSlashEffect()
    {
        _slashEffect.gameObject.transform.position = transform.position;
        _slashEffect.gameObject.SetActive(true);
        _attackRange1_trigger.enabled = true;
    }

    public void AttackStart()
    {
        _attackRange1.gameObject.SetActive(true);
    }

    public void AttackEnd()
    {
        _attackRange1_trigger.enabled = false;
        _isAttack = falseValue;
        if (_moveDir.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (_moveDir.x < 0)
        {
            transform.localScale = Vector3.one;
        }
    }

    private void CauseDamage(Monster_Base mob)
    {
        mob.SufferDamage(_attackPower + UnityEngine.Random.Range(0f, _attackPower * 0.1f));
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

    IEnumerator HitCoroutine()
    {
        Color color = _sprite.material.color;
        while (_hit_invincibleTime_value > 0 )
        {
            color.a = 0.5f;
            _sprite.material.color = color;
            yield return _wait_hitCorotine;
            if (_hit_invincibleTime_value <= 0)
            {
                break;
            }
            color.a = 1f;
            _sprite.material.color = color;
            yield return _wait_hitCorotine;
        }
        color.a = 1f;
        _sprite.material.color = color;
    }

    private void Update()
    {
        _hit_invincibleTime_value -= Time.deltaTime;
        _hit_invincibleTime_value %= _hit_invincibleTime;
    }

    private void LateUpdate()
    {
        _sprite.sortingOrder = (int)(_position.position.y * -100);
    }

    private void FixedUpdate()
    {
        _rigid.transform.position = _rigid.transform.position + Time.fixedDeltaTime * _moveSpeed * _isAttack * (Vector3)_moveDir;
        _rigid.velocity = Vector2.zero;
    }

    public void Test_HPChange(float value)
    {
        HP += value;
    }
}