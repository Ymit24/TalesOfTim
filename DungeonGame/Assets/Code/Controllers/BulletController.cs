using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public BulletArchetype Archetype;
    public Vector2 Direction;

    private Rigidbody2D rigidbody2D;
    private Vector2 spawnLocation;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spawnLocation = transform.position;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(spawnLocation, transform.position) >= Archetype.Distance)
        {
            // Destroy bullet
            Destroy(gameObject);
            return;
        }
        rigidbody2D.MovePosition(transform.position + (Vector3)(Direction * Archetype.MoveSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        EnemyController ctr = collision.gameObject.GetComponent<EnemyController>();
        if (ctr != null)
        {
            ctr.TakeDamage();
        }
        // Do things like damage and animations
    }
}
