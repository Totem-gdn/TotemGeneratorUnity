# Totem Spear and Avatar Generator for Unity! 
This Package knows how to randomly generate a Totem Avatar and a Totem Spear from scrath or with specific attributes.

## Instructions
You can install this package in three ways!
### A) Package Manager (GIT Url)

The suggested way to install this package is through the "Unity's Package Manager", with our GIT url. 
This way, you will be notified once new versions get released, and you will be able to download them directly from the Unity Editor.

1/ **The GIT url you need to add to the Package manager is the following** (copy and paste it): **https://github.com/Totem-gdn/TotemGeneratorUnity.git**

2/ In the Unity editor, open **Window -> Package Manager** to open the Package Manager, then click **+ -> Add package from git URL... -> paste the link above**.

**Still having trouble this way?**
<br>
You can always check Unity's docs for how to install from git: https://docs.unity3d.com/Manual/upm-ui-giturl.html

### B) Manual Install
You can also install this package manually, by copying the source files directly into your project's assets folder. (You'd have to do this every time there is a new update.)

### C) Using .unitypackage from our "Release" tab
1/ Head over to our releases tab: https://github.com/Totem-gdn/TotemGeneratorUnity/releases.
Download the requested version .unitypackage.
<br>
2/ In the Unity editor, open **Assets -> Import Package -> Custom Package... -> navigate and select downloaded package** and Voilà!


**You can check the manual: https://docs.unity3d.com/Manual/upm-ui-giturl.html**
<br>
---

## Example

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities; // (avatar, spear)

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Generator to create new items
        TotemSpear item = TotemGenerator.GenerateSpear();
        Debug.Log("Got the spear: " + item);
        TotemAvatar avatar = TotemGenerator.GenerateAvatar();
        Debug.Log("Got the avatar: " + avatar);
        
        // To use our MockDB you can use the following:
        TotemMockDB mockDB = new TotemMockDB();
        mockDB.EntitiesDB.GetAllAvatars();
        mockDB.EntitiesDB.GetAllSpears();
        // There are 5 preset users
        mockDB.UsersDB.GetUser("totem1");
        mockDB.UsersDB.GetUser("totem2");
        mockDB.UsersDB.GetUser("totem3");
        mockDB.UsersDB.GetUser("totem4");
        mockDB.UsersDB.GetUser("totem5");
        
        // You can use the TotemEntitiesDB itself as a standalone
        TotemEntitiesDB entitiesDB = new TotemEntitiesDB();
        
        entitiesDB.AddSpear(item);
        entitiesDB.AddAvatar(avatar);
        
        var spears = entitiesDB.GetSpears();
        var avatars = entitiesDB.GetAvatars();
        
        // You can check the TotemUsersDB itself as a standalone
        TotemUsersDB uesrsDB = new TotemUsersDB();
        usersDB.AddUser("example1", "password");
        usersDB.AddSpearToUser("example1", item);
        usersDB.AddAvatarToUser("example1", avatar); 
    }
}
```
---
## Spear and Avatars objects:

### TotemSpear:
```csharp
public class TotemSpear
{
    public TipMaterialEnum tipMaterial;
    public ElementEnum element;
    public Color shaftColor;
    public float range;
    public float damage;

    public TotemSpear(TipMaterialEnum aTip, ElementEnum aElement, Color aShaftColor, float aRange, float aDamage) {
        ...
    }

    public override string ToString() {
        return $"Tip:{tipMaterial},Element:{element},ShaftColor:{shaftColor},Range:{range},Damage:{damage}";
    }
}
```

### TotemAvatar:
```csharp
public class TotemAvatar 
{
    public SexEnum sex;
    public Color skinColor;
    public Color hairColor;
    public HairStyleEnum hairStyle;
    public Color eyeColor;
    public BodyFatEnum bodyFat;
    public BodyMusclesEnum bodyMuscles;

    public TotemAvatar(SexEnum aSex, Color aSkinColor, Color aHairColor, HairStyleEnum aHairStyle, Color aEyeColor, BodyFatEnum aBodyFat, BodyMusclesEnum aBodyMuscles) {
        ...
    }

    public override string ToString() {
        return $"Sex:{sex},SkinColor:{skinColor}HairColor:{hairColor},HairStyle:{hairStyle},EyeColor:{eyeColor},BodyFat:{bodyFat},BodyMuscles:{bodyMuscles}";
    }
}
```

## Enums:
### ElementEnum:
```csharp
public enum ElementEnum {
    Air, 
    Earth,
    Water,
    Fire,
}
```

### HairStyleEnum:
```csharp
public enum HairStyleEnum {
    Afro,
    Asymmetrical,
    Braids,
    Dreadlocks,
    BuzzCut,
    Long,
    Ponytail,
    Short
}
```

### SexEnum:
```csharp
public enum SexEnum {
    Male,
    Female,
}
```

### TipMaterialEnum:
```csharp
public enum TipMaterialEnum {
    Wood=0,
    Bone=1,
    Flint=2,
    Obsidian=3
}
```

### BodyFatEnum
```csharp
public enum BodyFatEnum {
    Thin,
    Fat
}
```

### BodyMusclesEnum
```csharp
public enum BodyMusclesEnum {
    Wimp,
    Muscular
}
```

## Eye, Skin and Hair Colors:
These values can be shown and selected with their own const files that look like that:
### NaturalEyeColors
```csharp
    public static class NaturalEyeColors
    {
        private static List<string> EyeColors { get; } = new List<string>
        {
            "b5d6e0", "90b4ca", "a7ad7f", "7c8b4f", "c4a05f", "a97e33", "7a3411", "3d0d04"
        };
        
        public static Color GetRandom()
        {
            ...
        }
        
        public static Color GetColorByString(string colorHex)
        {
            ...
        }
    }
```
### NaturalSkinColors
```csharp
    public static class NaturalSkinColors 
    {
        private static List<string> HColors { get; } = new List<string>
        {
            "b1b1b1", "070504", "341c0d", "62422e", "914329", "cd622b", "ad7b41", "e4b877"
        };
        
        public static Color GetRandom()
        {
            ...
        }
        
        public static Color GetColorByString(string colorHex)
        {
            ...
        }
    }
```
### NaturalHairColors
```csharp
    public static class NaturalHairColors
    {
        private static List<string> HColors { get; } = new List<string>
        {
            "b1b1b1", "070504", "341c0d", "62422e", "914329", "cd622b", "ad7b41", "e4b877"
        };
        
        public static Color GetRandom()
        {
            ...
        }
        
        public static Color GetColorByString(string colorHex)
        {
            ...
        }
    }
```