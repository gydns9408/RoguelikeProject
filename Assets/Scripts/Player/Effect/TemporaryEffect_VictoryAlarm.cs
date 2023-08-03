using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryEffect_VictoryAlarm : TemporaryEffect
{
    protected override IEnumerator AnimationPlay()
    {
        yield return _wait;
        GameManager.Instance.StageClear();
        gameObject.SetActive(false);
    }
}
