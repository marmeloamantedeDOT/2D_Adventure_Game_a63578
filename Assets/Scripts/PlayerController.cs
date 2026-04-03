using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Invencibilidade
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    // Animação
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

    // Projetil
    public GameObject projectilePrefab;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // INPUT (GETAXIS)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        move = new Vector2(horizontal, vertical);

        // DIREÇÃO PARA ANIMAÇÃO
        if (move.magnitude > 0.01f)
        {
            moveDirection = move.normalized;
        }

        // ANIMAÇÕES
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // INVENCIBILIDADE
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }

        // DISPARAR
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        // INTERAÇÃO NPC
        if (Input.GetKeyDown(KeyCode.X))
        {
            FindFriend();
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position + move * speed * Time.fixedDeltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        if (projectilePrefab == null) return;

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            rigidbody2d.position + Vector2.up * 0.5f,
            Quaternion.identity
        );

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);

        animator.SetTrigger("Launch");
    }

    void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            rigidbody2d.position + Vector2.up * 0.2f,
            moveDirection,
            1.5f,
            LayerMask.GetMask("NPC")
        );

        if (hit.collider != null)
        {
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();

            if (character != null)
            {
                UIHandler.instance.DisplayDialogue();
            }
        }
    }
}