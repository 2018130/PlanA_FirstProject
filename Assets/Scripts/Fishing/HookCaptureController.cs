using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCaptureController : MonoBehaviour
{
    [SerializeField] private Fishing fishing;

    bool isTriggerBlocked = false;

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Fish") && !isTriggerBlocked)
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

    public void SetTriggerBlocked(bool state)
    {
        isTriggerBlocked = state;
    }
}
