using UnityEngine;

public class gravitycontroller : MonoBehaviour
{
    public Rigidbody2D rb;
    public float gravityScaleWhenFalling = 3f;
    public float gravityScaleWhenIdle = 0f;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.y > -2.78f)
        {
            // Enable gravity so the fish drops
            rb.gravityScale = gravityScaleWhenFalling;
        }
        else
        {
            // Disable gravity so it stays at the water level
            rb.gravityScale = gravityScaleWhenIdle;

            // Optional: stop downward velocity
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }
}
