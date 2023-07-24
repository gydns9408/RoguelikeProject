using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_North : Door_Base
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.Instance.Player.MoveDir.y > 0)
        {
            GameManager.Instance.MoveStage(Door_Arrow);
        }
    }
}
