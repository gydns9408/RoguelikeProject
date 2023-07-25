using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_East : Door_Base
{
    Arrow Door_Arrow = Arrow.East;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.Instance.Player.MoveDir.x > 0)
        {
            GameManager.Instance.MoveStage(Door_Arrow);
        }
    }
}
