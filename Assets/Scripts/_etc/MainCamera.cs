using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    GameObject fishingFloats;

    private void Start()
    {
        if(!fishingFloats)
        {
            Fishing fishing = FindObjectOfType<Fishing>();
            if(fishing)
            {
                fishingFloats = fishing.transform.GetChild(1).GetChild(0).gameObject;
            }
        }
    }

    private void Update()
    {
        float topCamPosY = -100f, bottomCamPosY = 100f;
        float leftCamPosX = -200f, rightCamPosX = 200f;
        if (fishingFloats.gameObject.transform.position.y > bottomCamPosY &&
            fishingFloats.gameObject.transform.position.y < topCamPosY)
        {
            transform.position = new Vector3(gameObject.transform.position.x, fishingFloats.gameObject.transform.position.y, transform.position.z);
        }
        if (fishingFloats.gameObject.transform.position.x > leftCamPosX &&
            fishingFloats.gameObject.transform.position.x < rightCamPosX)
        {
            transform.position = new Vector3(fishingFloats.gameObject.transform.position.x, gameObject.transform.position.y, transform.position.z);
        }
    }
}
