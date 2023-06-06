using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_First : Test_Base
{
    public Player player;
    public Monster_Monster1 monster_Monster1;
    // Start is called before the first frame update
    protected override void Test_Action1(InputAction.CallbackContext _)
    {
        player.Test_HPChange(10);
    }

    protected override void Test_Action2(InputAction.CallbackContext _)
    {
        player.Test_HPChange(-10);
    }

    protected override void Test_Action3(InputAction.CallbackContext _)
    {
        monster_Monster1.Test1();
    }

}
