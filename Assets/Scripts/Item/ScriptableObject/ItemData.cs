using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCode
{
    None,
    BeastMeat
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("아이템 기본 데이터")]
    public ItemCode itmeCode;
    public string itemName = "아이템이름";
    public GameObject dropItmePrefab;
    public Sprite itemIcon;
    public uint maxAmount = 1;
    public string itemExplan = "아이템설명";
}
