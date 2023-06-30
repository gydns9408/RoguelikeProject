using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
{
    protected Image _image;

    protected Color _startColor = Color.white;
    protected Color _goalColor = Color.white;

    protected float _currentColorAchievement = 1;
    public float _colorChangeSpeed = 10;
    public float _colorRestoreTime = 0.1f;
    public Color NormalEnterColor;
    public Color PointerEnterColor;
    public Color PointerClickColor;

    protected WaitForSeconds _wait;

    protected virtual void Awake()
    {
        _image = GetComponent<Image>();
        _wait = new WaitForSeconds(_colorRestoreTime);
    }

    private void Update()
    {
        _currentColorAchievement += Time.deltaTime * _colorChangeSpeed;
        _image.color = Color.Lerp(_startColor, _goalColor, _currentColorAchievement);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        _currentColorAchievement = 0;
        _startColor = _image.color;
        _goalColor = PointerEnterColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        _currentColorAchievement = 0;
        _startColor = _image.color;
        _goalColor = NormalEnterColor;
    }

    public void OnPointerMove(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StopAllCoroutines();
        _currentColorAchievement = 0;
        _startColor = _image.color;
        _goalColor = PointerClickColor;
        StartCoroutine(RestoreColor());
    }

    IEnumerator RestoreColor()
    {
        yield return _wait;
        _currentColorAchievement = 0;
        _startColor = _image.color;
        _goalColor = PointerEnterColor;
    }
}
