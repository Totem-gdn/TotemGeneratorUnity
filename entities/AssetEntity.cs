using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TotemEntities
{
    public interface ITotemAsset
    {
        string Id { get; set; }

        List<TotemUser> Owners { get; set; }

    }
}
