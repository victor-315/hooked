using UnityEngine;

public class objectinteraction_O : MonoBehaviour
{
    public float damage = 10f;
    public float knockbackForce = 12f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playermovement pm = collision.gameObject.GetComponent<playermovement>();
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (pm != null && rb != null)
            {
                
                Vector2 dir = (collision.transform.position - transform.position).normalized;

                pm.Knockback(dir, knockbackForce);
            }
        }
    }
}
