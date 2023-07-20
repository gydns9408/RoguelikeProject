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
    const int Wall_Type_Amount = 5;

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
        if (!GameManager.Instance.NowRoom.IsClear)
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

                if (!reset)
                {
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
                        int rand = Random.Range(0, Wall_Type_Amount);
                        Wall_Base wall = SpawnManager_Etc.Instance.GetObject_Wall((WallCode)rand);
                        wall.transform.position = GridMap.GridToWorld(node.x_coordinate, node.y_coordinate) + new Vector2(wall.X_Correction_Value, wall.Y_Correction_Value);
                        wall.Sprite_SortingOrderSetting();
                        WallInfo wallInfo = new WallInfo((WallCode)rand, node.x_coordinate, node.y_coordinate);
                        GameManager.Instance.NowRoom.AddToWallInfoList(wallInfo);
                    }
                    for (int i = 0; i < _doorSettingNodeList.Count; i++)
                    {
                        if (GameManager.Instance.NowRoom.LinkedRooms[i] != null)
                        {
                            _doors[i].transform.position = GridMap.GridToWorld(_doorSettingNodeList[i].x_coordinate, _doorSettingNodeList[i].y_coordinate) + new Vector2(_doors[i].X_Correction_Value, _doors[i].Y_Correction_Value);
                        }
                    }
                    Monster_Spawn();
                }

            }
        }
        else
        {
            foreach (var wallInfo in GameManager.Instance.NowRoom.WallInfoList)
            {
                Wall_Base wall = SpawnManager_Etc.Instance.GetObject_Wall(wallInfo.wallCode);
                wall.transform.position = GridMap.GridToWorld(wallInfo.x, wallInfo.y) + new Vector2(wall.X_Correction_Value, wall.Y_Correction_Value);
                wall.Sprite_SortingOrderSetting();
            }
        }
    }

    private void Monster_Spawn()
    {
        GameManager.Instance.IsMonsterSpawn = false;
        Monster_Base.TotalCount = 0;
        if (!GameManager.Instance.NowRoom.IsClear)
        {
            foreach (var spawnInfo in GameManager.Instance.NowRoom.SpawnMonsterList)
            {
                Monster_Spawn(spawnInfo.monsterType, spawnInfo.spawnAmount);
            }
        }
        GameManager.Instance.IsMonsterSpawn = true;
    }

    private void Monster_Spawn(Monster_Type type, uint spawnAmount)
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            int settingIndex = Random.Range(0, GridMap.PlainNodes.Count);
            Node node = GridMap.PlainNodes[settingIndex];
            node.gridType = Node.GridType.Monster;
            Monster_Base mob = SpawnManager_Monster.Instance.GetObject(type);
            mob.transform.position = GridMap.GridToWorld(node.x_coordinate, node.y_coordinate);
            GridMap.PlainNodes_Remove(node);
            Monster_Base.TotalCount++;
        }
    }

}
