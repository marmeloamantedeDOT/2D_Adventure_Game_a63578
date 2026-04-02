using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Movimento
    public float speed = 3.0f;
    Rigidbody2D rigidbody2d;
    Vector2 move;

    // Vida
    public int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }

    // Invencibilidade temporária
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Movimento com teclado (WASD + setas)
        move = Vector2.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            move.y += 1f;

        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            move.y -= 1f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            move.x -= 1f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            move.x += 1f;

        // Normalizar diagonal
        if (move.magnitude > 1f)
            move.Normalize();

        // Contador de invencibilidade
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;

            if (damageCooldown <= 0)
                isInvincible = false;
        }
    }

    void FixedUpdate()
    {
        // Movimento com física (Rigidbody2D)
        Vector2 position = rigidbody2d.position + move * speed * Time.fixedDeltaTime;
        rigidbody2d.MovePosition(position);
    }

    // Sistema de vida
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            damageCooldown = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        Debug.Log("Vida: " + currentHealth + "/" + maxHealth);
    }
}