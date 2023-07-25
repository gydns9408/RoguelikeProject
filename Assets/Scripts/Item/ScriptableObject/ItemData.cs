using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCode
{
    None,
    BeastMeat,
    ApakiFruit,
    BronzeCoin,
    GoldCoin,
    Purse
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("������ �⺻ ������")]
    public ItemCode itmeCode;
    public string itemName = "�������̸�";
    public Sprite itemIcon;
    public uint maxAmount = 1;
    public uint price = 0;
    public string itemExplan = "�����ۼ���";
}
