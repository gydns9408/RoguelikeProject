using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    Player _player;
    public Player Player => _player;
    PlayerInfoUI _playerInfoUI;
    ItemInventoryUI _invenUI;
    public ItemInventoryUI InvenUI => _invenUI;

    ItemDataManager _itemDataManager;
    public ItemDataManager ItemData => _itemDataManager;

    bool _isVisible_enemyHpBar = true;
    public bool IsVisible_enemyHpBar => _isVisible_enemyHpBar;
    public Action<bool> _onChange_isVisible_enemyHpBar;

    SystemSettingInputActions _inputActions;

    public float _randomSpawnArea_minX = -8.5f;
    public float _randomSpawnArea_maxX = 30f;
    public float _randomSpawnArea_minY = -13.5f;
    public float _randomSpawnArea_maxY = 15f;

    public uint _inventorySlotAmount = 20;

    bool _invenUIItemSplitMode;
    public bool InvenUIItemSplitMode => _invenUIItemSplitMode;

    Room[] rooms;
    Room _nowRoom;

    uint monster_totalAmount;
    uint monster_killAmount;
    public uint Monster_KillAmount
    {
        set
        {
            if (isMonsterSpawn)
            {
                monster_killAmount = value;
                if (monster_killAmount >= monster_totalAmount && !_nowRoom.IsClear)
                {
                    _nowRoom.IsClear = true;
                    StageClear();
                }
            }
        }
    }
    bool isMonsterSpawn;

    protected override void RunOnlyOnce_Initialize()
    {
        if (_initialized == false)
        {
            _initialized = true;
            _itemDataManager = GetComponent<ItemDataManager>();
            _inputActions = new SystemSettingInputActions();
            _inputActions.System.Enable();
            _inputActions.System.MonsterHpBarVisible.performed += OnMonster_HpBarVisible_Option_Input;
            _inputActions.System.PlayerInfoUIVisible.performed += OnUI_PlayerInfoUIVisible_Option_Input;
            _inputActions.System.InvenUIItemSplitMode.performed += OnUI_InvenItemSplitMode_Option_Input;
            _inputActions.System.InvenUIItemSplitMode.canceled += OnUI_InvenItemSplitMode_Option_Input;

            int roomAmount = UnityEngine.Random.Range(10, 15);
            rooms = new Room[roomAmount];
            for (int i = 0; i < roomAmount; i++)
            {
                List<SpawnMonsterInfo> spawnList = new List<SpawnMonsterInfo>();
                int monsterAmount = UnityEngine.Random.Range(10, 15);
                SpawnMonsterInfo spawnMonsterInfo = new SpawnMonsterInfo(Monster_Type.WildBoar, (uint)monsterAmount);
                spawnList.Add(spawnMonsterInfo);
                Room room = new Room(spawnList);

                rooms[i] = room;
            }

            rooms[roomAmount - 1].IsBossroom = true;
            Stack<int> roomStack = new Stack<int>();
            roomStack.Push(0);
            int currentRoomNumber = 1;
            Arrow[] arrowOrder = new Arrow[] { Arrow.North, Arrow.East, Arrow.South, Arrow.West };

            while (true)
            {
                if (roomStack.Count < 1)
                {
                    break;
                }
                int selectRoomNumber = roomStack.Pop();
                int linkRoomAmount = 0;

                int linkedRoomAmount = 0;
                for (int i = 0; i < rooms[selectRoomNumber].LinkedRooms.Length; i++)
                {
                    if (rooms[selectRoomNumber].LinkedRooms[i] != null)
                    {
                        linkedRoomAmount++;
                    }
                }
                int minAddRoomAmount = 0;
                int maxAddRoomAmount = Mathf.Min(5 - linkedRoomAmount, roomAmount - currentRoomNumber + 1);
                if (roomStack.Count < 1 && currentRoomNumber < roomAmount)
                {
                    minAddRoomAmount = 1;
                }
                else
                {
                    minAddRoomAmount = 0;
                }

                linkRoomAmount = UnityEngine.Random.Range(minAddRoomAmount, maxAddRoomAmount);

                arrowOrder = SuffleArray(arrowOrder);
                for (int i = 0; i < linkRoomAmount; i++)
                {
                    int j = i % arrowOrder.Length;

                    if (rooms[selectRoomNumber].LinkedRooms[(int)arrowOrder[j]] != null)
                    {
                        linkRoomAmount++;
                        continue;
                    }
                    rooms[selectRoomNumber].LinkedRooms[(int)arrowOrder[j]] = rooms[currentRoomNumber];
                    int k = ((int)arrowOrder[j] + 2) % 4;
                    rooms[currentRoomNumber].LinkedRooms[k] = rooms[selectRoomNumber];
                    rooms[currentRoomNumber].SettingDepth(rooms[selectRoomNumber].Depth + 1);
                    roomStack.Push(currentRoomNumber);
                    currentRoomNumber++;
                }
            }
            _nowRoom = rooms[0];
        }
    }
    protected override void Initialize()
    {
        _player = FindObjectOfType<Player>();
        _playerInfoUI = FindObjectOfType<PlayerInfoUI>(true);
        ItemInventory itemInventory = new ItemInventory(_inventorySlotAmount, _player);
        _invenUI = FindObjectOfType<ItemInventoryUI>(true);
        _invenUI.Initialize(itemInventory);
    }



    private void OnMonster_HpBarVisible_Option_Input(InputAction.CallbackContext _)
    {
        _isVisible_enemyHpBar = !_isVisible_enemyHpBar;
        _onChange_isVisible_enemyHpBar?.Invoke(_isVisible_enemyHpBar);
    }

    private void OnUI_PlayerInfoUIVisible_Option_Input(InputAction.CallbackContext _)
    {
        if (_playerInfoUI.gameObject.activeSelf == false)
        {
            _playerInfoUI.Open();
        }
        else
        {
            _playerInfoUI.Close();
        }
    }

    private void OnUI_InvenItemSplitMode_Option_Input(InputAction.CallbackContext context)
    {
        _invenUIItemSplitMode = context.performed;
    }

    private void OnDisable()
    {
        _inputActions.System.PlayerInfoUIVisible.performed -= OnUI_PlayerInfoUIVisible_Option_Input;
        _inputActions.System.MonsterHpBarVisible.performed -= OnMonster_HpBarVisible_Option_Input;
        _inputActions.System.Disable();
    }

    public void Monster_Spawn()
    {
        monster_killAmount = 0;
        monster_totalAmount = 0;
        if (!_nowRoom.IsClear)
        {
            foreach (var spawnInfo in _nowRoom.SpawnMonsterList)
            {
                monster_totalAmount += spawnInfo.spawnAmount;
                Monster_Spawn(spawnInfo.monsterType, spawnInfo.spawnAmount);
            }
        }
        isMonsterSpawn = true;
    }

    private void Monster_Spawn(Monster_Type type, uint spawnAmount)
    {
        for(int i = 0; i < spawnAmount; i++)
        {
            Monster_Base mob = SpawnManager_Monster.Instance.GetObject(type);
            mob.transform.position = new Vector3(UnityEngine.Random.Range(_randomSpawnArea_minX, _randomSpawnArea_maxX), UnityEngine.Random.Range(_randomSpawnArea_minY, _randomSpawnArea_maxY), 0f);
        }
    }

    private void StageClear()
    {

    }

    private T[] SuffleArray<T>(T[] array)
    {
        int changeTarget1, changeTarget2;
        T temp;
        for (int i = 0; i < array.Length; i++)
        {
            changeTarget1 = UnityEngine.Random.Range(0, array.Length);
            changeTarget2 = UnityEngine.Random.Range(0, array.Length);

            temp = array[changeTarget1];
            array[changeTarget1] = array[changeTarget2];
            array[changeTarget2] = temp;
        }

        return array;
    }
}
