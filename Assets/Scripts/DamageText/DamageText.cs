using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DamageSkin
{
    Default = 0,
    Critical,
    Player
}

public class DamageText : PoolObjectShape
{
    Transform _firstParent;
    Transform _critical_transform;

    Image[] _images;
    Image _critical_image;

    public float _disappearSpeed = 1f;
    public float _takeoffSpeed = 1f;

    int _activeNumberAmount;
    float _critical_image_positionX_correction_value = -20f;

    private void Awake()
    {
        _firstParent = transform.parent;
        Transform child = transform.GetChild(0);
        _critical_image = child.GetComponent<Image>();
        _critical_image.color = Color.clear;
        _critical_transform = child;

        child = transform.GetChild(1);
        _images = new Image[child.childCount];
        for (int i = 0; i < _images.Length; i++)
        {
            child = transform.GetChild(1).GetChild(i);
            _images[i] = child.GetComponent<Image>();
            _images[i].color = Color.clear;
            _images[i].gameObject.SetActive(false);
        }
        
    }

    public void DamageTextSetting(string numberString, DamageSkin damageSkin)
    {
        _activeNumberAmount = numberString.Length;
        for (int i = 0; i < _activeNumberAmount; i++)
        {
            _images[i].sprite = SpawnManager_Etc.Instance.DamageSkin_Sprites_List[(int)damageSkin][int.Parse(numberString[i].ToString())];
            _images[i].SetNativeSize();
            _images[i].gameObject.SetActive(true);
        }
        StartCoroutine(Disappearing(damageSkin));
    }

    private IEnumerator Disappearing(DamageSkin damageSkin)
    { 
        yield return null;
        for (int i = 0; i < _activeNumberAmount; i++)
        {
            _images[i].color = Color.white;
        }
        if (damageSkin == DamageSkin.Critical)
        {
            _critical_transform.position = new Vector3(_images[0].transform.position.x + _critical_image_positionX_correction_value, _critical_transform.position.y, _critical_transform.position.z);
            _critical_image.color = Color.white;
        }
        else
        {
            _critical_image.color = Color.clear;
        }
        Color color = Color.white;
        Color color2 = _critical_image.color;
        
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime * _disappearSpeed;
            color2.a -= Time.deltaTime * _disappearSpeed;
            for (int i = 0; i < _activeNumberAmount; i++)
            {
                _images[i].color = color;
            }
            _critical_image.color = color2;
            transform.Translate(Vector2.up * _takeoffSpeed, Space.World);
            yield return null;
        }
        Before_OnDisable();
        _activeNumberAmount = 0;
        gameObject.SetActive(false);
    }

    public override void Before_OnDisable()
    {
        for (int i = 0; i < _activeNumberAmount; i++)
        {
            _images[i].color = Color.clear;
            _images[i].gameObject.SetActive(false);
        }
        _critical_image.color = Color.clear;
        transform.SetParent(_firstParent);
    }
}
