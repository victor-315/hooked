using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelText : MonoBehaviour
{
    public float displayTime = 2f;
    public TextMeshProUGUI text;
    public int levelNumber;

    void Start()
    {
        text.text = "Level " + levelNumber;
        Invoke(nameof(HideText), displayTime);
    }

    void HideText()
    {
        gameObject.SetActive(false);
    }
}
