using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine.Rendering.Universal;

public class ItemExplanWindowUI : MonoBehaviour
{
    public float MaxAlpha = 0.6f;
    public float AlphaChangeSpeed = 1.0f;
    bool _isVisible = false;

    CanvasGroup _canvasGroup;

    Image _itemImage;
    TextMeshProUGUI _itemNameText;
    TextMeshProUGUI _itemExplanText;


    bool _isOff;
    public bool IsOff
    {
        get => _isOff;
        set
        {
            if (value)
            {
                Close();
            }
            _isOff = value;
        }
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        Transform child = transform.GetChild(0);
        _itemImage = child.GetComponent<Image>();
        child = transform.GetChild(1);
        _itemNameText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        _itemExplanText = child.GetComponent<TextMeshProUGUI>();

    }
    private void Update()
    {
        if (_isVisible)
        {
            _canvasGroup.alpha += Time.deltaTime * AlphaChangeSpeed;
        }
        else
        {
            _canvasGroup.alpha -= Time.deltaTime * AlphaChangeSpeed;
        }
        _canvasGroup.alpha = Mathf.Clamp(_canvasGroup.alpha, 0, MaxAlpha);
    }

    public void Open(ItemCode itemCode)
    {
        if (!_isOff)
        {
            _itemImage.sprite = GameManager.Instance.ItemData[itemCode].itemIcon;
            _itemNameText.text = GameManager.Instance.ItemData[itemCode].itemName;
            _itemExplanText.text = GameManager.Instance.ItemData[itemCode].itemExplan;
            _isVisible = true;
            PositionSetting(Mouse.current.position.ReadValue());
        }
    }

    public void Close()
    {
        _isVisible = false;
    }

    public void PositionSetting(Vector2 screenPos)
    {
        if (_isVisible)
        {
            RectTransform rect = (RectTransform)transform;
            int xCorrectionValue = (int)(screenPos.x + rect.sizeDelta.x) - Screen.width;
            xCorrectionValue = Mathf.Max(0, xCorrectionValue);
            screenPos.x -= xCorrectionValue;
            transform.position = screenPos;
        }
    }
}
