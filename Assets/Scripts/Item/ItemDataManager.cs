using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    public ItemData[] ItemDatas;
    public ItemData this[ItemType type] => ItemDatas[(int)type];
}
