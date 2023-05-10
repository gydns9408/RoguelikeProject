using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class Loading : MonoBehaviour
{
    AsyncOperation loadAsync;
    Slider loadingBar;
    TextMeshProUGUI loadingText;

    string[] loadingTexts = new string[] { "Now Loading.", "Now Loading..", "Now Loading...", "Now Loading....", "Now Loading....." };
    float spendTime = 0f;

    public float loadingBar_IncreaseSpeed = 1f;

    void Awake()
    {
        
    }

    private void Start()
    {
        StartCoroutine(LoadScene());
        loadingBar = FindObjectOfType<Slider>();
        loadingText = FindObjectOfType<TextMeshProUGUI>();
        StartCoroutine(loadingBarIncrease());
    }

    private void Update()
    {
        spendTime += Time.deltaTime * 2;
        spendTime %= 5;
        loadingText.text = loadingTexts[(int)spendTime];
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

    IEnumerator loadingBarIncrease()
    {
        while (loadingBar.value < 1f)
        {
            if (loadingBar.value <= loadAsync.progress + 0.1f)
            {
                loadingBar.value += Time.deltaTime * loadingBar_IncreaseSpeed;
            }
            yield return null;
        }
        loadAsync.allowSceneActivation = true;
    }
}
