using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCaptureController : MonoBehaviour
{
    [SerializeField] private Fishing fishing;

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Fish"))
        {
            Debug.Log($"{_collision}를 잡았다!");
            _collision.gameObject.SetActive(false);
            AgentMovement agentMovement = _collision.gameObject.GetComponent<AgentMovement>();
            if(agentMovement != null)
            {
                fishing.CatchFish(agentMovement);
            }
        }
    }
}
