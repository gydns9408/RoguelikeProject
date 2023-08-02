using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Player_Bar_Base : MonoBehaviour
{
    Slider _slider;
    TextMeshProUGUI _valueText;

    float _startValue = 1;
    float _goalValue = 1;
    public float _barChangeSpeed = 2;
    float _currentBarAchievement = 1;

    protected void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
        _valueText = GetComponentInChildren<TextMeshProUGUI>();
    }

    protected void Start()
    {
        _slider.value = _goalValue;
        Player player = FindObjectOfType<Player>();
        OnStart(player);
    }

    protected virtual void OnStart(Player player)
    { 
    }

    protected void Update()
    {
        _currentBarAchievement += Time.deltaTime * _barChangeSpeed;
        _slider.value = Mathf.Lerp(_startValue, _goalValue, _currentBarAchievement);
    }

    protected void Refresh(float maxValue, float currentValue)
    {
        _currentBarAchievement = 0;
        _goalValue = currentValue / maxValue;
        _startValue = _slider.value;
        _valueText.text = $"{(int)currentValue} / {(int)maxValue}";
    }

    protected void FirstRefresh(float maxValue, float currentValue)
    {
        _currentBarAchievement = 1;
        _goalValue = currentValue / maxValue;
        _startValue = _goalValue;
        _valueText.text = $"{(int)currentValue} / {(int)maxValue}";
    }
}
