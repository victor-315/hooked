using UnityEngine;
using UnityEngine.SceneManagement;

public class playermovement : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public healthbar healthBar;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    [Header("Gravity")]
    public float normalGravity = 3f;
    public float gravityFadeSpeed = 2f;

    [Header("Knockback")]
    public float knockbackDuration = 0.05f;
    private bool isKnocked = false;
    private float knockTimer = 0f;

    [Header("Effects")]
    public GameObject splashPrefab;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;

    [Range(0f, 1f)]
    public float dashMomentumCarry = 0.4f;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector2 dashDirection;

    [Header("Dash Charges")]
    public int maxDashCharges = 5;
    public int currentDashCharges = 5;
    public float dashRechargeTime = 1f;
    private float dashRechargeTimer = 0f;

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
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (isKnocked)
            return;

        // -------------------------------------------------
        // DASH INPUT (requires charge)
        // -------------------------------------------------
        if (!isDashing && dashCooldownTimer <= 0f && currentDashCharges > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            if (input.sqrMagnitude > 0.01f)
                dashDirection = input;
            else
                dashDirection = sr.flipX ? Vector2.right : Vector2.left;

            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;

            currentDashCharges--;
            dashRechargeTimer = dashRechargeTime;
        }

        // Dash cooldown
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        // -------------------------------------------------
        // DASH RECHARGE (1 per second)
        // -------------------------------------------------
        if (currentDashCharges < maxDashCharges)
        {
            dashRechargeTimer -= Time.deltaTime;

            if (dashRechargeTimer <= 0f)
            {
                currentDashCharges++;
                dashRechargeTimer = dashRechargeTime;
            }
        }

        // -------------------------------------------------
        // INPUT (disabled during dash)
        // -------------------------------------------------
        if (!isDashing)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (transform.position.y > -2.8f && input.y > 0)
                input.y = 0f;

            input.Normalize();
        }

        // Sprite flip
        if (input.x > 0) sr.flipX = true;
        else if (input.x < 0) sr.flipX = false;

        // -------------------------------------------------
        // SPLASH WHEN ENTERING WATER
        // -------------------------------------------------
        bool isAboveWater = transform.position.y > -2.8f;

        if (wasAboveWater && !isAboveWater)
        {
            if (splashPrefab != null)
            {
                Instantiate(splashPrefab,
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
            rb.gravityScale = normalGravity;
        else
            rb.gravityScale = Mathf.MoveTowards(rb.gravityScale, 0f, gravityFadeSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        // -------------------------------------------------
        // DASH MOVEMENT
        // -------------------------------------------------
        if (isDashing)
        {
            dashTimer -= Time.fixedDeltaTime;

            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);

            if (dashTimer <= 0f)
            {
                isDashing = false;
                velocity = dashDirection * dashSpeed * dashMomentumCarry;
            }

            return;
        }

        // -------------------------------------------------
        // KNOCKBACK MOVEMENT
        // -------------------------------------------------
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

        // -------------------------------------------------
        // NORMAL MOVEMENT
        // -------------------------------------------------
        Vector2 targetVelocity = input * moveSpeed;

        velocity = Vector2.MoveTowards(
            velocity,
            targetVelocity,
            acceleration * Time.fixedDeltaTime
        );

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        healthBar.SetHealth(currentHealth);
    }

    public void Knockback(Vector2 direction, float force)
    {
        isKnocked = true;
        knockTimer = knockbackDuration;
        velocity = direction * force;
        col.enabled = false;
        TakeDamage(10);
    }
}
