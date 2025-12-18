using UnityEngine;

public class ArrowDistance : MonoBehaviour
{
    public Transform player;
    public Transform target; 
    public float rotationSpeed = 200f; // degrees per second
    public float DistanceToTarget { get; private set; }

    void Update()
    {
        if (player == null || target == null) return;

        // Optional: calculate X-only distance
        DistanceToTarget = Mathf.Abs(target.position.x - player.position.x);

        // Direction vector from arrow to target
        Vector2 direction = target.position - player.position;

        // Calculate target rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        // Smoothly rotate toward the target
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
