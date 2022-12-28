using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCaptureController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Fish"))
            Debug.Log($"{_collision}를 잡았다!");
    }
}
