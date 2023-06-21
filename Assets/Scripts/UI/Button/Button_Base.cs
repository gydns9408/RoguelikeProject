using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Base : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        Button button= GetComponent<Button>();
        button.onClick.AddListener(ButtonClickEvent);
    }

    // Update is called once per frame
    protected virtual void ButtonClickEvent()
    {

    }
}
