using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon_Skill3 : SkillIcon_Base
{
    private void Start()
    {
        _coolTime_reciprocal = 1 / GameManager.Instance.Player.Skill3_CoolTime;
    }
    private void Update()
    {
        _image.fillAmount = GameManager.Instance.Player.Skill3_CoolTime_Value * _coolTime_reciprocal;
    }
}
