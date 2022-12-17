using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingFloat : MonoBehaviour
{
    [SerializeField]
    CatchFish catchFish;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fish fishScript = collision.GetComponent<Fish>();
        if(fishScript != null)
        {
            catchFish.SetRewardText(fishScript.PFishType.ToString());
            catchFish.ActiveToViewport();
        }
    }
}
