using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    public ItemData[] ItemDatas;
    public ItemData this[ItemCode type] => ItemDatas[(int)type];
}
