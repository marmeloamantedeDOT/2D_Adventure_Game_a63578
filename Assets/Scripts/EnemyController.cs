using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2.0f;
    public bool vertical = true;
    public float changeTime = 3.0f;

    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private float timer;
    private int direction = 1;
    bool broken = true;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            direction = -direction;
            timer = changeTime;
        }

        if (animator != null)
        {
            if (vertical)
            {
                animator.SetFloat("Move X", 0f);
                animator.SetFloat("Move Y", direction);
            }
            else
            {
                animator.SetFloat("Move X", direction);
                animator.SetFloat("Move Y", 0f);
            }
        }
    }
    void FixedUpdate()
    {
        if (!broken) return; // inimigo consertado não se move

        Vector2 position = rigidbody2d.position;

        if (vertical)
            position.y += speed * direction * Time.fixedDeltaTime;
        else
            position.x += speed * direction * Time.fixedDeltaTime;

        rigidbody2d.MovePosition(position);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
            player.ChangeHealth(-1);
    }
    public void Fix()
    {
        broken = false;
        GetComponent<Rigidbody2D>().simulated = false;
        animator.SetTrigger("Fixed");
    }

}