# Totem Spear and Avatar Generator for Unity!

This Package knows how to randomly generate a Totem Avatar and a Totem Spear from scrath or with specific attributes.

## Requirements

- Unity Editor 2019.4.9f1 or greater
- .Net Framework 4.x

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

## Building for Android and IOS

Before building the application for Android/IOS you need to generate a deep link which can be easily done by going to **Window > Totem Generator > Generate Deep Link** and typing in your gameId

---

## Samples

You can import samples through "Samples" menu in the Unity package details. **Window -> Package Manager -> Totem Generator for Unity -> Samples**

### Totem Legacy Jam Demo

A sample that demonstrates how to do a basic plugin setup, retrieve avatars and use Legacy records.

---

## Examples

### User login and spears/avatars retrieval

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemServices;
using TotemEntities;

public class test : MonoBehaviour
{
    ///Id of your game
    ///Used for legacy records identification
    private string _gameId = "TotemDemo";

    // Start is called before the first frame update
    void Start()
    {
        //Initialize TotemDB
        TotemDB totemDB = new TotemDB(_gameId);


        //Authenticate user through social login in web browser and get user's assets
        totemDB.AuthenticateUserWithAssets(Provider.GOOGLE, (user) =>
            {
                List<TotemSpear> spears = user.GetOwnedSpears();
                foreach (var spear in spears)
                {
                    Debug.Log(spear.ToString());

                    //Retrieve legacy records of each spear
                    GetLegacyRecords(avatar, (records) =>
                    {
                        Debug.Log($"Records count: {records.Count}")
                    });
                }

                List<TotemAvatar> avatars = user.GetOwnedAvatars();
                foreach (var avatar in avatars)
                {
                    Debug.Log(avatar.ToString());

                    //Retrieve legacy records of each avatar
                    GetLegacyRecords(avatar, (records) =>
                    {
                        Debug.Log($"Records count: {records.Count}")
                    });
                }

            });

    }

    public void AddLegacyRecord(ITotemAsset asset, string data)
    {
        totemDB.AddLegacyRecord(asset, data, (record) =>
        {
            Debug.Log("New legacy record data:" + record.data)
        });
    }

    public void GetLegacyRecords(ITotemAsset asset, UnityAction<List<TotemLegacyRecord>> onSuccess)
    {
        totemDB.GetLegacyRecords(asset, onSuccess, legacyGameIdInput.text);
    }

}
```

---

## Spear and Avatars objects:

### TotemSpear:

```csharp
public class TotemSpear : ITotemAsset
{
    public string Id { get; set; }
    public List<TotemUser> Owners { get; set; }

    public string shaftColor;
    public TipMaterialEnum tipMaterial;
    public ElementEnum element;
    public Color shaftColorRGB;
    public float range;
    public float damage;

    public TotemSpear(TipMaterialEnum aTip, ElementEnum aElement, Color aShaftColor, float aRange, float aDamage) {
        ...
    }

    public override string ToString() {
        return $"Tip:{tipMaterial},Element:{element},ShaftColor:{shaftColorRGB},Range:{range},Damage:{damage}";
    }
}
```

### TotemAvatar:

```csharp
public class TotemAvatar : ITotemAsset
{
    public string Id { get; set; }
    public List<TotemUser> Owners { get; set; }

    public string skinColor;
    public string hairColor;
    public string eyeColor;
    public string clothingColor;

    public SexEnum sex;
    public Color eyeColorRGB;
    public Color skinColorRGB;
    public Color hairColorRGB;
    public Color clothingColorRGB;
    public HairStyleEnum hairStyle;
    public BodyFatEnum bodyFat;
    public BodyMusclesEnum bodyMuscles;

    public TotemAvatar(SexEnum aSex, Color aSkinColor, Color aHairColor, HairStyleEnum aHairStyle, Color aEyeColor, BodyFatEnum aBodyFat, BodyMusclesEnum aBodyMuscles) {
        ...
    }

    public override string ToString() {
        return $"Sex:{sex},SkinColor:{skinColorRGB}HairColor:{hairColorRGB},HairStyle:{hairStyle},EyeColor:{eyeColorRGB},BodyFat:{bodyFat},BodyMuscles:{bodyMuscles},ClothingColor:{clothingColor}";
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
