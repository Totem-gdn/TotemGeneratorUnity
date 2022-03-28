using consts;
using DefaultNamespace;
using enums;
using TotemEntities;
using UnityEngine;

namespace utilities
{
    public class MockDBsUtil
    {
        public static void PopulateEntitiesDB(TotemEntitiesDB entitiesDB)
        {
            ColorUtility.TryParseHtmlString("#256D7B", out var shaftColor1);
            ColorUtility.TryParseHtmlString("#c81d4a", out var shaftColor2);
            ColorUtility.TryParseHtmlString("#6474b7", out var shaftColor3);
            ColorUtility.TryParseHtmlString("#936529", out var shaftColor4);
            ColorUtility.TryParseHtmlString("#f99595", out var shaftColor5);
            ColorUtility.TryParseHtmlString("#7a7a30", out var shaftColor6);
            ColorUtility.TryParseHtmlString("#91e676", out var shaftColor7);
            ColorUtility.TryParseHtmlString("#bf6345", out var shaftColor8);
            ColorUtility.TryParseHtmlString("#93b71e", out var shaftColor9);
            ColorUtility.TryParseHtmlString("#d7668d", out var shaftColor10);
            ColorUtility.TryParseHtmlString("#7ab1f7", out var shaftColor11);
            ColorUtility.TryParseHtmlString("#9c96a6", out var shaftColor12);
            ColorUtility.TryParseHtmlString("#c899e4", out var shaftColor13);
            ColorUtility.TryParseHtmlString("#d1d21e", out var shaftColor14);
            ColorUtility.TryParseHtmlString("#9acaa1", out var shaftColor15);
            ColorUtility.TryParseHtmlString("#6b0734", out var shaftColor16);
            
            
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Bone, ElementEnum.Water, shaftColor1, 51.25f, 66.98f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Flint, ElementEnum.Fire, shaftColor2, 84.74f, 79.38f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Wood, ElementEnum.Earth, shaftColor3, 45.29f, 46.06f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Obsidian, ElementEnum.Air, shaftColor4, 37.52f, 41.9f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Wood, ElementEnum.Water, shaftColor5, 70.22f, 24.74f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Wood, ElementEnum.Earth, shaftColor6, 72.02f, 60.29f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Wood, ElementEnum.Earth, shaftColor7, 75.14f, 68.62f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Flint, ElementEnum.Water, shaftColor8, 46.34f, 56.66f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Wood, ElementEnum.Air, shaftColor9, 43.9f, 37.75f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Wood, ElementEnum.Fire, shaftColor10, 53.23f, 74.52f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Bone, ElementEnum.Earth, shaftColor11, 44.13f, 67.27f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Obsidian, ElementEnum.Air, shaftColor12, 69.1f, 77.31f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Flint, ElementEnum.Water, shaftColor13, 44.77f, 44.19f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Wood, ElementEnum.Fire, shaftColor14, 42.51f, 47.44f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Bone, ElementEnum.Fire, shaftColor15, 26.80f, 38.94f));
            entitiesDB.AddSpear(new TotemSpear(TipMaterialEnum.Flint, ElementEnum.Air, shaftColor16, 47.97f, 59.5f));

            var skinColor1 = NaturalSkinColors.GetColorByString("472422");
            var hairColor1 = NaturalHairColors.GetColorByString("b1b1b1");
            var eyeColor1 = NaturalEyeColors.GetColorByString("b5d6e0");
            
            var skinColor2 = NaturalSkinColors.GetColorByString("f9d4ab");
            var hairColor2 = NaturalHairColors.GetColorByString("341c0d");
            var eyeColor2 = NaturalEyeColors.GetColorByString("7c8b4f");
            
            var skinColor3 = NaturalSkinColors.GetColorByString("81574b");
            var hairColor3 = NaturalHairColors.GetColorByString("e4b877");
            var eyeColor3 = NaturalEyeColors.GetColorByString("c4a05f");
            
            var skinColor4 = NaturalSkinColors.GetColorByString("efd2c4");
            var hairColor4 = NaturalHairColors.GetColorByString("914329");
            var eyeColor4 = NaturalEyeColors.GetColorByString("90b4ca");
            
            var skinColor5 = NaturalSkinColors.GetColorByString("c58351");
            var hairColor5 = NaturalHairColors.GetColorByString("070504");
            var eyeColor5 = NaturalEyeColors.GetColorByString("a97e33");
            
            var skinColor6 = NaturalSkinColors.GetColorByString("7a3e10");
            var hairColor6 = NaturalHairColors.GetColorByString("cd622b");
            var eyeColor6 = NaturalEyeColors.GetColorByString("a7ad7f");
            
            var skinColor7 = NaturalSkinColors.GetColorByString("8a6743");
            var hairColor7 = NaturalHairColors.GetColorByString("62422e");
            var eyeColor7 = NaturalEyeColors.GetColorByString("3d0d04");
            
