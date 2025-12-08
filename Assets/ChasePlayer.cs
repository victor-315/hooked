using UnityEngine;

public class FishChasePlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float swimSpeed = 2f;           // continuous slow movement
    public float splashForce = 7f;         // burst impulse
    public float splashInterval = 0.7f;
    public float blendSpeed = 2f;          // how fast the glide slows back into normal swim

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

        if (distance > detectionRadius)
        {
            return; // fish idle if too far
        }

        Vector2 direction = (player.position - transform.position).normalized;

        // ---- SPLASH BURSTS ----
        if (splashTimer <= 0f)
        {
            rb.AddForce(direction * splashForce, ForceMode2D.Impulse);
            splashTimer = splashInterval;
        }

        // ---- SMOOTH GLIDE + SWIM ----
        // DO NOT override velocity — only blend toward swimSpeed softly
        Vector2 targetVelocity = direction * swimSpeed;

        rb.velocity = Vector2.Lerp(
            rb.velocity,           // current glide/burst speed
            targetVelocity,        // normal swim speed
            blendSpeed * Time.fixedDeltaTime
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
