using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Data - HP Heal Item", order = 2)]
public class ItemData_HPHealItem : ItemData, IUsable
{
    [Header("HP 회복 아이템 기본 데이터")]
    public float _healAmount = 10.0f;
    public bool Use(Unit_Base unit)
    {
        bool result = false;

        if (unit.HP < unit.MaxHP) {
            unit.HPChange(_healAmount);
            result = true;
        }
        return result;
    }
}
