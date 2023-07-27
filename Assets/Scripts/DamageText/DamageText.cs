using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : PoolObjectShape
{
    Image[] _images;
    Image _critical_image;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        _critical_image = child.GetComponent<Image>();
        child = transform.GetChild(1);
        _images = new Image[child.childCount];
        for (int i = 0; i < _images.Length; i++)
        {
            child = transform.GetChild(1).GetChild(i);
            _images[i] = child.GetComponent<Image>();

        }

    }

    private void OnEnable()
    {
        for (int i = 0; i < _images.Length; i++)
        {
            _images[i].color = Color.white;
        }
        _critical_image.color = Color.white;
        StartCoroutine(Disappearing());
    }

    private IEnumerator Disappearing()
    {
        Color color = _critical_image.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime;
            for (int i = 0; i < _images.Length; i++)
            {
                _images[i].color = color;
            }
            _critical_image.color = color;
            transform.Translate(Vector2.up);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
