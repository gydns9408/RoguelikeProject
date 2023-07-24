using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Base : MonoBehaviour
{
    public float X_Correction_Value = 0f;
    public float Y_Correction_Value = 0.2f;
    public Arrow Door_Arrow;

    BoxCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.enabled = false;
    }

    public void Open()
    {
        _collider.enabled = true;
    }
}
