using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateClass_ItemInventory : StateMachineBehaviour
{
    public class IdleStateClass_ItemSpliterUI : StateMachineBehaviour
    {
        ItemInventoryUI _inven;

        private void Awake()
        {
            _inven = FindObjectOfType<ItemInventoryUI>();
        }

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _inven.FullOpen();
        }
    }

}
