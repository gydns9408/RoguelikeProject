using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HPBar : MonoBehaviour
{
    Slider _slider;
    TextMeshProUGUI _HpText;

    float _startValue = 1;
    float _goalValue = 1;
    public float _barChangeSpeed = 2;
    float _currentBarAchievement = 1;

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
        //if (_slider.value > _goalValue)
        //{
        //    _slider.value -= _barChangeSpeed * Time.deltaTime;
        //    if (_slider.value < _goalValue) 
        //    {
        //        _slider.value = _goalValue;
        //    }
        //}
        //else if (_slider.value < _goalValue)
        //{
        //    _slider.value += _barChangeSpeed * Time.deltaTime;
        //    if (_slider.value > _goalValue)
        //    {
        //        _slider.value = _goalValue;
        //    }
        //}
        _currentBarAchievement += Time.deltaTime * _barChangeSpeed;
        _slider.value = Mathf.Lerp(_startValue, _goalValue, _currentBarAchievement);
    }

    // Update is called once per frame
    private void Refresh(float maxHp, float hp)
    {
        _currentBarAchievement = 0;
        _goalValue = hp / maxHp;
        _startValue = _slider.value;
        _HpText.text = $"{(int)hp} / {(int)maxHp}";
    }
}
