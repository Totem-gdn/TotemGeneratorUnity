using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TotemEntities
{
    public class TotemUser
    {
        public string PublicKey { get; private set; }
        private List<TotemSpear> OwnedSpears { get; }
        private List<TotemAvatar> OwnedAvatars { get; }
        private List<TotemSword> OwnedSwords { get; }
        private readonly string _name;
        private readonly string _pwd;

        public TotemUser(string aName, string aPwd, string publicKey = "")
        {
            _name = aName;
            _pwd = aPwd;
            PublicKey = publicKey;
            OwnedSpears = new List<TotemSpear>();
            OwnedAvatars = new List<TotemAvatar>();
            OwnedSwords = new List<TotemSword>();
        }

        public TotemUser(string publicKey)
        {
            PublicKey = publicKey;
            OwnedSpears = new List<TotemSpear>();
            OwnedAvatars = new List<TotemAvatar>();
            OwnedSwords = new List<TotemSword>();
        }

        public string GetUserName()
        {
            return _name;
        } 
        
        public void AddSpear(TotemSpear s)
        {
            OwnedSpears.Add(s);
        }

        public void AddAvatar(TotemAvatar a)
        {
            OwnedAvatars.Add(a);
        }

        public void AddSword(TotemSword s)
        {
            OwnedSwords.Add(s);
        }

        public bool Authenticate(string uName, string pwd)
        {
            return pwd == _pwd && uName == _name;
        }

        public bool RemoveAvatar(TotemAvatar a)
        {
            return OwnedAvatars.Remove(a);
        }
        
        public bool RemoveSpear(TotemSpear s)
        {
            return OwnedSpears.Remove(s);;
        }

        public bool RemoveSword(TotemSword s)
        {
            return OwnedSwords.Remove(s); ;
        }

        public List<TotemSpear> GetOwnedSpears()
        {
            return OwnedSpears;
        }
        
        public List<TotemAvatar> GetOwnedAvatars()
        {
            return OwnedAvatars;
        }

        public List<TotemSword> GetOwnedSwords()
        {
            return OwnedSwords;
        }

        public override string ToString()
        {
            var tempAvatars = OwnedAvatars.Aggregate("", (current, ownedAvatar) =>
                (current == "" ? current : current + ", ") + "{" + ownedAvatar + "}");
            var tempSpears = OwnedSpears.Aggregate("", (current, ownedSpear) =>
                (current == "" ? current : current + ", ") + "{" + ownedSpear + "}");
            var tempSwords = OwnedSwords.Aggregate("", (current, ownedSword) =>
                (current == "" ? current : current + ", ") + "{" + ownedSword + "}");

            return $"Name:{GetUserName()}, Avatars: [{tempAvatars}], Spears: [{tempSpears}], Swords: [{tempSwords}]";
        }
    }
}