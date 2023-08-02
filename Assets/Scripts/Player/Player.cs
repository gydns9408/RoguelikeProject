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
                        _coroutine.Hit();
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

    [Header("플레이어 기본 데이터")]
    [SerializeField]
    float _maxMp = 25f;
    public float MaxMP => _maxMp;
    float _mp;
    public float MP
    {
        get => _mp;
        protected set
        {
            if (_isAlive)
            {
                float refine_value = Mathf.Clamp(value, 0, _maxMp);
                _mp = refine_value;
                _onChangeMP?.Invoke(MaxMP, _mp);
            }
        }
    }
    public Action<float, float> _onChangeMP;
    [SerializeField]
    float _mpRecoverCoroutine_recoverAmount = 10f;
    public float MpRecoverCoroutine_RecoverAmount => _mpRecoverCoroutine_recoverAmount;

    [SerializeField]
    float _criticalChance = 0f;
    public float CriticalChance => _criticalChance;
    [SerializeField]
    float _criticalDamage = 2f;
    public float CriticalDamage => _criticalDamage;

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


    public float Hit_InvincibleTime_Value => _hit_invincibleTime_value;
    public float Skill1_CoolTime = 2.0f;
    public float Skill1_MPCost = 10.0f;
    float _skill1_coolTime_value = 0;
    public float Skill1_CoolTime_Value => _skill1_coolTime_value;
    float _isSkill1On = falseValue;
    float _skill1_dashMaxSpeed = 8.0f;
    [SerializeField]
    float _skill1_CauseStunTime = 2.0f;
    float _skill1_dashSpeed = 1.0f;
    float _skill1_dash_currentAchievement = 1f;
    float _skill1_dash_currentAchievement_increaseSpeed = 3f;
    public float Skill2_CoolTime = 10.0f;
    public float Skill2_MPCost = 40.0f;
    float _skill2_coolTime_value = 0;
    public float Skill2_CoolTime_Value => _skill2_coolTime_value;
    public float Skill3_CoolTime = 15.0f;
    public float Skill3_MPCost = 45.0f;
    float _skill3_coolTime_value = 0;
    public float Skill3_CoolTime_Value => _skill3_coolTime_value;
    Skill3_Ball _skill3_ball;

    WaitForSeconds _wait_skill1;
    public float _pickUpRange = 3.0f;
    public float _coinPickUpRange = 3.0f;

    readonly int _isMoveHash = Animator.StringToHash("IsMove");
    readonly int _attackHash = Animator.StringToHash("Attack");
    readonly int _attack2Hash = Animator.StringToHash("Attack2");
    readonly int _attack3Hash = Animator.StringToHash("Attack3");
    readonly int _attack4Hash = Animator.StringToHash("Attack4");
    readonly int _stunHash = Animator.StringToHash("Stun");
    readonly int _activeHash = Animator.StringToHash("Active");
    readonly int _inactiveHash = Animator.StringToHash("Inactive");

    public bool IsStageStart
    {
        get => _isStageStart;
        set => _isStageStart = value;
    }
    bool _save_isMove;
    public Vector2 MoveDir => _keyDir;
    Vector2 _keyDir;

    PlayerInputActions _inputActions;

    TemporaryEffect _slashEffect;
    TemporaryEffect _dashEffect;
    TemporaryEffect _skill1Effect;
    TemporaryEffect _skill1ChargeEffect;
    TemporaryEffect _skill2Effect;
    SpriteRenderer _skill2Effect_sprite;
    TemporaryEffect _skill3ChargeEffect;
    PlayerAttackRange_Base _attackRange1;
    PlayerAttackRange_Base _attackRange2;
    PlayerAttackRange_Base _attackRange3;
    Collider2D _attackRange1_trigger;
    Collider2D _attackRange2_trigger;
    Collider2D _attackRange3_trigger;
    Animator _attackRange1_anim;
    Animator _attackRange2_anim;
    Animator _attackRange3_anim;
    UnitCoroutine_Player _coroutine;

    protected override void Awake()
    {
        base.Awake();
        _inputActions = new PlayerInputActions();
        Transform child = transform.GetChild(2);
        _slashEffect = child.GetComponentInChildren<TemporaryEffect>(true);
        child = transform.GetChild(3);
        _dashEffect = child.GetComponentInChildren<TemporaryEffect>(true);
        child = transform.GetChild(4);
        _skill1Effect = child.GetComponentInChildren<TemporaryEffect>(true);
        child = transform.GetChild(5);
        _skill1ChargeEffect = child.GetComponentInChildren<TemporaryEffect>(true);
        child = transform.GetChild(1);
        _attackRange1 = child.GetComponent<PlayerAttackRange_Base>();
        _attackRange1_trigger = _attackRange1.gameObject.GetComponentInChildren<Collider2D>(true);
        _attackRange1_anim = _attackRange1.gameObject.GetComponent<Animator>();
        child = transform.GetChild(6);
        _attackRange2 = child.GetComponent<PlayerAttackRange_Base>();
        _attackRange2_trigger = _attackRange2.gameObject.GetComponentInChildren<Collider2D>(true);
        _attackRange2_anim = _attackRange2.gameObject.GetComponent<Animator>();
        child = transform.GetChild(8);
        _skill2Effect = child.GetComponentInChildren<TemporaryEffect>(true);
        _skill2Effect_sprite = child.GetComponentInChildren<SpriteRenderer>(true);
        child = transform.GetChild(9);
        _attackRange3 = child.GetComponent<PlayerAttackRange_Base>();
        _attackRange3_trigger = _attackRange3.gameObject.GetComponentInChildren<Collider2D>(true);
        _attackRange3_anim = _attackRange3.gameObject.GetComponent<Animator>();
        child = transform.GetChild(10);
        _skill3ChargeEffect = child.GetComponentInChildren<TemporaryEffect>(true);
        _coroutine = GetComponent<UnitCoroutine_Player>();
        _coroutine.Owner = this;
        _coroutine.Sprite = _sprite;
        _coroutine.Wait = new WaitForSeconds(_hit_blinking_interval);
        _coroutine.MPRecoverCoroutineStart();

        _isAlive = true;

        _wait_skill1 = new WaitForSeconds(1 / _skill1_dash_currentAchievement_increaseSpeed);
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += OnMoveInput;
        _inputActions.Player.Move.canceled += OnMoveInput;


        _attackRange1.onMonsterAttack += CauseDamage_Attack1;
        _attackRange2.onMonsterAttack += CauseDamage_Skill1;
        _attackRange3.onMonsterAttack += CauseDamage_Skill2;
        _skill3_ball = FindObjectOfType<Skill3_Ball>(true);
        _skill3_ball.onMonsterAttack += CauseDamage_Skill3;
    }

    private void OnDisable()
    {

        _attackRange3.onMonsterAttack -= CauseDamage_Skill2;
        _attackRange2.onMonsterAttack -= CauseDamage_Skill1;
        _attackRange1.onMonsterAttack -= CauseDamage_Attack1;
        if (IsStageStart)
        {
            _inputActions.Player.Skill3.performed -= OnSkill3Input;
            _inputActions.Player.Skill2.performed -= OnSkill2Input;
            _inputActions.Player.Skill1.performed -= OnSkill1Input;
            _inputActions.Player.PickUp.performed -= OnPickUpInput;
            _inputActions.Player.Attack.performed -= OnAttackInput;
        }
        _inputActions.Player.Move.canceled -= OnMoveInput;
        _inputActions.Player.Move.performed -= OnMoveInput;
        _inputActions.Player.Disable();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        _keyDir = context.ReadValue<Vector2>();

        if (!_isAttack && IsStageStart && StunTime <= 0)
        {
            _moveDir = _keyDir;
            HeadTurn();
        }

        _save_isMove = !context.canceled;

        if (IsStageStart && StunTime <= 0)
        {
            _anim.SetBool(_isMoveHash, _save_isMove);
        }
    }

    private void OnAttackInput(InputAction.CallbackContext _)
    {
        if (!_isAttack && StunTime <= 0)
        {
            _moveDir = Vector2.zero;
            _isAttack = true;
            _anim.SetTrigger(_attackHash);
            _attackRange1_anim.SetTrigger(_activeHash);
        }
    }

    private void OnPickUpInput(InputAction.CallbackContext _)
    {
        if (StunTime <= 0)
        {
            Collider2D[] items = Physics2D.OverlapCircleAll(Position.position, _pickUpRange, LayerMask.GetMask("Item"));
            foreach (Collider2D collider in items)
            {
                DropItem dropItem = collider.GetComponent<DropItem>();
                if (dropItem != null)
                {
                    IConsumable iconsume = GameManager.Instance.ItemData[dropItem.ItemCode] as IConsumable;
                    if (iconsume == null && dropItem.IsGettable && dropItem.IsAlive)
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
    }

    private void PickUpCoin()
    {
        if (StunTime <= 0)
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
                        dropItem.Pulled(this);
                    }
                }
            }
        }
    }

    private void CreateSlashEffect()
    {
        if (StunTime <= 0)
        {
            _slashEffect.gameObject.transform.position = transform.position;
            _slashEffect.gameObject.SetActive(true);
            _attackRange1_trigger.enabled = true;
        }
    }

    private void CreateSkill1Effect()
    {
        if (StunTime <= 0)
        {
            _skill1Effect.gameObject.SetActive(true);
            _attackRange2_trigger.enabled = true;
        }
    }

    public void AttackEnd()
    {
        if (StunTime <= 0)
        {
            _attackRange1_trigger.enabled = false;
            _isAttack = false;
            _moveDir = _keyDir;
            HeadTurn();
        }
    }

    private void Skill1ColliderOff()
    {
        if (StunTime <= 0)
        {
            _attackRange2_trigger.enabled = false;
        }
    }

    public void Skill1End()
    {
        if (StunTime <= 0)
        {
            _isAttack = false;
            _isSkill1On = falseValue;
            _moveDir = _keyDir;
            HeadTurn();
        }
    }

    private void OnSkill1Input(InputAction.CallbackContext _)
    {
        if (!_isAttack && _skill1_coolTime_value <= 0 && StunTime <= 0)
        {
            if (MP >= Skill1_MPCost)
            {
                _isAttack = true;
                _isSkill1On = trueValue;
                _skill1_coolTime_value = Skill1_CoolTime;
                MP -= Skill1_MPCost;
                _skill1_dash_currentAchievement = 0f;
                if (transform.localScale.x > 0)
                {
                    _moveDir = Vector2.left;
                }
                else
                {
                    _moveDir = Vector2.right;
                }
                _anim.speed = 2.0f;
                _anim.SetBool(_isMoveHash, true);
                _dashEffect.gameObject.SetActive(true);
                StartCoroutine(Skill1ing());
            }
        }
    }



    private IEnumerator Skill1ing()
    {
        yield return _wait_skill1;
        _anim.speed = 1.0f;
        _skill1ChargeEffect.gameObject.SetActive(true);
        _anim.SetTrigger(_attack2Hash);
        _attackRange2_anim.SetTrigger(_activeHash);
    }

    private void OnSkill2Input(InputAction.CallbackContext _)
    {
        if (!_isAttack && _skill2_coolTime_value <= 0 && StunTime <= 0)
        {
            if (MP >= Skill2_MPCost)
            {
                _isAttack = true;
                _moveDir = Vector2.zero;
                _skill2_coolTime_value = Skill2_CoolTime;
                MP -= Skill2_MPCost;
                _anim.SetTrigger(_attack3Hash);
                _skill2Effect.gameObject.SetActive(true);
                _attackRange3_anim.SetTrigger(_activeHash);
            }
        }
    }

    private void OnSkill3Input(InputAction.CallbackContext _)
    {
        if (!_isAttack && _skill3_coolTime_value <= 0 && StunTime <= 0)
        {
            if (MP >= Skill3_MPCost)
            {
                _isAttack = true;
                _moveDir = Vector2.zero;
                _skill3_coolTime_value = Skill3_CoolTime;
                MP -= Skill3_MPCost;
                _anim.SetTrigger(_attack4Hash);
                _skill3ChargeEffect.gameObject.SetActive(true);
            }
        }
    }

    private void Skill2ColliderOn()
    {
        if (StunTime <= 0)
        {
            _attackRange3_trigger.enabled = true;
        }
    }

    private void Skill2ColliderOff()
    {
        if (StunTime <= 0)
        {
            _attackRange3_trigger.enabled = false;
        }
    }

    public void Skill2End()
    {
        if (StunTime <= 0)
        {
            _isAttack = false;
            _moveDir = _keyDir;
            HeadTurn();
        }
    }

    private void CreateSwordKi()
    {
        if (StunTime <= 0)
        {
            _skill3_ball.gameObject.SetActive(true);
            if (transform.localScale.x > 0)
            {
                _skill3_ball.Summon(Vector2.left);
            }
            else
            {
                _skill3_ball.Summon(Vector2.right);
            }
        }
    }

    private void CauseDamage_Attack1(Monster_Base mob)
    {
        SettingAttackTarget_Position(mob, Position.position);
        mob.SufferDamage(_attackPower + UnityEngine.Random.Range(0f, _attackPower * 0.3f), DamageSkin.Default);
    }

    private void CauseDamage_Skill1(Monster_Base mob)
    {
        SettingAttackTarget_Position(mob, Position.position);
        mob.SufferStun(_skill1_CauseStunTime);
        mob.SufferDamage(_attackPower * 1.5f + UnityEngine.Random.Range(0f, _attackPower * 0.45f), DamageSkin.Default);
    }

    private void CauseDamage_Skill2(Monster_Base mob)
    {
        SettingAttackTarget_Position(mob, Position.position);
        mob.SufferDamage(_attackPower * 2.5f + UnityEngine.Random.Range(0f, _attackPower * 0.75f), DamageSkin.Default);
    }

    private void CauseDamage_Skill3(Monster_Base mob)
    {
        SettingAttackTarget_Position(mob, _skill3_ball.Position.position);
        mob.SufferDamage(_attackPower * 4f + UnityEngine.Random.Range(0f, _attackPower * 1.2f), DamageSkin.Default);
    }

    private void SettingAttackTarget_Position(Monster_Base mob, Vector3 targetPos)
    {
        mob.AttackTarget_Position = targetPos;
    }

    protected override void OnSufferDamage(int damage, DamageSkin damageSkin)
    {
        DamageText damageText = SpawnManager_Etc.Instance.GetObject_DamageText(Position.position + Vector3.up * _damageTextHeight);
        damageText.DamageTextSetting(damage.ToString(), damageSkin);
    }

    protected override void OnSufferStun()
    {
        StopAllCoroutines();
        _moveDir = Vector2.zero;
        _anim.SetBool(_isMoveHash, false);
        _anim.SetTrigger(_stunHash);

        _attackRange1_anim.SetTrigger(_inactiveHash);
        _attackRange1_trigger.enabled = false;
        _attackRange2_anim.SetTrigger(_inactiveHash);
        _attackRange2_trigger.enabled = false;
        _attackRange3_anim.SetTrigger(_inactiveHash);
        _attackRange3_trigger.enabled = false;

        _isSkill1On = falseValue;
        _anim.speed = 1.0f;
        _skill1_dash_currentAchievement = 1f;

        _slashEffect.gameObject.SetActive(false);
        _dashEffect.gameObject.SetActive(false);
        _skill1Effect.gameObject.SetActive(false);
        _skill1ChargeEffect.gameObject.SetActive(false);
        _skill2Effect.gameObject.SetActive(false);
        _skill3ChargeEffect.gameObject.SetActive(false);
        _isAttack = false;
    }

    protected override void CureStun()
    {
        _anim.ResetTrigger(_stunHash);
        _attackRange1_anim.ResetTrigger(_inactiveHash);
        _attackRange2_anim.ResetTrigger(_inactiveHash);
        _attackRange3_anim.ResetTrigger(_inactiveHash);

        _moveDir = _keyDir;
        HeadTurn();
        _anim.SetBool(_isMoveHash, _save_isMove);
    }

    public void MPChange(float value)
    {
        MP = value;
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
    public void GameStart()
    {
        _moveDir = _keyDir;
        HeadTurn();
        _anim.SetBool(_isMoveHash, _save_isMove);
        _inputActions.Player.Attack.performed += OnAttackInput;
        _inputActions.Player.PickUp.performed += OnPickUpInput;
        _inputActions.Player.Skill1.performed += OnSkill1Input;
        _inputActions.Player.Skill2.performed += OnSkill2Input;
        _inputActions.Player.Skill3.performed += OnSkill3Input;
        IsStageStart = true;
    }

    protected override void OnFixedUpdate()
    {
        PickUpCoin();
        _moveSpeedChangeElement = _skill1_dashSpeed + _isSkill1On;
    }

    protected override void Update()
    {
        base.Update();
        float skill1_coolTime_value = _skill1_coolTime_value - Time.deltaTime;
        _skill1_coolTime_value = Mathf.Clamp(skill1_coolTime_value, 0f, Skill1_CoolTime);
        float skill2_coolTime_value = _skill2_coolTime_value - Time.deltaTime;
        _skill2_coolTime_value = Mathf.Clamp(skill2_coolTime_value, 0f, Skill2_CoolTime);
        float skill3_coolTime_value = _skill3_coolTime_value - Time.deltaTime;
        _skill3_coolTime_value = Mathf.Clamp(skill3_coolTime_value, 0f, Skill3_CoolTime);
        float skill1_dash_currentAchievement = _skill1_dash_currentAchievement + Time.deltaTime * _skill1_dash_currentAchievement_increaseSpeed;
        skill1_dash_currentAchievement = Mathf.Clamp(skill1_dash_currentAchievement, 0f, 1f);
        _skill1_dash_currentAchievement = skill1_dash_currentAchievement;
        _skill1_dashSpeed = Mathf.Lerp(_skill1_dashMaxSpeed, 0, _skill1_dash_currentAchievement);
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        _skill2Effect_sprite.sortingOrder = (int)(_position.position.y * -100 - 1);
    }

}
