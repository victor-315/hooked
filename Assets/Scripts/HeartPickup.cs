using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public int healAmount = 20;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object touching the heart is the player
        PlayerHealthBar playerHealth = other.GetComponent<PlayerHealthBar>();

        if (playerHealth != null)
        {
            // Increase health but don't exceed max health
            playerHealth.currentHealth = Mathf.Min(
                playerHealth.currentHealth + healAmount,
                playerHealth.maxHealth
            );

            // Update the health bar UI
            playerHealth.healthBar.SetHealth(playerHealth.currentHealth);

            // Destroy the heart after pickup
            Destroy(gameObject);
        }
    }
}