using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomSlider : CustomButton
{

    protected override void Awake()
    {
        Transform child = transform.GetChild(2);
        child = child.GetChild(0);
        _image = child.GetComponent<Image>();
        _wait = new WaitForSeconds(_colorRestoreTime);
    }
}
