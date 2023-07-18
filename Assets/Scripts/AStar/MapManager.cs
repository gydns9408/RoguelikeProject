using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    GridMap _gridMap;
    public GridMap GridMap => _gridMap;

    public int _wallMaxAmount = 16;
    List<Node> _wallSettingNodeList;
    List<Node> _doorSettingNodeList;

    public Door_Base[] _doors;

    const int Arrow_Amount = 4;

    private void Awake()
    {
        Transform parent = transform.parent;
        Tilemap tilemap = parent.GetChild(0).GetComponent<Tilemap>();
        _gridMap = new GridMap(tilemap);
        _wallSettingNodeList = new List<Node>(_wallMaxAmount);
        _doorSettingNodeList = new List<Node>(Arrow_Amount);
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
                wallAmount = Random.Range(0, _wallMaxAmount);
                if (wallAmount + 4 <= GridMap.PlainNodes.Count)
                {
                    break;
                }
            }

            for (int i = 0; i < wallAmount; i++)
            {
                int settingIndex = Random.Range(0, GridMap.PlainNodes.Count);
                Node node = GridMap.PlainNodes[settingIndex];
                GridMap.PossibleDoorNodes_Remove(node);
                if (GridMap.PossibleDoorNodes[(int)Arrow.North].Count < 1 || GridMap.PossibleDoorNodes[(int)Arrow.East].Count < 1 || GridMap.PossibleDoorNodes[(int)Arrow.South].Count < 1 || GridMap.PossibleDoorNodes[(int)Arrow.West].Count < 1)
                {
                    reset = true;
                    break;
                }
                node.gridType = Node.GridType.Wall;
                _wallSettingNodeList.Add(node);
                GridMap.PlainNodes_Remove(node);
            }

            if (!reset) {
                for (int i = 0; i < GridMap.PossibleDoorNodes.Length; i++)
                {
                    int settingIndex2 = Random.Range(0, GridMap.PossibleDoorNodes[i].Count);
                    Node node2 = GridMap.PossibleDoorNodes[i][settingIndex2];
                    node2.gridType = Node.GridType.Door;
                    _doorSettingNodeList.Add(node2);
                    GridMap.PlainNodes_Remove(node2);
                }
            }

            for (int i = 1; i < _doorSettingNodeList.Count; i++)
            {
                if (!AStar.IsPossiblePath(GridMap, _doorSettingNodeList[(int)Arrow.North], _doorSettingNodeList[i]))
                {
                    reset = true;
                    break;
                }
            }


            if (reset)
            {
                GridMap.PlainNodes_Restore();
                _wallSettingNodeList.Clear();
                _doorSettingNodeList.Clear();
            }
            else
            {
                foreach (var node in _wallSettingNodeList)
                {
                    Wall_BlackTombstone tomb = SpawnManager_Etc.Instance.GetObject_Wall_BlackTombstone();
                    tomb.transform.position = GridMap.GridToWorld(node.x_coordinate, node.y_coordinate) + new Vector2(tomb.X_Correction_Value, tomb.Y_Correction_Value);
                    tomb.Sprite_SortingOrderSetting();
                }
                for (int i = 0; i < _doorSettingNodeList.Count; i++)
                {
                    _doors[i].transform.position = GridMap.GridToWorld(_doorSettingNodeList[i].x_coordinate, _doorSettingNodeList[i].y_coordinate) + new Vector2(_doors[i].X_Correction_Value, _doors[i].Y_Correction_Value);
                }
            }

        } 
    }
}
