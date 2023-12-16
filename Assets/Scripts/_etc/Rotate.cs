using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEndRotState
{
    NONE,
    STOP,
    RETURNING,
}

public class Rotate : MonoBehaviour
{
    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private float minDegree = -10f;
    [SerializeField]
    private float maxDegree = 20f;
    [SerializeField]
    private EEndRotState endState = EEndRotState.NONE;

    private int sign = 1;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float nextRotZ = transform.localEulerAngles.z + Time.fixedDeltaTime * speed * sign;
        nextRotZ = nextRotZ > 180 ? nextRotZ - 360 : nextRotZ;
        Vector3 nextRot = new Vector3(0, 0, nextRotZ);

        if (nextRotZ > maxDegree || nextRotZ < minDegree)
        {
            nextRot = transform.localEulerAngles;

            if (endState == EEndRotState.RETURNING)
            {
                sign *= -1;
                Debug.Log(minDegree + " " + maxDegree + " " + transform.localEulerAngles.z);
            }
        }

        transform.localEulerAngles = nextRot;
    }
}
