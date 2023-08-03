using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Arrow
{
    North = 0,
    East,
    South,
    West
}

public struct SpawnMonsterInfo
{
    public Monster_Type monsterType;
    public uint spawnAmount;

    public SpawnMonsterInfo(Monster_Type MonsterType, uint SpawnAmount)
    {
        monsterType = MonsterType;
        spawnAmount = SpawnAmount;
    }
}

public struct WallInfo
{
    public WallCode wallCode;
    public int x;
    public int y;

    public WallInfo(WallCode WallCode, int X, int Y)
    {
        wallCode = WallCode;
        x = X;
        y = Y;
    }
}

public struct DoorInfo
{
    public int x;
    public int y;

    public DoorInfo(int X, int Y)
    {
        x = X;
        y = Y;
    }
}
public class Room
{
    uint _depth;
    public uint Depth => _depth;

    Room[] _linkedRooms;
    public Room[] LinkedRooms => _linkedRooms;
    DoorInfo[] _doorInfos;
    public DoorInfo[] DoorInfos => _doorInfos;

    List<SpawnMonsterInfo> _spawnMonsterList;
    public List<SpawnMonsterInfo> SpawnMonsterList
    {
        get => _spawnMonsterList;
        private set => _spawnMonsterList = value;
    }

    List<WallInfo> _wallInfoList;
    public List<WallInfo> WallInfoList
    {
        get => _wallInfoList;
        private set => _wallInfoList = value;
    }

    bool _isClear;
    public bool IsClear
    {
        get => _isClear;
        set
        {
            if (!_isClear)
            {
                _isClear = value;
            }
        }
    }

    bool _isBossRoom;
    public bool IsBossRoom
    {
        get => _isBossRoom;
        set
        {
            if (!_isBossRoom)
            {
                _isBossRoom = value;
            }
        }
    }

    bool _isShopRoom;
    public bool IsShopRoom
    {
        get => _isShopRoom;
        set
        {
            if (!_isShopRoom)
            {
                _isShopRoom = value;
            }
        }
    }

    public Room(List<SpawnMonsterInfo> spawnMonsterList)
    {
        SpawnMonsterList = spawnMonsterList;
        _linkedRooms = new Room[4];
        WallInfoList = new List<WallInfo>();
        _doorInfos = new DoorInfo[4];
    }

    public void SettingDepth(uint depth)
    {
        _depth = depth;
    }

    public void AddToWallInfoList(WallInfo info)
    {
        WallInfoList.Add(info);
    }

    public void AddToDoorInfoArray(DoorInfo info, int index)
    {
        DoorInfos[index] = info;
    }
}
