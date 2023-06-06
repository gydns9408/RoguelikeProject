using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationStateClass : StateMachineBehaviour
{
    Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player.AttackStart();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player.AttackEnd();
    }

}
