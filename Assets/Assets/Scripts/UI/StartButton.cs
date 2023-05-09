using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Button button= GetComponent<Button>();
        button.onClick.AddListener(GoLoadingScene);
    }

    // Update is called once per frame
    void GoLoadingScene()
    {
        SceneManager.LoadScene(1);
    }
}
