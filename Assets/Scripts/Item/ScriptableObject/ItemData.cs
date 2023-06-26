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
    [Header("������ �⺻ ������")]
    public ItemCode itmeCode;
    public string itemName = "�������̸�";
    public GameObject dropItmePrefab;
    public Sprite itemIcon;
    public uint maxAmount = 1;
    public string itemExplan = "�����ۼ���";
}
