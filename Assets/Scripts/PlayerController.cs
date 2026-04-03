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
    public int health { get { return currentHealth; } }
    int currentHealth;

    // Invencibilidade
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    // Animação
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

    // Projetil
    public GameObject projectilePrefab;

    // Áudio
    AudioSource audioSource;
    public AudioClip projectileClip;   // som do projétil
    public AudioClip playerHitClip;    // som quando o jogador recebe dano

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Input clássico usando GetAxis
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        move = new Vector2(horizontal, vertical);

        // Direção para animação
        if (move.magnitude > 0.01f)
        {
            moveDirection = move.normalized;
        }

        // Animações
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // Invencibilidade
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }

        // Disparo
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        // Interação NPC
        if (Input.GetKeyDown(KeyCode.X))
        {
            FindFriend();
        }
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rigidbody2d.position + move * speed * Time.fixedDeltaTime;
        rigidbody2d.MovePosition(newPosition);
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

            // Tocar som de dano
            PlaySound(playerHitClip);
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
        if (projectile != null)
            projectile.Launch(moveDirection, 300);

        animator.SetTrigger("Launch");

        // Tocar som do projétil
        PlaySound(projectileClip);
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

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}