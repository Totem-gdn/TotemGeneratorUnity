using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TotemEntities
{
    public class TotemUser
    {
        public string PublicKey { get; private set; }
        public string PrivateKey { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string ProfileImageUrl { get; private set; }


        public TotemUser(string publicKey)
        {
            PublicKey = publicKey;
        }

        public TotemUser(string name, string email, string profileImageUrl, string publicKey, string privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
            Name = name;
            Email = email;
            ProfileImageUrl = profileImageUrl;
        }
        
    }
}