using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    GridMap _gridMap;
    public GridMap GridMap => _gridMap;
    private void Awake()
    {
        Transform parent = transform.parent;
        Tilemap tilemap = parent.GetChild(0).GetComponent<Tilemap>();
        _gridMap = new GridMap(tilemap);
    }

    private void Start()
    {
        int wallAmount = 0;

        while (true)
        {
            wallAmount = Random.Range(0, 16);
            if (wallAmount + 4 <= GridMap.PlainNodes.Count) 
            {
                break;
            }
        }

        for (int i = 0; i < wallAmount; i++)
        {
            int settingIndex = Random.Range(0, GridMap.PlainNodes.Count);
            Node node = GridMap.PlainNodes[settingIndex];
            node.gridType = Node.GridType.Wall;
            BlackTombstone tomb = SpawnManager_Etc.Instance.GetObject_BlackTombstone();
            tomb.transform.position = GridMap.GridToWorld(node.x_coordinate, node.y_coordinate) + new Vector2(tomb.X_Correction_Value, tomb.Y_Correction_Value);
            tomb.Sprite_SortingOrderSetting();
            GridMap.PlainNodes_Remove(node);
        }
    }
}
