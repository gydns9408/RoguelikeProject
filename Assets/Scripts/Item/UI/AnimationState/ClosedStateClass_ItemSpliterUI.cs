using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedStateClass_ItemSpliterUI : StateMachineBehaviour
{
    ItemSpliterUI _spliter;

    private void Awake()
    {
        _spliter = FindObjectOfType<ItemSpliterUI>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _spliter.Close();
    }
}
