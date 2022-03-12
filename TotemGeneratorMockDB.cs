using System.Collections.Generic;
using consts;
using DefaultNamespace;
using entities;
using enums;
using UnityEngine;
using UnityEngine.Assertions;

public class TotemGeneratorMockDB
{
    private List<TotemSpear> _spears = new List<TotemSpear>();
    private List<TotemAvatar> _avatars = new List<TotemAvatar>();

    public TotemGeneratorMockDB()
    {
        _spears.Add(new TotemSpear(TipMaterialEnum.Wood, ElementEnum.Earth, Color.black, 1, 1));
        _spears.Add(new TotemSpear(TipMaterialEnum.Flint, ElementEnum.Air, Color.black, 10, 25));
        _spears.Add(new TotemSpear(TipMaterialEnum.Obsidian, ElementEnum.Fire, Color.black, 100, 100));
        
        _avatars.Add(new TotemAvatar(SexEnum.Male, NaturalSkinColors.GetRandom(), NaturalHairColors.GetRandom(), HairStyleEnum.BuzzCut, NaturalEyeColors.GetRandom(), BodyFatEnum.Thin, BodyMusclesEnum.Wimp));
        _avatars.Add(new TotemAvatar(SexEnum.Female, NaturalSkinColors.GetRandom(), NaturalHairColors.GetRandom(), HairStyleEnum.BuzzCut, NaturalEyeColors.GetRandom(), BodyFatEnum.Fat, BodyMusclesEnum.Wimp));
        _avatars.Add(new TotemAvatar(SexEnum.Female, NaturalSkinColors.GetRandom(), NaturalHairColors.GetRandom(), HairStyleEnum.BuzzCut, NaturalEyeColors.GetRandom(), BodyFatEnum.Thin, BodyMusclesEnum.Muscular));
    }

    public void AddSpear(TotemSpear s)
    {
        _spears.Add(s);
    }
    
    public void AddAvatar(TotemAvatar a)
    {
        _avatars.Add(a);
    }

    public List<TotemSpear> GetSpears(int index = -1)
    {
        if (index >= _spears.Count)
        {
            // TODO: Check if this is the valid way
            throw new AssertionException("[MockDB.GetSpears]", "Index out of range");
        }
        return index < 0 ? _spears : new List<TotemSpear>() {_spears[index]};
    }
    
    public List<TotemAvatar> GetAvatars(int index = -1)
    {
        if (index >= _avatars.Count)
        {
            // TODO: Check if this is the valid way
            throw new AssertionException("[MockDB.GetAvatars]", "Index out of range");
        }
        return index < 0 ? _avatars : new List<TotemAvatar>() {_avatars[index]};
    }

    public TotemSpear GetCommonSpear()
    {
        return GetSpears(0)[0];
    }
    
    public TotemSpear GetRareSpear()
    {
        return GetSpears(1)[0];
    }
    
    public TotemSpear GetEpicSpear()
    {
        return GetSpears(2)[0];
    }
    
    public TotemAvatar GetCommonAvatar()
    {
        return GetAvatars(0)[0];
    }
    
    public TotemAvatar GetRareAvatar()
    {
        return GetAvatars(1)[0];
    }
    
    public TotemAvatar GetEpicAvatar()
    {
        return GetAvatars(2)[0];
    }
}
