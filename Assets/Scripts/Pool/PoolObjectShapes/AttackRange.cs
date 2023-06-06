using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : PoolObjectShape
{
    float _lifeTime = 10.0f;

    private void Awake()
    {
        Animator anim = GetComponent<Animator>();
        _lifeTime = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    void OnEnable()
    {
        StopAllCoroutines();
        if (gameObject.activeSelf == true)
        {
            StartCoroutine(LifeOver(_lifeTime));
        }
    }
}
