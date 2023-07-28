using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Unit_Base
{
    public override float HP
    {
        get => _hp;
        protected set
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
    int _money;
    public int Money
    {
        get => _money;
        set   
        {
            if (_money != value)
            {
                _onChangeMoney?.Invoke(value);
            }
            _money = value;
        }
    }
    public Action<int> _onChangeMoney;

    WaitForSeconds _wait_hitCorotine;
    [Header("플레이어 기본 데이터")]
    public float _pickUpRange = 3.0f;
    public float _coinPickUpRange = 3.0f;

    readonly int _isMoveHash = Animator.StringToHash("IsMove");
    readonly int _isAttackHash = Animator.StringToHash("IsAttack");
    readonly int _activeHash = Animator.StringToHash("Active");

    public float IsStageStart
    {
        get => _isStageStart;
        set => _isStageStart = value;
    }
    bool _save_isMove;
    public Vector2 MoveDir => _moveDir;

    PlayerInputActions _inputActions;

    SlashEffect _slashEffect;
    PlayerAttackRange1 _attackRange1;
    Collider2D _attackRange1_trigger;
    Animator _attackRange1_anim;

    protected override void Awake()
    {
        base.Awake();
        _inputActions = new PlayerInputActions();
        _slashEffect = GetComponentInChildren<SlashEffect>(true);
        _attackRange1 = GetComponentInChildren<PlayerAttackRange1>(true);
        _attackRange1_trigger = _attackRange1.gameObject.GetComponentInChildren<Collider2D>(true);
        _attackRange1_anim = _attackRange1.gameObject.GetComponent<Animator>();

        _isAlive = true;
        HP = MaxHP;


        _wait_hitCorotine = new WaitForSeconds(_hit_blinking_interval);
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += OnMoveInput;
        _inputActions.Player.Move.canceled += OnMoveInput;


        _attackRange1.onMonsterAttack += CauseDamage;
    }

    private void OnDisable()
    {
        _attackRange1.onMonsterAttack -= CauseDamage;
        if (IsStageStart == trueValue_special)
        {
            _inputActions.Player.PickUp.performed -= OnPickUpInput;
            _inputActions.Player.Attack.performed -= OnAttackInput;
        }
        _inputActions.Player.Move.canceled -= OnMoveInput;
        _inputActions.Player.Move.performed -= OnMoveInput;
        _inputActions.Player.Disable();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveDir = context.ReadValue<Vector2>();
        if (_isAttack == falseValue && IsStageStart == trueValue_special)
        {
            HeadTurn();
        }

        if (IsStageStart == trueValue_special)
        {
            _anim.SetBool(_isMoveHash, !context.canceled);
        }
        else
        {
            _save_isMove = !context.canceled;
        }
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

    private void OnPickUpInput(InputAction.CallbackContext _)
    {
        Collider2D[] items = Physics2D.OverlapCircleAll(Position.position, _pickUpRange, LayerMask.GetMask("Item"));
        foreach (Collider2D collider in items)
        {
            DropItem dropItem = collider.GetComponent<DropItem>();
            if (dropItem != null)
            {
                IConsumable iconsume = GameManager.Instance.ItemData[dropItem.ItemCode] as IConsumable;
                if (iconsume == null)
                {
                    if (GameManager.Instance.InvenUI.Inven.AddItem(dropItem.ItemCode, dropItem.ItemAmount, out uint overCount))
                    {
                        dropItem.PickUp();
                    }
                    else
                    {
                        Debug.Log($"아이템 {overCount}개 추가 실패");
                        dropItem.DropItemSetting(dropItem.ItemCode, overCount);
                    }
                }
            }
        }
    }

    private void PickUpCoin()
    {
        Collider2D[] items = Physics2D.OverlapCircleAll(Position.position, _coinPickUpRange, LayerMask.GetMask("Item"));
        foreach (Collider2D collider in items)
        {
            DropItem dropItem = collider.GetComponent<DropItem>();
            if (dropItem != null)
            {
                IConsumable iconsume = GameManager.Instance.ItemData[dropItem.ItemCode] as IConsumable;
                if (iconsume != null)
                {
                    //iconsume.Consume(this);
                    dropItem.Pulled(this);
                }
            }
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
        HeadTurn();
    }

    private void CauseDamage(Monster_Base mob)
    {
        mob.SufferDamage(_attackPower + UnityEngine.Random.Range(0f, _attackPower * 0.3f));
    }

    private IEnumerator HitCoroutine()
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

    protected override void OnSufferDamage(int damage)
    {
        DamageText damageText = SpawnManager_Etc.Instance.GetObject_DamageText(Position.position + Vector3.up * _damageTextHeight);
        damageText.DamageTextSetting(damage.ToString(), DamageSkin.Player);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        PickUpCoin();
    }

    private void Update()
    {
        _hit_invincibleTime_value -= Time.deltaTime;
        _hit_invincibleTime_value %= _hit_invincibleTime;
    }

    public void GameStart()
    {
        HeadTurn();
        _anim.SetBool(_isMoveHash, _save_isMove);
        _inputActions.Player.Attack.performed += OnAttackInput;
        _inputActions.Player.PickUp.performed += OnPickUpInput;
        IsStageStart = trueValue_special;
    }

    private void HeadTurn()
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

}
