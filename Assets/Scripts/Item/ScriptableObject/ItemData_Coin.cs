using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Data - Coin", order = 3)]
public class ItemData_Coin : ItemData, IConsumable
{
    public void Consume(Player player)
    {
        player.Money += (int)price;
    }
}
