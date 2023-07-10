using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedStateClass_ItemInventory : StateMachineBehaviour
{
    ItemInventoryUI _inven;

    private void Awake()
    {
        _inven = FindObjectOfType<ItemInventoryUI>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _inven.EndClose();
    }
}
