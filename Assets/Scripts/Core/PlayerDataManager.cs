using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    [SerializeField]
    float _first_hp = 100;
    float _hp = 100;
    public float HP => _hp;
    int _money = 0;
    public int Money => _money;
    protected override void RunOnlyOnce_Initialize()
    {
        if (_initialized == false)
        {
            _hp = _first_hp;
        }
    }

    protected override void Initialize()
    {
        Player player = FindObjectOfType<Player>();
    }
}
