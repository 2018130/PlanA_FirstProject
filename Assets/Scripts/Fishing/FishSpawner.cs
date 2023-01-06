using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] fishPrefab;
    [SerializeField] private float fishInterval = 1f;
    [SerializeField] private GameObject moveField;

    [SerializeField] private Transform fishNext1PointTr;
    [SerializeField] private Transform fishNext2PointTr;
    [SerializeField] private Transform fishEndPointTr;

    private int limitFish = 20;
    private float ranStart1Y = 0f;
    private int fishIdx = 0;
    private int curFish = 0;
    private int boundaryX = 0;
    private int boundaryY = 0;

    private Vector3 fishStartPos = Vector3.zero;

    private void Start()
    {
        boundaryX = (int)moveField.transform.localScale.x;
        boundaryY = (int)moveField.transform.localScale.y;
        fishStartPos = transform.position - new Vector3(-5f, 0f, 0f);
        StartCoroutine("SpawnFish");
    }

    private IEnumerator SpawnFish()
    {
        float boundrayYMin = -(boundaryY * 2.5f - 1);
        float boundrayYMax = (boundaryY * 2.5f + 1);
        while (true)
        {
            yield return new WaitForSeconds(fishInterval);
            fishIdx = Random.Range(0, 3);

            ranStart1Y = Random.Range(boundrayYMin, boundrayYMax);
            fishStartPos.y = (ranStart1Y * 0.1f );

            if (curFish <= limitFish)
            {
                GameObject newFish = Instantiate(fishPrefab[fishIdx], transform.position, Quaternion.identity);
                newFish.GetComponent<AgentMovement>().SetSpawner(GetComponent<FishSpawner>());
                newFish.GetComponent<AgentMovement>().SetBoundary(boundaryX, boundaryY, 
                    transform.position, fishNext1PointTr.position, fishNext2PointTr.position, fishEndPointTr.position);
                newFish.transform.position = fishStartPos;
                newFish.transform.SetParent(this.transform, false);
                newFish.GetComponent<SpriteRenderer>().flipX = false;
                ++curFish;
            }

        }
    }


    public void DestroyCurFish()
    {
        --curFish;
    }
}
