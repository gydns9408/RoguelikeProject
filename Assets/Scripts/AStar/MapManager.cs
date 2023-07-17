using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    GridMap _gridMap;
    public GridMap GridMap => _gridMap;

    public int wallMaxAmount = 16;
    List<Wall_Base> wallList;
    private void Awake()
    {
        Transform parent = transform.parent;
        Tilemap tilemap = parent.GetChild(0).GetComponent<Tilemap>();
        _gridMap = new GridMap(tilemap);
        wallList = new List<Wall_Base>(wallMaxAmount);
    }

    private void Start()
    {
        int wallAmount = 0;
        bool reset = true;
        while (reset)
        {
            reset = false;
            while (true)
            {
                wallAmount = Random.Range(0, wallMaxAmount);
                if (wallAmount + 4 <= GridMap.PlainNodes.Count)
                {
                    break;
                }
            }

            for (int i = 0; i < wallAmount; i++)
            {
                int settingIndex = Random.Range(0, GridMap.PlainNodes.Count);
                Node node = GridMap.PlainNodes[settingIndex];
                GridMap.PossibleNorthDoorNodes_Remove(node);
                GridMap.PossibleEastDoorNodes_Remove(node);
                GridMap.PossibleSouthDoorNodes_Remove(node);
                GridMap.PossibleWestDoorNodes_Remove(node);
                if (GridMap.PossibleNorthDoorNodes.Count < 1 || GridMap.PossibleEastDoorNodes.Count < 1 || GridMap.PossibleSouthDoorNodes.Count < 1 || GridMap.PossibleWestDoorNodes.Count < 1)
                {
                    reset = true;
                    break;
                }
                node.gridType = Node.GridType.Wall;
                Wall_BlackTombstone tomb = SpawnManager_Etc.Instance.GetObject_Wall_BlackTombstone();
                tomb.transform.position = GridMap.GridToWorld(node.x_coordinate, node.y_coordinate) + new Vector2(tomb.X_Correction_Value, tomb.Y_Correction_Value);
                tomb.Sprite_SortingOrderSetting();
                wallList.Add(tomb);
                GridMap.PlainNodes_Remove(node);
            }

            if (reset)
            {
                GridMap.PlainNodes_Restore();
                foreach (var wall in wallList)
                {
                    wall.gameObject.SetActive(false);
                }
                wallList.Clear();
            }
        } 
    }
}
