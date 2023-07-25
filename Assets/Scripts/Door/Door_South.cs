using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_South : Door_Base
{
    Arrow Door_Arrow = Arrow.South;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.Instance.Player.MoveDir.y < 0)
        {
            GameManager.Instance.MoveStage(Door_Arrow);
        }
    }
}
