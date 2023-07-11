using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Arrow
{
    North,
    South,
    East,
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
    public Room _northRoom;
    public Room _southRoom;
    public Room _eastRoom;
    public Room _westRoom;

    public Room[] linkedRooms;


    List<SpawnMonsterInfo> _spawnMonsterList;
    public List<SpawnMonsterInfo> SpawnMonsterList
    {
        get => _spawnMonsterList;
        private set => _spawnMonsterList = value;
    }

    bool _isClear;
    public bool IsClear => _isClear;

    public Room(List<SpawnMonsterInfo> spawnMonsterList)
    {
        SpawnMonsterList = spawnMonsterList;
        linkedRooms = new Room[4];
    }
}
