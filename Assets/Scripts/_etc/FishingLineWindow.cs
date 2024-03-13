using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FishingLineWindow : MonoBehaviour
{
    PlayerController playerController;
    int cost;
    public int Cost
    {
        get => cost;

        set
        {
            cost = value;

            transform.GetChild(4).GetComponent<Text>().text = cost.ToString() + "G";
        }
    }

    private void Start()
    {
        playerController = PlayerController.SPlayerController;
        Cost = 1000;
    }
    public void BuyFisingLine()
    {
        Debug.Log("Call buy fisingline coin : " + playerController.Coin);
        if (playerController.Coin < Cost) return;

        Debug.Log("enough coin");
        playerController.Coin = playerController.Coin - Cost;
        playerController.AddFishingLineLenth();
    }
}
