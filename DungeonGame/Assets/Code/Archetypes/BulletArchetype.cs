using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBullet", menuName = "Archetypes/Bullet")]
public class BulletArchetype : ScriptableObject
{
    public GameObject Prefab;
    public float Distance;
    public float MoveSpeed;
}
