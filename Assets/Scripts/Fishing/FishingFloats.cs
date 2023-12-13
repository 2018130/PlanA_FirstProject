using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingFloats : MonoBehaviour
{
    [SerializeField]
    Fishing fishing;

    CircleCollider2D collider2d;

    private void Start()
    {
        collider2d = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NewFishMove newFishMove = collision.GetComponent<NewFishMove>();
        if (newFishMove != null)
        {
            fishing.CatchFish(newFishMove);
            newFishMove.RemoveFish();

            if (fishing.CurrentHealth <= 0)
            {
                fishing.OpenFailPanel();
            }
        }
    }
}
