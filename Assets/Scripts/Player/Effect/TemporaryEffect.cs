using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryEffect : MonoBehaviour
{
    protected WaitForSeconds _wait;
    protected void Awake()
    {
        Animator anim = GetComponent<Animator>();
        _wait = new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    protected void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(AnimationPlay());
    }

    protected virtual IEnumerator AnimationPlay()
    {
        yield return _wait;
        gameObject.SetActive(false);
    }
}
