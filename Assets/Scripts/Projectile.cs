using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public float speed = 10f; // velocidade do projétil
    private Vector2 moveDirection;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.gravityScale = 0;     // sem gravidade
        rigidbody2d.linearDamping = 0;    // sem resistência
    }

    void Update()
    {
        // Movimento contínuo usando linearVelocity
        rigidbody2d.linearVelocity = moveDirection * speed;

        // Destruir caso esteja muito longe
        if (transform.position.magnitude > 100f)
            Destroy(gameObject);
    }

    public void Launch(Vector2 direction, float force = 300f)
    {
        if (direction.sqrMagnitude < 0.01f)
        {
            direction = Vector2.up; // padrão para cima se não houver direção
        }

        moveDirection = direction.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.Fix();
        }

        Destroy(gameObject);
    }
}