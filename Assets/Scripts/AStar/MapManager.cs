using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    GridMap gridMap;
    void Awake()
    {
        Transform parent = transform.parent;
        Tilemap tilemap = parent.GetChild(0).GetComponent<Tilemap>();
    }
}
