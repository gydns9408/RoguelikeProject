using System;
using System.Collections;
using System.Collections.Generic;
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
            _hp = value;
            _hp = Mathf.Clamp(_hp, 0, MaxHP);
            _onChangeHP?.Invoke(MaxHP, _hp);
        }
    }
    public Action<float, float> _onChangeHP;

    public static float trueValue = 0f;
    public static float falseValue = 1f;

    float _isAttack = falseValue;
    float _attackPower = 10;

    public float _moveSpeed = 1f;
    Vector2 _moveDir;

    Rigidbody2D _rigid;

    Animator _anim;
    readonly int _isMoveHash = Animator.StringToHash("IsMove");
    readonly int _isAttackHash = Animator.StringToHash("IsAttack");

    PlayerInputActions _inputActions;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _inputActions = new PlayerInputActions();
        HP = MaxHP;
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += OnMoveInput;
        _inputActions.Player.Move.canceled += OnMoveInput;
        _inputActions.Player.Attack.performed += OnAttackInput;
    }

    private void OnDisable()
    {
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
        }
    }

    public void AttackEnd()
    {
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

    private void FixedUpdate()
    {
        _rigid.transform.position = _rigid.transform.position + Time.fixedDeltaTime * _moveSpeed * _isAttack * (Vector3)_moveDir ;
    }

    public void Test_HPChange(float value)
    {
        HP += value;
    }
}
