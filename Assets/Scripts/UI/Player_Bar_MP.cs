using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Player_Bar_MP : Player_Bar_Base
{
    protected override void OnStart(Player player)
    {
        player._onChangeMP += Refresh;
        FirstRefresh(player.MaxMP, PlayerDataManager.Instance.MP);
    }
}
