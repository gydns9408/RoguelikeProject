using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HPBar : MonoBehaviour
{
    Slider _slider;
    TextMeshProUGUI _HpText;

    float _goalValue = 1;
    float _barChangeSpeed = 1;

    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
        _HpText = GetComponentInChildren<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player._onChangeHP += Refresh;
        _slider.value = _goalValue;
        Refresh(player.MaxHP, player.HP);
    }

    private void Update()
    {
        if (_slider.value > _goalValue)
        {
            _slider.value -= _barChangeSpeed * Time.deltaTime;
            if (_slider.value < _goalValue) 
            {
                _slider.value = _goalValue;
            }
        }
        else if (_slider.value < _goalValue)
        {
            _slider.value += _barChangeSpeed * Time.deltaTime;
            if (_slider.value > _goalValue)
            {
                _slider.value = _goalValue;
            }
        }
    }

    // Update is called once per frame
    private void Refresh(float maxHp, float hp)
    {
        _goalValue = hp / maxHp;
        _HpText.text = $"{(int)hp} / {(int)maxHp}";
    }
}
