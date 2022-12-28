using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] fishPrefab;
    [SerializeField] private float fishInterval = 1f;

    private int limitFish = 20;
    private float ranStart1Y = 0f;
    private float ranNext1Y = 0f;
    private int fishIdx = 0;
    private int curFish = 0;

    private Vector3 fishStartPos = new Vector3(-5.2f, 0f, 0f);
    private Vector3 fishNext1Pos = new Vector3(0f,0f,0f);

    private void Start()
    {
        fishStartPos = Vector3.zero; //Ãß°¡
        StartCoroutine("SpawnFish");
    }

    private IEnumerator SpawnFish()
    {
        while (true)
        {
            yield return new WaitForSeconds(fishInterval);
            fishIdx = Random.Range(0, 3);


            ranStart1Y = Random.Range(1, 25);
            if (ranStart1Y < 11) fishStartPos.y = (ranStart1Y * 0.1f);
            else fishStartPos.y = ((ranStart1Y - 11) * 0.1f);

            ranNext1Y = Random.Range(1, 25);
            if (ranNext1Y < 11) fishNext1Pos.y = (ranNext1Y * 0.1f);
            else fishNext1Pos.y = ((ranNext1Y - 11) * 0.1f);




            // +11 12 13 14 15 16  ( 12 - 11 * 0.1)

            if (curFish <= limitFish)
            {
                GameObject newFish = Instantiate(fishPrefab[fishIdx], fishStartPos, Quaternion.identity);
                newFish.transform.SetParent(this.transform, false);
                newFish.GetComponent<SpriteRenderer>().flipX = false;
                newFish.GetComponent<NavMeshAgent>().SetDestination(fishNext1Pos);
                ++curFish;
            }

        }
    }


    public void DestroyCurFish()
    {
        --curFish;
    }
}
