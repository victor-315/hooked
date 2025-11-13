using UnityEngine;

public class playermovement : MonoBehaviour
{
    public float moveSpeed = 5f;        
    public float acceleration = 10f;    
    public float deceleration = 10f;    
    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 velocity;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();

        if (input.x > 0)
            sr.flipX = true;
        else if (input.x < 0)
            sr.flipX = false;
    }

    void FixedUpdate()
    {
        if (input.magnitude > 0)
        {
            Vector2 targetVelocity = input * moveSpeed;
            velocity = Vector2.MoveTowards(velocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            velocity = Vector2.MoveTowards(velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
