using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBackground : MonoBehaviour
{
    static public FishingBackground SFishingBackground;

    float backgroungSpeed = 1f;
    public float BackgroundSpeed
    {
        get
        {
            return backgroungSpeed;
        }
    }

    private void Awake()
    {
        SFishingBackground = this;
    }

    // Update is called once per frame
    void Update()
    {
        float newPosY = transform.position.y + backgroungSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, newPosY, transform.position.z);
    }
}
