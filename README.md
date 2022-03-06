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

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TotemSpear item = TotemGenerator.GenerateSpear();
        Debug.Log("Got the spear: " + item);
        TotemAvatar avatar = TotemGenerator.GenerateAvatar();
        Debug.Log("Got the avatar: " + avatar);
    }
}
```
---
## Spear and Avatars objects:

### TotemSpear:
```csharp
public class TotemSpear
{
    public TipMaterialEnum TipMaterial;
    public ElementEnum Element;
    public Color ShaftColor;
    public float Range;
    public float Damage;

    public TotemSpear(TipMaterialEnum aTip, ElementEnum aElement, Color aShaftColor, float aRange, float aDamage) {
        ...
    }

    override public string ToString() {
        return $"Tip:{TipMaterial},Element:{Element},ShaftColor:{ShaftColor},Range:{Range},Damage:{Damage}";
    }
}
```

### TotemAvatar:
```csharp
public class TotemAvatar 
{
    public SexEnum Sex;
    public Color SkinColor;
    public Color HairColor;
    public HairStyleEnum HairStyle;
    public Color EyeColor;
    public BodyFatEnum BodyFat;
    public BodyMusclesEnum BodyMuscles;

    public TotemAvatar(SexEnum aSex, Color aSkinColor, Color aHairColor, HairStyleEnum aHairStyle, Color aEyeColor, BodyFatEnum aBodyFat, BodyMuscles aBodyMuscles) {
        ...
    }

    override public string ToString() {
        return $"Sex:{Sex},SkinColor:{SkinColor}HairColor:{HairColor},HairStyle:{HairStyle},EyeColor:{EyeColor},BodyFat:{BodyFat},BodyMuscles:{BodyMuscles}";
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
    Buzz,
    Cut,
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