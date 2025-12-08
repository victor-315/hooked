using UnityEngine;

public class FishChasePlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float swimSpeed = 2f;           // normal swim/glide speed
    public float splashForce = 7f;         // burst impulse
    public float splashInterval = 0.7f;
    public float blendSpeed = 2f;          // how fast they slow back down

    [Header("Gravity (Matches Player)")]
    public float normalGravity = 3f;       // gravity above water
    public float gravityFadeSpeed = 2f;    // fade-out underwater
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
        // GRAVITY LIKE THE PLAYER
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
        // WATER SPLASH EFFECT (if you want it)
        //------------------------------------
        if (wasAboveWater && !isAboveWater)
        {
            // optional: spawn splash here if your fish should splash
        }
        wasAboveWater = isAboveWater;

        //------------------------------------
        // MOVEMENT (Only when in water)
        //------------------------------------
        if (!isAboveWater) // fish only chase when underwater
        {
            if (distance <= detectionRadius)
                SwimBehavior();
        }
    }

    void SwimBehavior()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // ---- SPLASH BURSTS ----
        if (splashTimer <= 0f)
        {
            rb.AddForce(direction * splashForce, ForceMode2D.Impulse);
            splashTimer = splashInterval;
        }

        // ---- SMOOTH GLIDE + SWIM ----
        Vector2 targetVelocity = direction * swimSpeed;

        rb.velocity = Vector2.Lerp(
            rb.velocity,
            targetVelocity,
            blendSpeed * Time.fixedDeltaTime
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
