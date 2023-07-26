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

    public Room NowRoom => _nowRoom;

    bool _isMonsterSpawn;
    public bool IsMonsterSpawn
    {
        get => _isMonsterSpawn;
        set => _isMonsterSpawn = value;
    }

    MapManager _mapManager;
    Panel _panel;

    bool _isStageStart;
    public bool IsStageStart
    {
        get => _isStageStart;
        private set => _isStageStart = value;
    }

    Arrow _playerEntryArrow;
    public Arrow PlayerEntryArrow => _playerEntryArrow;

    ItemInventory _itemInventory;

    const float trueValue = 1f;
    const float falseValue = 0f;
    const int Arrow_Amount = 4;

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

            _playerEntryArrow = (Arrow)UnityEngine.Random.Range(0, Arrow_Amount);
            _itemInventory = new ItemInventory(_inventorySlotAmount);
        }
    }
    protected override void Initialize()
    {
        IsStageStart = false;
        _player = FindObjectOfType<Player>();
        _player.IsStageStart = falseValue;
        _playerInfoUI = FindObjectOfType<PlayerInfoUI>(true);
        _itemInventory.OwnerSetting(_player);
        _invenUI = FindObjectOfType<ItemInventoryUI>(true);
        _invenUI.Initialize(_itemInventory);
        _mapManager = FindObjectOfType<MapManager>();
        _panel = FindObjectOfType<Panel>();
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
        if (_initialized)
        {
            _inputActions.System.PlayerInfoUIVisible.performed -= OnUI_PlayerInfoUIVisible_Option_Input;
            _inputActions.System.MonsterHpBarVisible.performed -= OnMonster_HpBarVisible_Option_Input;
            _inputActions.System.Disable();
        }
    }

    public void StageStart()
    {
        IsStageStart = true;
        Player.GameStart();
        _panel.CloseEnd();
    }

    public void StageClear()
    {
        Debug.Log("스테이지 클리어!");
        _mapManager.DoorsOpen();
    }

    public void MoveStage(Arrow arrow)
    {
        SpawnManager_Etc.Instance.Before_OnDisable();
        _nowRoom = _nowRoom.LinkedRooms[(int)arrow];
        int oppositeArrow = ((int)arrow + 2) % 4;
        _playerEntryArrow = (Arrow)oppositeArrow;
        SceneManager.LoadScene(2);
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
