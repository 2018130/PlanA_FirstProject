using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBackground : MonoBehaviour
{
    [SerializeField]
    GameObject twinkleStarPrefab;
    [SerializeField]
    GameObject shootingStarPrefab;

    float shootingStarTimer = 0f;
    float shootingStarSpawnTime = 3f;
    float shootingStarMinSpawnPosY = 6f;
    float shootingStarMaxSpawnPosY = 10f;
    float shootingStarMinSpawnPosX = 0f;
    float shootingStarMaxSpawnPosX = 10f;

    float twinkleStarTimer = 1.5f;
    float twinkleStarSpawnTime = 4f;
    float twinkleStarMinSpawnPosY = 2f;
    float twinkleStarMaxSpawnPosY = 10f;
    float twinkleStarMinSpawnPosX = -6f;
    float twinkleStarMaxSpawnPosX = 10f;

    private void Update()
    {
        shootingStarTimer += Time.deltaTime;
        twinkleStarTimer += Time.deltaTime;

        if (shootingStarTimer >= shootingStarSpawnTime)
        {
            GameObject shootingStar = Instantiate(shootingStarPrefab,
                new Vector3(Random.Range(shootingStarMinSpawnPosX, shootingStarMaxSpawnPosX), Random.Range(shootingStarMinSpawnPosY, shootingStarMaxSpawnPosY), 0), Quaternion.identity);

            shootingStarTimer = 0;
        }

        if (twinkleStarTimer >= twinkleStarSpawnTime)
        {
            GameObject twinkleStar = Instantiate(twinkleStarPrefab,
                new Vector3(Random.Range(twinkleStarMinSpawnPosX, twinkleStarMaxSpawnPosX), Random.Range(twinkleStarMinSpawnPosY, twinkleStarMaxSpawnPosY), 0), Quaternion.identity);

            twinkleStarTimer = 0;
        }
    }
}
