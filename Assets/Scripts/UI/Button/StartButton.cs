using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : Button_Base
{
    protected override void ButtonClickEvent()
    {
        SceneManager.LoadScene(1);
    }
}
