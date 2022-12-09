using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FishSpawner : MonoBehaviour
{
    public static FishSpawner Instance;

    public GameObject fishPrefab;

    Queue<GameObject> poolingObjQueue = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        SpawnFish();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnFish();
        }
    }
    private GameObject CreateNewFish()
    {
        GameObject newFish = Instantiate(fishPrefab);
        newFish.transform.position = Instance.transform.position;
        newFish.SetActive(false);
        newFish.transform.SetParent(transform);

        return newFish;
    }

    public static void ReturnFish(GameObject fish)
    {
        Instance.poolingObjQueue.Enqueue(fish);
        fish.transform.position = Instance.transform.position;
        fish.transform.SetParent(Instance.transform);
        fish.SetActive(false);
    }

    public static GameObject SpawnFish()
    {
        GameObject spawnableFish = null;

        if(Instance.poolingObjQueue.Count > 0)
        {
            spawnableFish = Instance.poolingObjQueue.Dequeue();
            spawnableFish.transform.SetParent(null);
            spawnableFish.SetActive(true);
        }else
        {
            spawnableFish = Instance.CreateNewFish();
            spawnableFish.transform.SetParent(null);
            spawnableFish.SetActive(true);
        }
        spawnableFish.GetComponent<Fish>().SetFishStat((EFishType)UnityEngine.Random.Range(0,3));
        float randomHeight = UnityEngine.Random.Range(-Instance.transform.localScale.y / 2, Instance.transform.localScale.y / 2);
        spawnableFish.transform.position = new Vector3(Instance.transform.position.x, Instance.transform.position.y + randomHeight);

        return spawnableFish;
    }
}
