using System.Collections.Generic;
using consts;
using DefaultNamespace;
using TotemEntities;
using enums;
using UnityEngine;
using UnityEngine.Assertions;

public class TotemGeneratorMockDB
{
    private List<TotemSpear> _spears = new List<TotemSpear>();
    private List<TotemAvatar> _avatars = new List<TotemAvatar>();

    public TotemGeneratorMockDB()
    {
        ColorUtility.TryParseHtmlString("#5127FC", out var exampleShaft1);
        ColorUtility.TryParseHtmlString("#14984E", out var exampleShaft2);
        
        _spears.Add(new TotemSpear(TipMaterialEnum.Bone, ElementEnum.Water, exampleShaft1, 63.4f, 52.3f));
        _spears.Add(new TotemSpear(TipMaterialEnum.Obsidian, ElementEnum.Earth, exampleShaft2, 44.9f, 70.1f));
        _spears.Add(new TotemSpear(TipMaterialEnum.Obsidian, ElementEnum.Fire, Color.black, 100, 100));

        var skinColor1 = NaturalSkinColors.GetColorByString("8a6743");
        var hairColor1 = NaturalHairColors.GetColorByString("b1b1b1");
        var eyeColor1 = NaturalEyeColors.GetColorByString("b5d6e0");
        
        var skinColor2 = NaturalSkinColors.GetColorByString("ebb77d");
        var hairColor2 = NaturalHairColors.GetColorByString("cd622b");
        var eyeColor2 = NaturalEyeColors.GetColorByString("c4a05f");
        
        var skinColor3 = NaturalSkinColors.GetColorByString("7a3e10");
        var hairColor3 = NaturalHairColors.GetColorByString("62422e");
        var eyeColor3 = NaturalEyeColors.GetColorByString("7c8b4f");
        
        _avatars.Add(new TotemAvatar(SexEnum.Female, skinColor1, hairColor1, HairStyleEnum.Ponytail, eyeColor1, BodyFatEnum.Fat, BodyMusclesEnum.Muscular));
        _avatars.Add(new TotemAvatar(SexEnum.Male, skinColor2, hairColor2, HairStyleEnum.Dreadlocks, eyeColor2, BodyFatEnum.Thin, BodyMusclesEnum.Wimp));
        _avatars.Add(new TotemAvatar(SexEnum.Female, skinColor3, hairColor3, HairStyleEnum.BuzzCut, eyeColor3, BodyFatEnum.Thin, BodyMusclesEnum.Muscular));
    }

    public void AddSpear(TotemSpear s)
    {
        _spears.Add(s);
    }
    
    public void AddAvatar(TotemAvatar a)
    {
        _avatars.Add(a);
    }

    public List<TotemSpear> GetSpears(int? index = null)
    {
        if (index == null)
        {
            return _spears;
        }
        if (index >= _spears.Count)
        {
            // TODO: Check if this is the valid way
            throw new AssertionException("[MockDB.GetSpears]", "Index out of range");
        }
        return new List<TotemSpear>() {_spears[(int) index]};
    }
    
    public List<TotemAvatar> GetAvatars(int? index = null)
    {
        if (index == null)
        {
            return _avatars;
        }
        if (index >= _avatars.Count)
        {
            // TODO: Check if this is the valid way
            throw new AssertionException("[MockDB.GetAvatars]", "Index out of range");
        }
        return new List<TotemAvatar>() {_avatars[(int) index]};
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
