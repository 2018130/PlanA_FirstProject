using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFishSpawner : MonoBehaviour
{
    public static NewFishSpawner S_NewFishSpawner;

    [SerializeField]
    GameObject fishPrefab;

    Queue<GameObject> queue_readySpawnFish = new Queue<GameObject>();

    Dictionary<int, List<int>> fishSpawnIDByWaterDepth = new Dictionary<int, List<int>>();
    int waterDepthLv = 1;
    List<int> fishingLineLenthToChangeWaterDepth = null;

    PlayerController playerController;

    bool isCorutineRun = false;

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.SPlayerController;
        fishingLineLenthToChangeWaterDepth = new List<int>() { 0, 1500, 3000, PlayerController.SPlayerController.MaxFishingLineLenth };

        S_NewFishSpawner = this;

        fishSpawnIDByWaterDepth.Add(1, new List<int>() { 1, 2, 4,7,8,10,11,12,16,17,18,21,22,24,25,26,27,29,30,35,46
        });
        fishSpawnIDByWaterDepth.Add(2, new List<int>() { 3, 9,13,14,15,19,23,28,31,32,34,36,46,47
        });
        fishSpawnIDByWaterDepth.Add(3, new List<int>() { 5,6,20,33,37,38,39,40,41,42,43,44,45,47
        });
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

        for (int i = 0; i < fishingLineLenthToChangeWaterDepth.Count - 1; i++)
        {
            if (fishingLineLenthToChangeWaterDepth[i] <= Fishing.SFishing.FishingLineLenth &&
                fishingLineLenthToChangeWaterDepth[i + 1] > Fishing.SFishing.FishingLineLenth)
            {
                waterDepthLv = i + 1;
            }
        }
        SpawnFish(waterDepthLv);
        isCorutineRun = false;
    }

    void SpawnFish(int waterDepthLv)
    {
        if (queue_readySpawnFish.Count == 0)
        {
            CreateNewFish(waterDepthLv);
        }
        else
        {
            GameObject fish = queue_readySpawnFish.Dequeue();
            float spawnableMaxPosY = transform.position.y + transform.localScale.y / 2;
            float spawnableMinPosY = transform.position.y - transform.localScale.y / 2;
            FishData newFishData = GetRandomFishData(waterDepthLv);

            fish.GetComponent<NewFishMove>().SpawnFish(new Vector2(transform.position.x, Random.Range(spawnableMaxPosY, spawnableMinPosY)), newFishData); ;
        }
    }

    GameObject CreateNewFish(int waterDepthLv)
    {
        float spawnableMaxPosY = transform.position.y + transform.localScale.y / 2;
        float spawnableMinPosY = transform.position.y - transform.localScale.y / 2;
        GameObject newFish = Instantiate(fishPrefab, null);
        FishData newFishData = GetRandomFishData(waterDepthLv);

        newFish.GetComponent<NewFishMove>().SpawnFish(new Vector2(transform.position.x, Random.Range(spawnableMaxPosY, spawnableMinPosY)), newFishData);

        return newFish;
    }

    FishData GetRandomFishData(int waterDepthLV)
    {
        if (!fishSpawnIDByWaterDepth.ContainsKey(waterDepthLV)) return null;

        List<int> fishDataIds = fishSpawnIDByWaterDepth[waterDepthLV];
        int randomId = fishDataIds[Random.Range(0, fishDataIds.Count)];

        FishData newFishData = FishDataBundle.CopyFishData(randomId);
        return newFishData;
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
