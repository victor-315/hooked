using UnityEngine;
using System.Collections;

public class Spawner2D : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnInterval = 1f;

    void Start()
    {
        StartCoroutine(SpawnForever());
    }

    IEnumerator SpawnForever()
    {
        while (true)
        {
            Instantiate(
                objectToSpawn,
                transform.position,
                Quaternion.identity
            );

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
