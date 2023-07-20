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

public class Room
{
    uint _depth;
    public uint Depth => _depth;

    Room[] _linkedRooms;
    public Room[] LinkedRooms => _linkedRooms;

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

    bool _isBossroom;
    public bool IsBossroom
    {
        get => _isBossroom;
        set
        {
            if (!_isBossroom)
            {
                _isBossroom = value;
            }
        }
    }

    public Room(List<SpawnMonsterInfo> spawnMonsterList)
    {
        SpawnMonsterList = spawnMonsterList;
        _linkedRooms = new Room[4];
        WallInfoList = new List<WallInfo>();
    }

    public void SettingDepth(uint depth)
    {
        _depth = depth;
    }

    public void AddToWallInfoList(WallInfo info)
    {
        WallInfoList.Add(info);
    }
}