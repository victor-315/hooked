using UnityEngine;

public class FishChasePlayer : MonoBehaviour
{
    public SpriteRenderer sr;

    [Header("Movement Settings")]
    public float swimSpeed = 2f;
    public float splashForce = 7f;
    public float splashInterval = 0.7f;
    public float blendSpeed = 2f;

    [Header("Gravity (Matches Player)")]
    public float normalGravity = 3f;
    public float gravityFadeSpeed = 2f;
    private bool wasAboveWater = false;

    [Header("Detection")]
    public float detectionRadius = 8f;

    private Rigidbody2D rb;
    private Transform player;
    private float splashTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        splashTimer -= Time.fixedDeltaTime;

        float distance = Vector2.Distance(transform.position, player.position);
        bool isAboveWater = transform.position.y > -2.8f;

        //------------------------------------
        // GRAVITY (MATCH PLAYER)
        //------------------------------------
        if (isAboveWater)
        {
            rb.gravityScale = normalGravity;
        }
        else
        {
            rb.gravityScale = Mathf.MoveTowards(
                rb.gravityScale,
                0f,
                gravityFadeSpeed * Time.fixedDeltaTime
            );
        }

        //------------------------------------
        // WATER SPLASH CHECK
        //------------------------------------
        if (wasAboveWater && !isAboveWater)
        {
            // optional splash
        }
        wasAboveWater = isAboveWater;

        //------------------------------------
        // MOVEMENT (UNDERWATER ONLY)
        //------------------------------------
        if (!isAboveWater && distance <= detectionRadius)
        {
            SwimBehavior();
        }
    }

    void SwimBehavior()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // ---- SPLASH BURST ----
        if (splashTimer <= 0f)
        {
            rb.AddForce(direction * splashForce, ForceMode2D.Impulse);
            splashTimer = splashInterval;
        }

        // ---- SMOOTH GLIDE ----
        Vector2 targetVelocity = direction * swimSpeed;

        rb.velocity = Vector2.Lerp(
            rb.velocity,
            targetVelocity,
            blendSpeed * Time.fixedDeltaTime
        );

        // ---- ROTATE (INVERTED FOR FLIPPED SPRITE) ----
        HandleRotation(direction);
    }

    private void HandleRotation(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.01f) return;

        // -----------------------------
        // INVERTED FLIP LOGIC
        // (because sprite is reversed)
        // -----------------------------
        if (dir.x > 0.01f)
            sr.flipX = true;   // WAS false
        else if (dir.x < -0.01f)
            sr.flipX = false;  // WAS true

        // -----------------------------
        // INVERTED ROTATION LOGIC
        // -----------------------------
        float angle = Mathf.Atan2(dir.y, Mathf.Abs(dir.x)) * Mathf.Rad2Deg;

        // Reverse angle when NOT flipped (opposite of player)
        if (!sr.flipX)
            angle = -angle;

        rb.rotation = angle;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