            var skinColor8 = NaturalSkinColors.GetColorByString("dca788");
            var hairColor8 = NaturalHairColors.GetColorByString("ad7b41");
            var eyeColor8 = NaturalEyeColors.GetColorByString("7a3411");

            entitiesDB.AddAvatar(new TotemAvatar(SexEnum.Female, skinColor1, hairColor1, HairStyleEnum.Dreadlocks, eyeColor1, BodyFatEnum.Thin, BodyMusclesEnum.Wimp));
            entitiesDB.AddAvatar(new TotemAvatar(SexEnum.Male, skinColor2, hairColor2, HairStyleEnum.BuzzCut, eyeColor2, BodyFatEnum.Fat, BodyMusclesEnum.Muscular));
            entitiesDB.AddAvatar(new TotemAvatar(SexEnum.Male, skinColor3, hairColor3, HairStyleEnum.Asymmetrical, eyeColor3, BodyFatEnum.Thin, BodyMusclesEnum.Muscular));
            entitiesDB.AddAvatar(new TotemAvatar(SexEnum.Female, skinColor4, hairColor4, HairStyleEnum.Braids, eyeColor4, BodyFatEnum.Thin, BodyMusclesEnum.Muscular));
            entitiesDB.AddAvatar(new TotemAvatar(SexEnum.Female, skinColor5, hairColor5, HairStyleEnum.Ponytail, eyeColor5, BodyFatEnum.Fat, BodyMusclesEnum.Wimp));
            entitiesDB.AddAvatar(new TotemAvatar(SexEnum.Male, skinColor6, hairColor6, HairStyleEnum.Afro, eyeColor6, BodyFatEnum.Fat, BodyMusclesEnum.Wimp));
            entitiesDB.AddAvatar(new TotemAvatar(SexEnum.Female, skinColor7, hairColor7, HairStyleEnum.Long, eyeColor7, BodyFatEnum.Fat, BodyMusclesEnum.Muscular));
            entitiesDB.AddAvatar(new TotemAvatar(SexEnum.Male, skinColor8, hairColor8, HairStyleEnum.Short, eyeColor8, BodyFatEnum.Thin, BodyMusclesEnum.Wimp));
        }

        public static void PopulateUsersDB(TotemUsersDB usersDB)
        {
            usersDB.AddNewUser("totem1", "indieverse");
            usersDB.AddNewUser("totem2", "indieverse");
            usersDB.AddNewUser("totem3", "indieverse");
            usersDB.AddNewUser("totem4", "indieverse");
            usersDB.AddNewUser("totem5", "indieverse");
        }

        public static void PopulateUsersWithEntities(TotemEntitiesDB entitiesDB, TotemUsersDB usersDB)
        {
            usersDB.AddAvatarToUser("totem1", entitiesDB.GetAvatar(0));
            
            usersDB.AddAvatarToUser("totem2", entitiesDB.GetAvatar(1));
            usersDB.AddAvatarToUser("totem2", entitiesDB.GetAvatar(2));
            
            usersDB.AddAvatarToUser("totem3", entitiesDB.GetAvatar(3));
            usersDB.AddAvatarToUser("totem3", entitiesDB.GetAvatar(4));
            usersDB.AddAvatarToUser("totem3", entitiesDB.GetAvatar(5));
            
            usersDB.AddAvatarToUser("totem4", entitiesDB.GetAvatar(6));
            
            usersDB.AddAvatarToUser("totem5", entitiesDB.GetAvatar(7));
            
            usersDB.AddSpearToUser("totem1", entitiesDB.GetSpear(0));
            
            usersDB.AddSpearToUser("totem2", entitiesDB.GetSpear(1));
            usersDB.AddSpearToUser("totem2", entitiesDB.GetSpear(2));
            usersDB.AddSpearToUser("totem2", entitiesDB.GetSpear(3));
            usersDB.AddSpearToUser("totem2", entitiesDB.GetSpear(4));
            
            usersDB.AddSpearToUser("totem3", entitiesDB.GetSpear(5));
            usersDB.AddSpearToUser("totem3", entitiesDB.GetSpear(6));
            usersDB.AddSpearToUser("totem3", entitiesDB.GetSpear(7));
            usersDB.AddSpearToUser("totem3", entitiesDB.GetSpear(8));
            usersDB.AddSpearToUser("totem3", entitiesDB.GetSpear(9));
            usersDB.AddSpearToUser("totem3", entitiesDB.GetSpear(10));
            
            usersDB.AddSpearToUser("totem4", entitiesDB.GetSpear(11));
            usersDB.AddSpearToUser("totem4", entitiesDB.GetSpear(12));
            usersDB.AddSpearToUser("totem4", entitiesDB.GetSpear(13));
            
            usersDB.AddSpearToUser("totem5", entitiesDB.GetSpear(14));
            usersDB.AddSpearToUser("totem5", entitiesDB.GetSpear(15));
            
            
        }
    }
}