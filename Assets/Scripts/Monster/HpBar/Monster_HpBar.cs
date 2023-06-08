using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Monster_HpBar : MonoBehaviour
{
    float _maxHp = 0;

    Transform _position;

    SpriteRenderer _sprite;
    Transform _fill_transform;
    SpriteRenderer _fill_sprite;
    SpriteRenderer _background_sprite;

    static int _sprite_base_sortingOrder = 0;
    static int _fill_sprite_base_sortingOrder = 1;
    static int _background_sprite_base_sortingOrder = -1;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _fill_transform = transform.GetChild(0);
        _fill_sprite = _fill_transform.GetComponent<SpriteRenderer>();
        Transform child = transform.GetChild(1);
        _background_sprite = child.GetComponent<SpriteRenderer>();
        Monster_Base parent = GetComponentInParent<Monster_Base>();
        _maxHp = parent._maxHp;
        _position = parent.Position;
        parent._onChangeHP += Refresh;
    }

    private void Refresh(float hp)
    {
        float ratio = hp / _maxHp;
        Color color = _fill_sprite.color;
        if (ratio > 0.5)
        {
            color.r = Color.green.r; 
            color.g = Color.green.g; 
            color.b = Color.green.b;
            _fill_sprite.color = color;
        }
        else if (ratio > 0.25)
        {
            color.r = Color.yellow.r;
            color.g = Color.yellow.g;
            color.b = Color.yellow.b;
            _fill_sprite.color = color;
        }
        else
        {
            color.r = Color.red.r;
            color.g = Color.red.g;
            color.b = Color.red.b;
            _fill_sprite.color = color;
        }

        _fill_transform.localScale = new Vector3(ratio, 1, 1);
    }

    public void HpBar_Visible(bool active)
    {
        Color color = _sprite.color;
        Color color2 = _fill_sprite.color;
        Color color3 = _background_sprite.color;
        if (active)
        {
            color.a = 1f;
            color2.a = 1f;
            color3.a = 1f;
        }
        else
        {
            color.a = 0f;
            color2.a = 0f;
            color3.a = 0f;
        }
        _sprite.color = color;
        _fill_sprite.color = color2;
        _background_sprite.color = color3;
    }

    private void LateUpdate()
    {
        _sprite.sortingOrder = _sprite_base_sortingOrder + (int)(_position.position.y * -100);
        _fill_sprite.sortingOrder = _fill_sprite_base_sortingOrder + (int)(_position.position.y * -100);
        _background_sprite.sortingOrder = _background_sprite_base_sortingOrder + (int)(_position.position.y * -100);
    }

}
