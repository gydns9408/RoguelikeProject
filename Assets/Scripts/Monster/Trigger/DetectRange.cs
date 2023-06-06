using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRange : MonoBehaviour
{
    bool _detectPlayer = false;
    bool _raycastPlayer = false;
    public bool DetectPlayer => _detectPlayer && _raycastPlayer;
    Transform _transform;
    public Transform Transform
    {
        set => _transform = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        { 
            _detectPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _detectPlayer = false;
            _raycastPlayer = false;
        }
    }

    private void Update()
    {
        if (_detectPlayer)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(_transform.position, GameManager.Instance.Player.Position.position - _transform.position, Mathf.Infinity, LayerMask.GetMask("Player", "Wall"));
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.gameObject.CompareTag("Player"))
                {
                    _raycastPlayer = true;
                }
                else 
                {
                    _raycastPlayer = false;
                }
            }
            else
            {
                _raycastPlayer = false;
            }
        }
    }
}
