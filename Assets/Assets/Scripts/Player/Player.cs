using System;
using System.Collections;
using System.Collections.Generic;
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

    float _attackPower = 10;

    public float _moveSpeed = 1f;
    Vector2 _moveDir;

    Rigidbody2D _rigid;

    Animator _anim;
    readonly int _isMoveHash = Animator.StringToHash("IsMove");
    
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
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.canceled -= OnMoveInput;
        _inputActions.Player.Move.performed -= OnMoveInput;
        _inputActions.Player.Disable();   
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveDir = context.ReadValue<Vector2>();
        if (_moveDir.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (_moveDir.x < 0)
        {
            transform.localScale = Vector3.one;
        }
        _anim.SetBool(_isMoveHash, !context.canceled);
    }

    private void FixedUpdate()
    {
        _rigid.transform.position = _rigid.transform.position + Time.fixedDeltaTime * _moveSpeed * (Vector3)_moveDir;
    }

    public void Test_HPChange(float value)
    {
        HP += value;
    }
}
