using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyArchetype", menuName = "Archetypes/Enemy")]
public class EnemyArchetype : ScriptableObject
{
    public float Health;
    public GameObject Prefab;
    public GameObject BloodPrefab;

    public AudioClip HitClip, DeathClip;

    public float MoveSpeed;

    public float DetectionRange;
    public float AttackRange;
    public float PreAttackTime;
    public float AttackDuration;
    public int AttackDamage;
    public float EscapeDetectionRange;

    public int XpDrop;
    public int GoldDrop;
}
