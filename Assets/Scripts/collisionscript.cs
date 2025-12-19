using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class collisionscript : MonoBehaviour
{
    public string sceneName; // Name of the scene to load

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}