using System;
using System.Collections.Generic;
using consts;
using DefaultNamespace;
using TotemEntities;
using TotemServices;
using enums;
using UnityEngine;
using UnityEngine.Assertions;
using utilities;

public class TotemEntitiesDB
{
    private List<TotemSpear> _spears;
    private List<TotemAvatar> _avatars;
    private List<TotemSword> _swords;

    public TotemEntitiesDB()
    {
        _spears = new List<TotemSpear>();
        _avatars = new List<TotemAvatar>();
        _swords = new List<TotemSword>();
    }

    public void AddSpear(TotemSpear s)
    {
        _spears.Add(s);
    }
    
    public void AddAvatar(TotemAvatar a)
    {
        _avatars.Add(a);
    }

    public void AddSword(TotemSword s)
    {
        _swords.Add(s);
    }

    public TotemSpear GetSpear(int index)
    {
        Assert.IsTrue(index >= 0 && index < _spears.Count, "Index out of range");
        return _spears[index];
    }

    public List<TotemSpear> GetAllSpears(int? index = null)
    {
        if (index == null)
        {
            return _spears;
        }

        
        return new List<TotemSpear>() {_spears[(int) index]};
    }

    public TotemAvatar GetAvatar(int index)
    {
        Assert.IsTrue(index >= 0 && index < _avatars.Count, "Index out of range");
        return _avatars[index];
    }
    
    public List<TotemAvatar> GetAllAvatars()
    {
        return _avatars;
    }

    public TotemSword GetSword(int index)
    {
        Assert.IsTrue(index >= 0 && index < _swords.Count, "Index out of range");
        return _swords[index];
    }

    public List<TotemSword> GetAllSwords(int? index = null)
    {
        if (index == null)
        {
            return _swords;
        }


        return new List<TotemSword>() { _swords[(int)index] };
    }
}
