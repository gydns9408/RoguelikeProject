using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3AnimationStateClass : StateMachineBehaviour
{
    Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player.Skill2End();
    }
}
