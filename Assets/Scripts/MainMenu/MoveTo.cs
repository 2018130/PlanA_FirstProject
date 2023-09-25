using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEndState
{
    NONE,
    STOP,
    GOSTARTPOINT,
    GOENDPOINT,
    RETURINGX,
    RETURINGY
}

public class MoveTo : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0f;
    [SerializeField]
    Vector2 startPosition = Vector2.zero;
    [SerializeField]
    Vector2 endPosition = Vector2.zero;
    [SerializeField]
    Vector2 direction = Vector2.zero;
    [SerializeField]
    EEndState eEndState = EEndState.NONE;
    bool bEndOfMove = false;

    // Update is called once per frame
    void Update()
    {
        if (bEndOfMove) return;

        Vector2 nextPos = new Vector2(transform.position.x, transform.position.y) +
            Time.deltaTime * direction.normalized * moveSpeed;

        if (nextPos.x < startPosition.x || nextPos.x > endPosition.x)
        {
            nextPos.x = transform.position.x;
        }
        if (nextPos.y < startPosition.y || nextPos.y > endPosition.y)
        {
            nextPos.y = transform.position.y;
        }

        transform.position = nextPos;

        if ((Mathf.Abs(transform.position.x - startPosition.x) < 0.01f &&
            Mathf.Abs(transform.position.y - startPosition.y) < 0.01f) ||
                (Mathf.Abs(transform.position.x - endPosition.x) < 0.01f &&
            Mathf.Abs(transform.position.y - endPosition.y) < 0.01f))
        {
            switch (eEndState)
            {
                case EEndState.GOSTARTPOINT:
                    {
                        transform.position = startPosition + Time.deltaTime * direction.normalized * moveSpeed * 100;
                        break;
                    }
                case EEndState.GOENDPOINT:
                    {
                        transform.position = endPosition + Time.deltaTime * direction.normalized * moveSpeed * 100;
                        break;
                    }
                case EEndState.STOP:
                    {
                        bEndOfMove = true;
                        break;
                    }
                case EEndState.RETURINGX:
                    {
                        direction.x *= -1;
                        break;
                    }
                case EEndState.RETURINGY:
                    {
                        direction.y *= -1;
                        break;
                    }
            }
        }
    }
}
