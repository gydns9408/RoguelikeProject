using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateClass_Panel : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Instance.StageStart();
    }
}
