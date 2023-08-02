using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    [SerializeField]
    float _first_hp = 100;
    float _hp = 100;
    public float HP => _hp;
    [SerializeField]
    float _first_mp = 100;
    float _mp = 100;
    public float MP => _mp;
    int _money = 0;
    public int Money => _money;
    protected override void RunOnlyOnce_Initialize()
    {
        if (_initialized == false)
        {
            _initialized = true;
            _hp = _first_hp;
            _mp = _first_mp;
        }
    }

    protected override void Initialize()
    {
        Player player = FindObjectOfType<Player>();
        player.HPChange(_hp);
        player.MPChange(_mp);
        player.Money = _money;
    }

    public void TempSaveData()
    {
        _hp = GameManager.Instance.Player.HP;
        _mp = GameManager.Instance.Player.MP;
        _money = GameManager.Instance.Player.Money;
    }

}
