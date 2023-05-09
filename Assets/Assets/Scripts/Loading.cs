using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Loading : MonoBehaviour
{
    AsyncOperation loadAsync;
    Slider loadingBar; 

    void Awake()
    {
        StartCoroutine(LoadScene());
    }

    private void Start()
    {
        loadingBar = FindObjectOfType<Slider>();
    }

    void Update()
    {
        
    }

    IEnumerator LoadScene()
    {
        loadAsync = SceneManager.LoadSceneAsync(2);
        loadAsync.allowSceneActivation = false;
        while (loadAsync.progress < 0.9f)
        {
            yield return null;
        }
    }
}
