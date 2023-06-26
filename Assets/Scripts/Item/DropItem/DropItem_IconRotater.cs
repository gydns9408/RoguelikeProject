using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem_IconRotater : MonoBehaviour
{
    public float _rotSpeed = 360f;

    private void OnEnable()
    {
        transform.rotation = Quaternion.identity;
    }
    private void Update()
    {
        transform.Rotate(0f, _rotSpeed * Time.deltaTime, 0f);
    }
}
