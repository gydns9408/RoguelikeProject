using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_Etc : Singleton<SpawnManager_Etc>
{
    ObjectPool_DropItem _objectPool_dropItem;
    ObjectPool_ItemIcon _objectPool_itemIcon;
    ObjectPool_DamageText _objectPool_damageText;
    ObjectPool_Wall_Base[] _objectPool_wall_base;

    public Sprite[] DamageSkin_Enemy_Default_Sprites;
    public Sprite[] DamageSkin_Enemy_Critical_Sprites;
    public Sprite[] DamageSkin_Player_Sprites;
    public List<Sprite[]> DamageSkin_Sprites_List;

    Transform _damageTextParent;
    protected override void RunOnlyOnce_Initialize()
    {
        if (_initialized == false)
        {
            _initialized = true;
            _objectPool_dropItem = GetComponentInChildren<ObjectPool_DropItem>();
            _objectPool_itemIcon = GetComponentInChildren<ObjectPool_ItemIcon>();
            _objectPool_damageText = GetComponentInChildren<ObjectPool_DamageText>();
            _objectPool_wall_base = GetComponentsInChildren<ObjectPool_Wall_Base>();
            DamageSkin_Sprites_List = new List<Sprite[]>(3)
            {
                DamageSkin_Enemy_Default_Sprites,
                DamageSkin_Enemy_Critical_Sprites,
                DamageSkin_Player_Sprites
            };
        }
    }

    protected override void Initialize()
    {
        DamageTextParent dtp = FindObjectOfType<DamageTextParent>();
        if (dtp != null) 
        {
            _damageTextParent = dtp.transform;
        }
        if (_objectPool_dropItem != null) 
        {
            _objectPool_dropItem.Initialize();
        }
        if (_objectPool_itemIcon != null)
        {
            _objectPool_itemIcon.Initialize();
        }
        if (_objectPool_damageText != null)
        {
            _objectPool_damageText.Initialize();
        }
        if (_objectPool_wall_base != null)
        {
            foreach (var pool in _objectPool_wall_base)
            {
                pool.Initialize();
            }    
        }
    }

    public DropItem GetObject_DropItem(ItemCode itemCode, uint itemAmount)
    {
        DropItem dropItem = _objectPool_dropItem.GetObject();
        dropItem.DropItemSetting(itemCode, itemAmount);
        return dropItem;
    }

    public ItemIcon GetObject_ItemIcon()
    {
        return _objectPool_itemIcon.GetObject();
    }

    public DamageText GetObject_DamageText(Vector3 wolrdPos)
    {
        DamageText damageText = _objectPool_damageText.GetObject();
        damageText.transform.SetParent(_damageTextParent);
        damageText.transform.position = Camera.main.WorldToScreenPoint(wolrdPos);
        return damageText;
    }

    public Wall_Base GetObject_Wall(WallCode wallCode)
    {
        return _objectPool_wall_base[(int)wallCode].GetObject();
    }

    public void Before_OnDisable()
    {
        _objectPool_itemIcon.Before_OnDisable();
        _objectPool_damageText.Before_OnDisable2();
    }
}
