using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel : UI_Window_Base
{

    Image _image;
    Animator _anim;

    readonly int _openHash = Animator.StringToHash("Open");

    private void Awake()
    {
        _image = GetComponent<Image>();
        _anim = GetComponent<Animator>();
    }

    public void CloseEnd()
    {
        _image.raycastTarget = false;
    }

    public void StartOpen()
    {
        _anim.SetTrigger(_openHash);
    }

}
