using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Arrow
{
    North,
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
    }

    public void SettingDepth(uint depth)
    {
        _depth = depth;
    }
}
