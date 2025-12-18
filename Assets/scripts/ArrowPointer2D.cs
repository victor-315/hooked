using UnityEngine;

public class ArrowPointer2D : MonoBehaviour
{
    public Transform player;
    public Transform finish;

    void Update()
    {
        Vector2 direction = finish.position - player.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
