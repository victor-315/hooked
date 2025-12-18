// File name: FollowPlayerXOnly.cs
using UnityEngine;

public class FollowPlayerXOnly : MonoBehaviour
{
    public Transform player;
    private float xOffset;

    void Start()
    {
        if (player == null) return;
        xOffset = transform.position.x - player.position.x;
    }

    void LateUpdate()
    {
        if (player == null) return;
        Vector3 pos = transform.position;
        pos.x = player.position.x + xOffset;
        transform.position = pos;
    }
}
