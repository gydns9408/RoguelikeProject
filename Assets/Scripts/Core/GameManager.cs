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
                SpawnMonsterInfo spawnMonsterInfo = new SpawnMonsterInfo(Monster_Type.WildBoar , (uint)monsterAmount);
                spawnList.Add(spawnMonsterInfo);
                Room room = new Room(spawnList);
                rooms[i] = room;
            }
            Stack<int> roomStack = new Stack<int>();
            roomStack.Push(0);
            int currentRoomNumber = 1;
            int[] arrowOrder = new int[] { 0, 1, 2, 3 };
            
           
            while (true)
            {
                if (roomStack.Count < 1)
                {
                    break;
                }
                int selectRoomNumber = roomStack.Pop();
                int linkRoomAmount = 0;

                int linkedRoomAmount = 0;
                for (int i = 0; i < rooms[selectRoomNumber].linkedRooms.Length; i++)
                {
                     if (rooms[selectRoomNumber].linkedRooms[i] != null)
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

                    switch (arrowOrder[i])
                    {
                        case 0:
                            if (rooms[selectRoomNumber].linkedRooms[(int)Arrow.North] != null)
                            {
                            }
                            rooms[selectRoomNumber].linkedRooms[(int)Arrow.North] = rooms[currentRoomNumber];
                            rooms[currentRoomNumber].linkedRooms[(int)Arrow.South] = rooms[selectRoomNumber];
                            break;
                        case 1:
                            if (rooms[selectRoomNumber]._southRoom != null)
                            {
                            }
                            rooms[selectRoomNumber]._southRoom = rooms[currentRoomNumber];
                            rooms[currentRoomNumber]._northRoom = rooms[selectRoomNumber];
                            break;
                        case 2:
                            if (rooms[selectRoomNumber]._eastRoom != null)
                            {
                            }
                            rooms[selectRoomNumber]._eastRoom = rooms[currentRoomNumber];
                            rooms[currentRoomNumber]._westRoom = rooms[selectRoomNumber];
                            break;
                        case 3:
                        default:
                            if (rooms[selectRoomNumber]._westRoom != null)
                            {
                            }
                            rooms[selectRoomNumber]._westRoom = rooms[currentRoomNumber];
                            rooms[currentRoomNumber]._eastRoom = rooms[selectRoomNumber];
                            break;
                    }
                    roomStack.Push(currentRoomNumber);
                    currentRoomNumber++;
                }
            }
        }
    }
    protected override void Initialize()
    {
        _player = FindObjectOfType<Player>();
        _playerInfoUI = FindObjectOfType<PlayerInfoUI>(true);
        ItemInventory itemInventory = new ItemInventory(_inventorySlotAmount, _player);
        _invenUI = FindObjectOfType<ItemInventoryUI>(true);
        _invenUI.Initialize(itemInventory);
        StartCoroutine(Monster_Spawn(10f));
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

    private IEnumerator Monster_Spawn(float intervalTime)
    {
        while(true)
        {
            yield return new WaitForSeconds(intervalTime);
            Monster_Base mob = SpawnManager_Monster.Instance.GetObject(Monster_Type.WildBoar);
            mob.transform.position = new Vector3(UnityEngine.Random.Range(_randomSpawnArea_minX, _randomSpawnArea_maxX), UnityEngine.Random.Range(_randomSpawnArea_minY, _randomSpawnArea_maxY), 0f);
        }
    }

    private T[] SuffleArray<T> (T[] array)
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
