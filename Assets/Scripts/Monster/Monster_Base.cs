using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct DropItemInfo
{
    public ItemCode itemcode;
    [Range(0.0f, 1.0f)]
    public float dropProbability;
    public uint dropMaxAmount;
}

public class Monster_Base : Unit_Base
{
    //public float _maxHp = 25f;
    //protected float _hp;
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
                        if (Mathf.Abs(_hp - refine_value) > _maxHp * 0.1f)
                        {
                            _NowState = EnemyState.Hit;
                        }
                        else
                        {
                            _afterHit_chasingTime_value = _afterHit_chasingTime;
                            if (_NowState != EnemyState.Attack)
                            {
                                Ready_Chase();
                                _NowState = EnemyState.Chase;
                            }
                        }
                    }
                    else
                    {
                        _isAlive = false;
                        _coroutine.Die();
                        _NowState = EnemyState.Die;
                    }
                }
                _hp = refine_value;
                _onChangeHP?.Invoke(_hp);
            }
        }
    }

    public Action<float> _onChangeHP;
    public Action _onDisable;

    //protected bool _isAlive = false;
    //protected float _attackPower = 3;
    //protected float _defencePower = 0;

    protected float _spawnTime = 1.0f;
    protected float _spawnTime_reciprocal;
    //public float _moveSpeed = 1.0f;
    protected float _freeMove_notMoveDirChangeTime = 1.0f;
    protected float _freeMove_notMoveDirChangeTime_value = 0f;
    [Header("몬스터 기본 데이터")]
    public float _idleTime = 3.0f;
    public float _dieTime = 1.0f;
    protected float _dieTime_reciprocal;

    protected float _randomGoalArea_minX = -8.5f;
    protected float _randomGoalArea_maxX = 30f;
    protected float _randomGoalArea_minY = -13.5f;
    protected float _randomGoalArea_maxY = 15f;
    protected float _randomGoalArea_X;
    protected float _randomGoalArea_Y;
    protected Vector3 _randomGoalArea_moveDir;

    public float _attack_coolTime = 5.0f;
    protected float _attack_coolTime_value = 0f;
    public float _attack_beforeDelayTime = 0.33f;
    protected WaitForSeconds _attack_wait;
    protected bool _attack_active = false;

    //public float _hit_invincibleTime = 0.6f;
    //protected float _hit_invincibleTime_value = 0f;
    protected WaitForSeconds _hit_wait;
    protected float _afterHit_chasingTime = 0.5f;
    protected float _afterHit_chasingTime_value = 0f;

    //protected Transform _position;
    //public Transform Position => _position;
    protected Monster_HpBar _hpBar;
    public Monster_HpBar HPBar => _hpBar;

    public List<DropItemInfo> _dropItemList;

    Vector3 _hpBar_localScale = new Vector3(1.5f, 0.2f, 1f);
    Vector3 _hpBar_localScale_reverse = new Vector3(-1.5f, 0.2f, 1f);
    //protected SpriteRenderer _sprite;
    //protected SpriteRenderer _position_sprite;
    //protected Collider2D _collider;
    //protected Rigidbody2D _rigid;
    //protected Animator _anim;
    protected DetectRange _detectRange;
    protected MonsterAttackRange1 _attackRange1;
    protected Animator _attackRange1_anim;
    protected MonsterCoroutine _coroutine;

    protected readonly int _isMoveHash = Animator.StringToHash("IsMove");
    protected readonly int _isHitHash = Animator.StringToHash("IsHit");
    protected readonly int _isDieHash = Animator.StringToHash("IsDie");
    protected readonly int _activeHash = Animator.StringToHash("Active");

    protected enum EnemyState
    {
        Spawn,
        FreeMove,
        Idle,
        Chase,
        Attack,
        Hit,
        Die
    }

    protected EnemyState _nowState = EnemyState.Idle;
    protected EnemyState _NowState
    {
        get => _nowState;
        set 
        {
            if(value != _nowState)
            {
                _nowState = value;
                switch (_nowState)
                {
                    case EnemyState.Spawn :
                        _stateFixedUpdate = FixedUpdate_Spawn;
                        break;
                    case EnemyState.FreeMove :
                        _stateFixedUpdate = FixedUpdate_FreeMove;
                        _freeMove_notMoveDirChangeTime_value = _freeMove_notMoveDirChangeTime;
                        _anim.SetBool(_isMoveHash, true);
                        break;
                    case EnemyState.Idle:
                        _stateFixedUpdate = FixedUpdate_Idle;
                        _anim.SetBool(_isMoveHash, false);
                        StopAllCoroutines();
                        StartCoroutine(Idle(_idleTime + UnityEngine.Random.Range(0f, 1f)));
                        break;
                    case EnemyState.Chase:
                        StopAllCoroutines();
                        _stateFixedUpdate = FixedUpdate_Chase;
                        _anim.SetBool(_isMoveHash, true);
                        break;
                    case EnemyState.Attack:
                        _stateFixedUpdate = FixedUpdate_Attack;
                        StopAllCoroutines();
                        StartCoroutine(Attack());
                        break;
                    case EnemyState.Hit:
                        _stateFixedUpdate = FixedUpdate_Hit;
                        _anim.SetBool(_isHitHash, true);
                        StopAllCoroutines();
                        StartCoroutine(Hit());
                        break;
                    case EnemyState.Die:
                        _stateFixedUpdate = FixedUpdate_Die;
                        _anim.SetTrigger(_isDieHash);
                        StopAllCoroutines();
                        StartCoroutine(Die());
                        break;
                }
            }
        }
    }

    Action _stateFixedUpdate = null;

    protected override void Awake()
    {
        base.Awake();
        //_sprite = GetComponent<SpriteRenderer>();
        //_collider = GetComponent<Collider2D>();
        //_rigid = GetComponent<Rigidbody2D>();
        //_anim = GetComponent<Animator>();
        _detectRange = GetComponentInChildren<DetectRange>();
        _attackRange1 = GetComponentInChildren<MonsterAttackRange1>();
        _attackRange1_anim = _attackRange1.gameObject.GetComponent<Animator>();
        _coroutine = GetComponent<MonsterCoroutine>();


        //_position = transform.GetChild(0);
        //_position_sprite = _position.GetComponent<SpriteRenderer>();
        _hpBar = GetComponentInChildren<Monster_HpBar>();

        Color color = _sprite.material.color;
        color.a = 0f;
        _sprite.material.color = color;
        color = _position_sprite.color;
        color.a = 0f;
        _position_sprite.color = color;
        _collider.enabled = false;
        _spawnTime_reciprocal = 1 / _spawnTime;
        _dieTime_reciprocal = 1 / _dieTime;
        _detectRange.Transform = _position;
        _attack_wait = new WaitForSeconds(_attack_beforeDelayTime);
        _attackRange1.onPlayerAttack += AttackRange_DetectPlayer;

        _coroutine.Sprite = _sprite;
        _coroutine.Wait = new WaitForSeconds(_hit_blinking_interval);
        _hit_wait = new WaitForSeconds(_hit_invincibleTime);
    }

    protected void OnEnable()
    {
        
        _isAlive = true;
        HP = _maxHp;
        _NowState = EnemyState.Spawn;
        StopAllCoroutines();
        StartCoroutine(SpawnEffect());
        _attack_coolTime_value = 0;
        _hit_invincibleTime_value = 0;
        _collider.enabled = false;
    }

    protected IEnumerator SpawnEffect()
    {
        Color color = _sprite.material.color;
        Color color2 = _position_sprite.color;
        while (color.a < 1)
        {
            color.a += Time.deltaTime * _spawnTime_reciprocal;
            color2.a += Time.deltaTime * _spawnTime_reciprocal * 0.5f;
            _sprite.material.color = color;
            _position_sprite.color = color2;
            yield return null;
        }
        FreeMoveDirSetting();
        color2.a = 0.5f;
        _position_sprite.color = color2;
        _collider.enabled = true;
        _NowState = EnemyState.FreeMove;
    }

    protected IEnumerator Idle(float idleTime)
    {
        yield return new WaitForSeconds(idleTime);
        FreeMoveDirSetting();
        _NowState = EnemyState.FreeMove;
    }

    protected void FreeMoveDirSetting(bool randomGoalAreaReset = true)
    {
        if (randomGoalAreaReset)
        {
            _randomGoalArea_X = UnityEngine.Random.Range(_randomGoalArea_minX, _randomGoalArea_maxX);
            _randomGoalArea_Y = UnityEngine.Random.Range(_randomGoalArea_minY, _randomGoalArea_maxY);
        }
        _randomGoalArea_moveDir = (new Vector3(_randomGoalArea_X, _randomGoalArea_Y, 0) - _position.position).normalized;
        HeadTurn(_randomGoalArea_moveDir);
    }

    protected void FixedUpdate_Spawn()
    {
        _moveDir = Vector3.zero;
    }

    protected void FixedUpdate_FreeMove()
    {
        if (!_detectRange.DetectPlayer)
        {
            if ((new Vector3(_randomGoalArea_X, _randomGoalArea_Y, 0) - _position.position).sqrMagnitude > 0.1f)
            {
                //_rigid.MovePosition(transform.position + _randomGoalArea_moveDir * _moveSpeed * Time.fixedDeltaTime);
                _moveDir = _randomGoalArea_moveDir;
            }
            else
            {
                _NowState = EnemyState.Idle;
            }
        }
        else
        {
            _NowState = EnemyState.Chase;
        }

    }

    protected void FixedUpdate_Idle()
    {
        if (_detectRange.DetectPlayer)
        {
            _NowState = EnemyState.Chase;
        }
        else
        {
            _moveDir = Vector3.zero;
        }
    }

    protected void FixedUpdate_Chase()
    {
        if (_detectRange.DetectPlayer|| _afterHit_chasingTime_value > 0)
        {
            if (_attack_coolTime_value < 0)
            {
                if (_attackRange1.DetectPlayer)
                {
                    _attack_coolTime_value = _attack_coolTime;
                    _NowState = EnemyState.Attack;
                }
                else
                {
                    Chasing();
                }
            }
            else 
            {
                Chasing();
            }
        }
        else
        {
            _NowState = EnemyState.Idle;
        }
    }

    protected void Chasing()
    {
        if ((GameManager.Instance.Player.Position.position - _position.position).sqrMagnitude > 0.2f)
        {
            Vector3 moveDir = (GameManager.Instance.Player.Position.position - _position.position).normalized;
            HeadTurn(moveDir);
            //_rigid.MovePosition(transform.position + moveDir * _moveSpeed * Time.fixedDeltaTime);
            _moveDir = moveDir;
        }
    }

    protected void Ready_Chase()
    {
        Vector3 goalDir = GameManager.Instance.Player.Position.position - _position.position;
        HeadTurn(goalDir);
    }

    protected void HeadTurn(Vector3 moveDir)
    {
        if (moveDir.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            if (_hpBar.transform.localScale.x > 0)
            {
                _hpBar.transform.localScale = _hpBar_localScale_reverse;
            }
        }
        else if (moveDir.x < 0)
        {
            transform.localScale = Vector3.one;
            if (_hpBar.transform.localScale.x < 0)
            {
                _hpBar.transform.localScale = _hpBar_localScale;
            }
        }
    }

    protected void FixedUpdate_Attack()
    {
        _moveDir = Vector3.zero;
    }

    protected void FixedUpdate_Hit()
    {
        _moveDir = Vector3.zero;
    }

    protected void FixedUpdate_Die()
    {
        _moveDir = Vector3.zero;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionSomething(collision.gameObject);
    }

    protected void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionSomething(collision.gameObject);
    }

    protected void OnCollisionSomething(GameObject something)
    {
        if (something.CompareTag("Enemy") || something.CompareTag("Wall"))
        {
            if (_NowState == EnemyState.FreeMove && _freeMove_notMoveDirChangeTime_value < 0)
            {
                _NowState = EnemyState.Idle;
            }
        }
    }


    protected virtual IEnumerator Attack()
    {
        _anim.SetBool(_isMoveHash, false);
        _attackRange1_anim.SetTrigger(_activeHash);
        yield return _attack_wait;

    }

    private void AttackRange_DetectPlayer()
    {
        if (_attack_active)
        {
            GameManager.Instance.Player.SufferDamage(_attackPower + UnityEngine.Random.Range(0f, _attackPower * 0.1f));
        }
    }

    //public void SufferDamage(float damage)
    //{
    //    if (_hit_invincibleTime_value < 0)
    //    {
    //        _hit_invincibleTime_value = _hit_invincibleTime;
    //        float fianl_Damage = damage * (1 - (_defencePower / (100f + _defencePower)));
    //        HP -= fianl_Damage;

    //    }
    //}

    protected IEnumerator Hit()
    {
        yield return _hit_wait;
        _anim.SetBool(_isHitHash, false);
        _afterHit_chasingTime_value = _afterHit_chasingTime;
        Ready_Chase();
        _NowState = EnemyState.Chase;
    }

    protected IEnumerator Die()
    {
        _collider.enabled = false;
        Color color = _sprite.material.color;
        Color color2 = _position_sprite.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime * _dieTime_reciprocal;
            color2.a -= Time.deltaTime * _dieTime_reciprocal * 0.5f;
            _sprite.material.color = color;
            _position_sprite.color = color2;
            yield return null;
        }
        color2.a = 0f;
        _position_sprite.color = color2;
        DropPlunder();
        gameObject.SetActive(false);
    }

    private void DropPlunder()
    {
        foreach (var item in _dropItemList)
        {
            float randomValue = UnityEngine.Random.value;
            if (item.dropProbability > randomValue)
            {
                int randomValue2 = UnityEngine.Random.Range(1, (int)item.dropMaxAmount + 1);
                DropItem dropItem = SpawnManager_Etc.Instance.GetObject_DropItem(item.itemcode, (uint)randomValue2);
                dropItem.transform.position = _position.position;
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _stateFixedUpdate();
    }

    protected void Update()
    {
        _attack_coolTime_value -= Time.deltaTime;
        _attack_coolTime_value %= _attack_coolTime;
        _hit_invincibleTime_value -= Time.deltaTime;
        _hit_invincibleTime_value %= _hit_invincibleTime;
        _afterHit_chasingTime_value -= Time.deltaTime;
        _afterHit_chasingTime_value %= _afterHit_chasingTime;
        _freeMove_notMoveDirChangeTime_value -= Time.deltaTime;
        _freeMove_notMoveDirChangeTime_value %= _freeMove_notMoveDirChangeTime;
    }



    protected virtual void OnDisable()
    {
        _onDisable?.Invoke();
    }
}
