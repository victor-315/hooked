using UnityEngine;
using UnityEngine.SceneManagement;

public class playermovement : MonoBehaviour
{
    public Animator animator;

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

    // ---------------- NORMAL DASH ----------------
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

    // ---------------- POWER DASH ----------------
    [Header("Power Dash (Right Click)")]
    public float powerDashSpeed = 23f;
    public float powerDashDuration = 0.18f;
    public float powerDashCooldown = 1f;

    [Range(0f, 1f)]
    public float powerDashMomentumCarry = 0.55f;

    public int maxPowerDashCharges = 2;
    public int currentPowerDashCharges = 2;
    public float powerDashRechargeTime = 1.5f;

    private bool isPowerDashing = false;
    private float powerDashTimer = 0f;
    private float powerDashCooldownTimer = 0f;
    private float powerDashRechargeTimer = 0f;
    private Vector2 powerDashDirection;

    // ------------------------------------------------
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
        if (isKnocked) return;

        // ================================
        // NORMAL DASH INPUT (Left Click)
        // ================================
        if (!isDashing && !isPowerDashing &&
            dashCooldownTimer <= 0f &&
            currentDashCharges > 0 &&
            Input.GetMouseButtonDown(0))
        {
            dashDirection = (input.sqrMagnitude > 0.01f)
                ? input
                : (sr.flipX ? Vector2.left : Vector2.right);

            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;

            currentDashCharges--;
            dashRechargeTimer = dashRechargeTime;

            animator.SetTrigger("dash");
        }

        // NORMAL DASH COOLDOWN
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        // NORMAL DASH RECHARGE
        if (currentDashCharges < maxDashCharges)
        {
            dashRechargeTimer -= Time.deltaTime;
            if (dashRechargeTimer <= 0f)
            {
                currentDashCharges++;
                dashRechargeTimer = dashRechargeTime;
            }
        }

        // ================================
        // POWER DASH INPUT (Right Click)
        // ================================
        if (!isPowerDashing && !isDashing &&
            powerDashCooldownTimer <= 0f &&
            currentPowerDashCharges > 0 &&
            Input.GetMouseButtonDown(1))
        {
            powerDashDirection = (input.sqrMagnitude > 0.01f)
                ? input
                : (sr.flipX ? Vector2.left : Vector2.right);

            isPowerDashing = true;
            powerDashTimer = powerDashDuration;
            powerDashCooldownTimer = powerDashCooldown;

            currentPowerDashCharges--;
            powerDashRechargeTimer = powerDashRechargeTime;

            animator.SetTrigger("dash");
        }

        // POWER DASH COOLDOWN
        if (powerDashCooldownTimer > 0f)
            powerDashCooldownTimer -= Time.deltaTime;

        // POWER DASH RECHARGE
        if (currentPowerDashCharges < maxPowerDashCharges)
        {
            powerDashRechargeTimer -= Time.deltaTime;
            if (powerDashRechargeTimer <= 0f)
            {
                currentPowerDashCharges++;
                powerDashRechargeTimer = powerDashRechargeTime;
            }
        }

        // ================================
        // INPUT
        // ================================
        if (!isDashing && !isPowerDashing)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (transform.position.y > -2.8f && input.y > 0)
                input.y = 0f;

            input.Normalize();
        }

        animator.SetFloat("speed", Mathf.Abs(input.x));

        if (input.x > 0) sr.flipX = false;
        else if (input.x < 0) sr.flipX = true;

        // WATER SPLASH
        bool isAboveWater = transform.position.y > -2.8f;
        if (wasAboveWater && !isAboveWater && splashPrefab)
        {
            Instantiate(splashPrefab,
                new Vector3(transform.position.x, -2.8f, transform.position.z),
                Quaternion.identity
            );
        }
        wasAboveWater = isAboveWater;

        // GRAVITY
        rb.gravityScale = isAboveWater
            ? normalGravity
            : Mathf.MoveTowards(rb.gravityScale, 0f, gravityFadeSpeed * Time.deltaTime);
    }


    void FixedUpdate()
    {
        // =====================================
        // NORMAL DASH MOVEMENT
        // =====================================
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

        // =====================================
        // POWER DASH MOVEMENT
        // =====================================
        if (isPowerDashing)
        {
            powerDashTimer -= Time.fixedDeltaTime;

            rb.MovePosition(rb.position + powerDashDirection * powerDashSpeed * Time.fixedDeltaTime);

            if (powerDashTimer <= 0f)
            {
                isPowerDashing = false;
                velocity = powerDashDirection * powerDashSpeed * powerDashMomentumCarry;
            }
            return;
        }

        // =====================================
        // KNOCKBACK
        // =====================================
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

        // =====================================
        // NORMAL MOVEMENT
        // =====================================
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
