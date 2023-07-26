using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyWindowUI : MonoBehaviour
{
    TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.Instance.Player._onChangeMoney += Refresh;
        Refresh(GameManager.Instance.Player.Money);
    }

    private void Refresh(int money)
    {
        _text.text = $"{money:N0}";
    }
}
