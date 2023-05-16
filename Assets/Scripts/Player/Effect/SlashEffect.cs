using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    Animator _anim;
    WaitForSeconds _wait;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _wait = new WaitForSeconds(_anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(AnimationPlay());
    }

    IEnumerator AnimationPlay()
    {
        yield return _wait;
        gameObject.SetActive(false);
    }
}
