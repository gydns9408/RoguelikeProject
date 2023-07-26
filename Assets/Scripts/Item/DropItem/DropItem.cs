using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : PoolObjectShape
{
    ItemCode _itemCode;
    public ItemCode ItemCode
    {
        get => _itemCode;
        private set
        {
            if (_itemCode != value)
            {
                _sprite.sprite = GameManager.Instance.ItemData[value].itemIcon;
            }
            _itemCode = value;
        }
    }

    uint _itemAmount;
    public uint ItemAmount
    {
        get => _itemAmount;
        private set
        {
            _itemAmount = value;
        }
    }

    Transform _position;
    Transform _child;
    float _dropStart_position_y;
    Rigidbody2D _rigid;
    SpriteRenderer _sprite;
    SpriteRenderer _position_sprite;

    bool _isAlive = false;
    public float _moveSpeed = 1.0f;
    public float _pulledSpeed = 6.0f;
    Vector2 _moveDir;
    Vector2 _impulseDir;
    float _impulseSpeed = 8.0f;
    float _currentAchievement = 0;
    float _currentAchievement_increaseSpeed = 3.0f;
    float _currentAchievement_reverse = 1;

    float _currentAchievement2 = 0;
    float _old_sinValue = 0;

    public float _remainTime = 10.0f;

    const float _PI_Reciprocal = 1 / Mathf.PI;

    private void Awake()
    {
        _position = transform.GetChild(0);
        _child = transform.GetChild(1);
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = _child.GetComponent<SpriteRenderer>();
        Transform child = transform.GetChild(2);
        _position_sprite = child.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _sprite.color = Color.white;
        _position_sprite.color = Color.white;
        _moveDir = Vector2.zero;
        _currentAchievement = 0;
        _currentAchievement_reverse = 1;
        _currentAchievement2 = 0;
        StopAllCoroutines();
        StartCoroutine(LifeOver(_remainTime));
        _isAlive = true;
        _impulseDir = Random.insideUnitCircle * _impulseSpeed;
    }

    protected override IEnumerator LifeOver(float remainingTime = 0.0f)
    {
        yield return new WaitForSeconds(remainingTime);
        _isAlive = false;
        StartCoroutine(Disappearing());
    }

    public void DropItemSetting(ItemCode itemCode, uint itemAmount)
    {
        ItemCode = itemCode;
        ItemAmount = itemAmount;
        
    }
    private void FixedUpdate()
    {
        float new_sinValue = Mathf.Sin(_currentAchievement2);
        Vector3 add_position = new Vector3(0, new_sinValue - _old_sinValue, 0);
        _old_sinValue = new_sinValue;
        _rigid.transform.position = _rigid.transform.position + Time.fixedDeltaTime * _moveSpeed * (Vector3)(_moveDir + _impulseDir * _currentAchievement_reverse) + add_position;
        _rigid.velocity = Vector2.zero;
    }

    public void PickUp()
    {
        if (_isAlive)
        {
            _isAlive = false;
            StopAllCoroutines();
            StartCoroutine(PickingUp());
        }
    }

    public void Pulled(Player player)
    {
        if (_isAlive) {
            if ((player.Position.position - _position.position).sqrMagnitude > 0.2f)
            {
                Vector3 moveDir = (player.Position.position - _position.position).normalized;
                _rigid.transform.position = _rigid.transform.position + Time.deltaTime * _pulledSpeed * moveDir;
                _rigid.velocity = Vector2.zero;
            }
            else
            {
                _isAlive = false;
                StopAllCoroutines();
                IConsumable iconsume = GameManager.Instance.ItemData[ItemCode] as IConsumable;
                iconsume.Consume(player);
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator PickingUp()
    {
        Color color = _sprite.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime;
            _sprite.color = color;
            if ((GameManager.Instance.Player.Position.position - _position.position).sqrMagnitude > 0.2f)
            {
                Vector3 moveDir = (GameManager.Instance.Player.Position.position - _position.position).normalized;
                _moveDir = moveDir;
            }
            else
            {
                _moveDir = Vector3.zero;
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }

    private IEnumerator Disappearing()
    {
        Color color = _sprite.color;
        Color color2 = _position_sprite.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime;
            _sprite.color = color;
            color2.a -= Time.deltaTime;
            _position_sprite.color = color2;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    private void Update()
    {
        _currentAchievement += Time.deltaTime * _currentAchievement_increaseSpeed;
        float currentAchievement2_temporary = _currentAchievement2 + Time.deltaTime * _currentAchievement_increaseSpeed;
        _currentAchievement2 = Mathf.Clamp(currentAchievement2_temporary, 0, Mathf.PI);
        _currentAchievement_reverse = Mathf.Lerp(1f, 0f , _PI_Reciprocal * _currentAchievement2);
    }

    private void LateUpdate()
    {
        _sprite.sortingOrder = (int)(_position.position.y * -100);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _impulseDir = Vector2.Reflect(_impulseDir, collision.contacts[0].normal);
    }
}
