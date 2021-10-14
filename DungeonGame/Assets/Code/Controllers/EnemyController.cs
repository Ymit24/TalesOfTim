using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Npc
{
    public static event Action<EnemyArchetype> OnEnemyKilled;
    public EnemyArchetype Archetype;
 
    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;

    public Vector3 BloodSpawnOffset;

    private string currentState = Idle;

    private const string Idle = "Idle";
    private const string Chasing = "Chasing";
    private const string PreAttack = "PreAttack";
    private const string Attack = "Attacking";
    private const string Hurt = "Hurt";
    private const string Dead = "Death";

    private float stateTimer = HurtTime;
    private const float HurtTime = 0.2f;
    private const float DeathTime = 1f;

    private float health;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        health = Archetype.Health;
    }

    private void ChangeState(string newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        animator.Play(newState);
    }

    private void Update()
    {
        spriteRenderer.flipX = PlayerController.Instance.transform.position.x < transform.position.x;
        switch (currentState)
        {
            case Idle:
                {
                    float distance = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
                    if (distance <= Archetype.DetectionRange)
                    {
                        ChangeState(Chasing);
                        break;
                    }
                    break;
                }
            case Hurt:
                {
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0)
                    {
                        ChangeState(Idle);
                    }
                    break;
                }
            case Dead:
                {
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0)
                    {
                        Destroy(gameObject);
                    }
                    break;
                }
            case Chasing:
                {
                    float distance = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
                    if (distance >= Archetype.EscapeDetectionRange)
                    {
                        ChangeState(Idle);
                        break;
                    }
                    else if (distance <= Archetype.AttackRange)
                    {
                        stateTimer = Archetype.PreAttackTime;
                        ChangeState(PreAttack);
                        break;
                    }
                    break;
                }
            case PreAttack:
                {
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0)
                    {
                        stateTimer = Archetype.AttackDuration;
                        ChangeState(Attack);
                        float distance = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);

                        if (distance <= Archetype.AttackRange)
                        {
                            GlobalHitTextController.SpawnHitText(PlayerController.Instance.transform.position + (Vector3)Vector2.up, Archetype.AttackDamage);
                            PlayerController.Instance.Hurt(Archetype.AttackDamage);
                        }
                    }
                    break;
                }
            case Attack:
                {
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0)
                    {
                        ChangeState(Chasing);
                    }
                    break;
                }
            default: { ChangeState("Idle"); break; }
        }
    }
    private void FixedUpdate()
    {
        rigidbody2D.velocity = Vector3.zero;
        switch (currentState)
        {
            case Chasing:
                {
                    float distance = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
                    if (distance < Archetype.EscapeDetectionRange && distance > Archetype.AttackRange)
                    {
                        rigidbody2D.MovePosition(
                                transform.position
                                + (
                                    PlayerController.Instance.transform.position
                                    - transform.position
                                ).normalized * Archetype.MoveSpeed * Time.deltaTime
                            );
                    }
                    break;
                }
        }
    }
    public void TakeDamage()
    {
        if (currentState == Dead) { return; }

        stateTimer = HurtTime;
        ChangeState(Hurt);
        health -= PlayerController.Instance.Damage;
        GlobalHitTextController.SpawnHitText(transform.position + (Vector3)Vector2.up, PlayerController.Instance.Damage);
        SoundEffectController.Instance.PlaySound(Archetype.HitClip);
        Vector3 bloodSpawnLocation =
            transform.position
            + new Vector3(
                BloodSpawnOffset.x * (
                    PlayerController.Instance.transform.position.x > transform.position.x ? -1 : 1
                ),
                BloodSpawnOffset.y,
                BloodSpawnOffset.z
            );
        GameObject bloodEffectGo = Instantiate(Archetype.BloodPrefab);
        bloodEffectGo.transform.position = bloodSpawnLocation;

        if (health <= 0)
        {
            stateTimer = DeathTime;
            SoundEffectController.Instance.PlaySound(Archetype.DeathClip);
            PlayerController.Instance.DefeatedEnemy(Archetype);
            ChangeState(Dead);

            OnEnemyKilled?.Invoke(Archetype);
            GlobalContinuityController.KillNpc(this);
        }
    }
}
