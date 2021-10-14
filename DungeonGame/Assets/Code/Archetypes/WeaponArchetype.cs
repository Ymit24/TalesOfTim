using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponType", menuName = "Archetypes/Weapon")]
public class WeaponArchetype : ItemBase
{
    public BulletArchetype Bullet;

    public override string Info => $"+{Damage} DMG";
    public override bool isWeapon => true;

    public float TimeBetweenShot;
    public int Damage;

    public FiringMethod FireMode;

    public enum FiringMethod
    {
        Single,
        Double,
        Triple,
        Crazy
    }
}
