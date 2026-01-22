using UnityEngine;

public class SlowAura2D : MonoBehaviour
{
    [Range(0f, 1f)]
    public float slowMultiplier = 0.5f; // 0.5 = 50% speed

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playermovement player = other.GetComponent<playermovement>();
            if (player != null)
            {
                player.moveSpeed *= slowMultiplier;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playermovement player = other.GetComponent<playermovement>();
            if (player != null)
            {
                player.moveSpeed /= slowMultiplier;
            }
        }
    }
}
