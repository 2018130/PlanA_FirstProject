using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundMove : MonoBehaviour
{
    [SerializeField]
    float radius = 5f;
    [SerializeField]
    float startDegree = 80f;
    [SerializeField]
    float endDegree = 120f;
    float currentDegree;
    [SerializeField]
    float speed = 1f;
    [SerializeField]
    GameObject roundTarget;
    [SerializeField]
    float resetTime = 55f;

    private void Awake()
    {
        currentDegree = startDegree;
    }

    private void Start()
    {
        StartCoroutine(ResetTargetDegree());
    }

    private void Update()
    {
        if (currentDegree >= startDegree && currentDegree <= endDegree)
        {
            float newPosX = transform.position.x + radius * Mathf.Cos(Mathf.PI / 180 * currentDegree);
            float newPosY = transform.position.y + radius * Mathf.Sin(Mathf.PI / 180 * currentDegree);

            roundTarget.transform.position = new Vector3(newPosX, newPosY);


            currentDegree += Time.deltaTime * speed;
            if (currentDegree >= 360) currentDegree = 0;
        }
    }

    IEnumerator ResetTargetDegree()
    {
        yield return new WaitForSeconds(resetTime);

        currentDegree = startDegree;

        StartCoroutine(ResetTargetDegree());
    }
}
