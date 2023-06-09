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
    Rigidbody2D _rigid;
    SpriteRenderer _sprite;

    bool _isAlive = false;
    public float _moveSpeed = 1.0f;
    Vector2 _moveDir;

    public float _remainTime = 10.0f;
    private void Awake()
    {
        _position = transform.GetChild(0);
        _child = transform.GetChild(1);
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = _child.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _sprite.color = Color.white;
        _moveDir = Vector2.zero;
        StopAllCoroutines();
        StartCoroutine(LifeOver(_remainTime));
        _isAlive = true;
    }

    protected override IEnumerator LifeOver(float remainingTime = 0.0f)
    {
        yield return new WaitForSeconds(remainingTime);
        _isAlive = false;
        StartCoroutine(Disappearing());
    }

    public void DropItemSetting(ItemCode itemCode, uint itemAmount)
    {
        _itemCode = itemCode;
        _itemAmount = itemAmount;
        
    }
    private void FixedUpdate()
    {
        _rigid.transform.position = _rigid.transform.position + Time.fixedDeltaTime * _moveSpeed * (Vector3)_moveDir;
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
            yield return null;
        }
        gameObject.SetActive(false);
    }

    private IEnumerator Disappearing()
    {
        Color color = _sprite.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime;
            _sprite.color = color;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        _sprite.sortingOrder = (int)(_position.position.y * -100);
    }
}
