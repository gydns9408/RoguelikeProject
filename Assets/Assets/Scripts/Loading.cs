using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class Loading : MonoBehaviour
{
    AsyncOperation _loadAsync;
    Slider _loadingBar;
    TextMeshProUGUI _loadingText;

    string[] _loadingTexts = new string[] { "Now Loading.", "Now Loading..", "Now Loading...", "Now Loading....", "Now Loading....." };
    float _spendTime = 0f;

    public float _loadingBar_IncreaseSpeed = 1f;

    private void Start()
    {
        StartCoroutine(LoadScene());
        _loadingBar = FindObjectOfType<Slider>();
        _loadingText = FindObjectOfType<TextMeshProUGUI>();
        StartCoroutine(loadingBarIncrease());
    }

    private void Update()
    {
        _spendTime += Time.deltaTime * 2;
        _spendTime %= 5;
        _loadingText.text = _loadingTexts[(int)_spendTime];
    }

    IEnumerator LoadScene()
    {
        _loadAsync = SceneManager.LoadSceneAsync(2);
        _loadAsync.allowSceneActivation = false;
        while (_loadAsync.progress < 0.9f)
        {
            yield return null;
        }
    }

    IEnumerator loadingBarIncrease()
    {
        while (_loadingBar.value < 1f)
        {
            if (_loadingBar.value <= _loadAsync.progress + 0.1f)
            {
                _loadingBar.value += Time.deltaTime * _loadingBar_IncreaseSpeed;
            }
            yield return null;
        }
        _loadAsync.allowSceneActivation = true;
    }
}
