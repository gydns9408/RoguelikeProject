using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_HpBar : MonoBehaviour
{
    float _maxHp = 0;

    SpriteRenderer _sprite;
    Transform _fill_transform;
    SpriteRenderer _fill_sprite;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _fill_transform = transform.GetChild(0);
        _fill_sprite = _fill_transform.GetComponent<SpriteRenderer>();
        Monster_Base parent = GetComponentInParent<Monster_Base>();
        _maxHp = parent._maxHp;
        parent._onChangeHP += Refresh;
    }

    // Update is called once per frame
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
        if (active)
        {
            color.a = 1f;
            color2.a = 1f;
        }
        else
        {
            color.a = 0f;
            color2.a = 0f;
        }
        _sprite.color = color;
        _fill_sprite.color = color2;

    }
}
