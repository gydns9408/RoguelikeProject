using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3_Ball : MonoBehaviour
{
    Transform _position;
    public Transform Position => _position;
    Vector2 _moveDir;
    [SerializeField]
    float _moveSpeed = 5f;
    SpriteRenderer _sprite;
    Collider2D _collider;
    Rigidbody2D _rigid;

    public Action<Monster_Base> onMonsterAttack;

    private void Awake()
    {
        _position = transform.GetChild(0);
        Transform child = transform.GetChild(1);
        _sprite = child.GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider.enabled = false;
        gameObject.SetActive(false);
    }

    public void Summon(Vector2 moveDir)
    {
        if (moveDir.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveDir.x < 0)
        {
            transform.localScale = Vector3.one;
        }
        Vector3 moveAmount = GameManager.Instance.Player.Position.position - _position.position;
        _rigid.transform.position = _rigid.transform.position + moveAmount;
        _moveDir = moveDir;
        _collider.enabled = true;
    }
    

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Monster_Base mob = collision.gameObject.GetComponent<Monster_Base>();
            if (mob != null)
            {
                onMonsterAttack?.Invoke(mob);
            }
        }
        else
        {
            _collider.enabled = false;
            gameObject.SetActive(false);
        }
    }


    private void FixedUpdate()
    {
        _rigid.transform.position = _rigid.transform.position + Time.fixedDeltaTime * _moveSpeed * (Vector3)_moveDir;
        _rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        _sprite.sortingOrder = (int)(_position.position.y * -100);
    }
}
