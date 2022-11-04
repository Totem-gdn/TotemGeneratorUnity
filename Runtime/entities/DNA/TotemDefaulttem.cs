using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotemEntities.DNA
{
    public class TotemDNADefaultItem
    {
        public Color primary_color;
        public string classical_element;
        public uint damage_nd;
        public uint range_nd;
        public uint power_nd;
        public uint magical_exp;
        public string weapon_material;

        public override string ToString()
        {
            return $"classical_element: {classical_element} | damage_nd: {damage_nd} | range_nd: {range_nd} | power_nd: {power_nd} | " +
                $"magical_exp: {magical_exp} | weapon_material: {weapon_material} | primary_color: {primary_color}";
        }
    }
}
