using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UI_Window_HaveOpenCloseAnim : UI_Window_Base
{
    protected bool _isFullOpen = false;
    public bool IsFullOpen => _isFullOpen;
    protected Animator _anim;

    readonly int _closeHash = Animator.StringToHash("Close");

    protected virtual void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        Close();
    }

    public virtual void FullOpen()
    {
        _isFullOpen = true;
    }

    public virtual void StartClose()
    {
        _isFullOpen = false;
        _anim.SetTrigger(_closeHash);
    }

}
