using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjectShape : MonoBehaviour
{

    public Action _onDisable;

    protected virtual IEnumerator LifeOver(float remainingTime = 0.0f) {
        yield return new WaitForSeconds(remainingTime);
        gameObject.SetActive(false);
    }

    protected virtual void OnDisable() {
        _onDisable?.Invoke();
    }

    public virtual void Before_OnDisable()
    { 
    }

}
