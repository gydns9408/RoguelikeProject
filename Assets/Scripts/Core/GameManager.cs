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

    public float _randomSpawnArea_minX = -8.58f;
    public float _randomSpawnArea_maxX = 8.58f;
    public float _randomSpawnArea_minY = -4.89f;
    public float _randomSpawnArea_maxY = 4.89f;

    public uint _inventorySlotAmount = 20;

    bool _invenUIItemSplitMode;
    public bool InvenUIItemSplitMode => _invenUIItemSplitMode;

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
        }
    }
    protected override void Initialize()
    {
        _player = FindObjectOfType<Player>();
        _playerInfoUI = FindObjectOfType<PlayerInfoUI>();
        ItemInventory itemInventory = new ItemInventory(_inventorySlotAmount, _player);
        _invenUI = FindObjectOfType<ItemInventoryUI>();
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
}
