using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArmorType", menuName = "Archetypes/Armor")]
public class ArmorArchetype : ItemBase
{
    public int Health;
    public override string Info => $"+{Health} HP";
    public override bool isWeapon => false;
}
