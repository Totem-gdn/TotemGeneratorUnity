using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TotemEntities
{
    public class TotemUser
    {
        private List<TotemSpear> OwnedSpears { get; }
        private List<TotemAvatar> OwnedAvatars { get; }
        private readonly string _name;
        private readonly string _pwd;

        public TotemUser(string aName, string aPwd)
        {
            _name = aName;
            _pwd = aPwd;
            OwnedSpears = new List<TotemSpear>();
            OwnedAvatars = new List<TotemAvatar>();
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

        public bool Authenticate(string uName, string pwd)
        {
            return pwd == _pwd && uName == _name;
        }

        public bool RemoveAvatar(TotemAvatar a)
        {
            var result = OwnedAvatars.Find(av => av.Equals(a));
            if (result == null)
            {
                Debug.Log($"User {_name} doesn't own this avatar");    
            }
            return result != null;
        }
        
        public bool RemoveSpear(TotemSpear s)
        {
            var result = OwnedSpears.Find(sp => sp.Equals(s));
            if (result == null)
            {
                Debug.Log($"User {_name} doesn't own this spear");    
            }
            return result != null;
        }
        
        public override string ToString()
        {
            var tempAvatars = OwnedAvatars.Aggregate("", (current, ownedAvatar) =>
                (current == "" ? current : current + ", ") + "{" + ownedAvatar + "}");
            var tempSpears = OwnedSpears.Aggregate("", (current, ownedSpear) =>
                (current == "" ? current : current + ", ") + "{" + ownedSpear + "}");

            return $"Name:{GetUserName()}, Avatars: [{tempAvatars}], Spears: [{tempSpears}]";
        }
    }
}