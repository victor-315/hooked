using UnityEngine;

public class BoatFollowPlayer2D : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform player;      // Player to follow
    [SerializeField] private float followSpeed = 3f; // How fast boat catches up
    [SerializeField] private float xOffset = 2f;     // Distance behind the player

    private Rigidbody2D rb;
    private float fixedY;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        fixedY = rb.position.y; // Lock the Y position
    }

    void FixedUpdate()
    {
        if (!player) return;

        // Target position is to the left of the player
        Vector2 targetPosition = new Vector2(player.position.x - xOffset, fixedY);

        // Smooth movement with Rigidbody2D
        rb.MovePosition(Vector2.Lerp(rb.position, targetPosition, followSpeed * Time.fixedDeltaTime));
    }
}
