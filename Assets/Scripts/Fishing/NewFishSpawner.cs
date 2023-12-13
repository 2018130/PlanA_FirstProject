using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFishSpawner : MonoBehaviour
{
    public static NewFishSpawner S_NewFishSpawner;

    [SerializeField]
    GameObject fishPrefab;

    Queue<GameObject> queue_readySpawnFish = new Queue<GameObject>();

    bool isCorutineRun = false;

    // Start is called before the first frame update
    void Start()
    {
        S_NewFishSpawner = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCorutineRun)
        {
            StartCoroutine(CSpawnFishPerFewSec());
        }
    }

    IEnumerator CSpawnFishPerFewSec()
    {
        isCorutineRun = true;
        float spawnWaitTime = 0.2f;
        yield return new WaitForSeconds(spawnWaitTime);

        SpawnFish();
        isCorutineRun = false;
    }

    void SpawnFish()
    {
        if(queue_readySpawnFish.Count == 0)
        {
            CreateNewFish();
        }
        else
        {
            GameObject fish = queue_readySpawnFish.Dequeue();
            float spawnableMaxPosY = transform.position.y + transform.localScale.y / 2;
            float spawnableMinPosY = transform.position.y - transform.localScale.y / 2;
            fish.GetComponent<NewFishMove>().SpawnFish(new Vector2(transform.position.x, Random.Range(spawnableMaxPosY, spawnableMinPosY)));
        }
    }

    GameObject CreateNewFish()
    {
        float spawnableMaxPosY = transform.position.y + transform.localScale.y / 2;
        float spawnableMinPosY = transform.position.y - transform.localScale.y / 2;
        GameObject newFish = Instantiate(fishPrefab, null);
        newFish.GetComponent<NewFishMove>().SpawnFish(new Vector2(transform.position.x, Random.Range(spawnableMaxPosY, spawnableMinPosY)));

        return newFish;
    }

    public void InsertReadyQueue(GameObject fish)
    {
        queue_readySpawnFish.Enqueue(fish);
    }

    private void OnDisable()
    {
        isCorutineRun = false;
    }
}
