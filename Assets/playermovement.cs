using UnityEngine;

public class playermovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    [Header("Gravity")]
    public float normalGravity = 3f;
    public float gravityFadeSpeed = 2f;

    [Header("Knockback")]
    public float knockbackDuration = 0.15f;
    private bool isKnocked = false;
    private float knockTimer = 0f;

    [Header("Effects")]
    public ParticleSystem splashPrefab;

    private bool wasAboveWater = false;

    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 velocity;
    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isKnocked)
            return;

        // INPUT
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // Disable upward movement when above -2.8
        if (transform.position.y > -2.8f && input.y > 0)
            input.y = 0f;

        input.Normalize();

        // Sprite flip
        if (input.x > 0) sr.flipX = true;
        else if (input.x < 0) sr.flipX = false;

        // -------------------------------------------------
        // WATER SPLASH SPAWN
        // -------------------------------------------------
        bool isAboveWater = transform.position.y > -2.8f;

        if (wasAboveWater && !isAboveWater)
        {
            // Spawn splash at water line
            if (splashPrefab != null)
            {
                Instantiate(
                    splashPrefab,
                    new Vector3(transform.position.x, -2.8f, transform.position.z),
                    Quaternion.identity
                );
            }
        }

        wasAboveWater = isAboveWater;

        // -------------------------------------------------
        // GRAVITY LOGIC
        // -------------------------------------------------
        if (isAboveWater)
        {
            rb.gravityScale = normalGravity;
        }
        else
        {
            rb.gravityScale = Mathf.MoveTowards(
                rb.gravityScale,
                0f,
                gravityFadeSpeed * Time.deltaTime
            );
        }
    }

    void FixedUpdate()
    {
        if (isKnocked)
        {
            knockTimer -= Time.fixedDeltaTime;

            if (knockTimer <= 0f)
            {
                isKnocked = false;
                col.enabled = true;
            }

            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            return;
        }

        Vector2 targetVelocity = input * moveSpeed;

        velocity = Vector2.MoveTowards(
            velocity,
            targetVelocity,
            acceleration * Time.fixedDeltaTime
        );

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    public void Knockback(Vector2 direction, float force)
    {
        isKnocked = true;
        knockTimer = knockbackDuration;
        velocity = direction * force;
        col.enabled = false;
    }
}
