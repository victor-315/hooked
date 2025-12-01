using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pullup_T : MonoBehaviour
{
    public Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(transition());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator transition()
    {
        
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("game");
    }
}