using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Player_Bar_HP : Player_Bar_Base
{
    protected override void OnStart(Player player)
    {
        player._onChangeHP += Refresh;
        FirstRefresh(player.MaxHP, PlayerDataManager.Instance.HP);
    }
}
