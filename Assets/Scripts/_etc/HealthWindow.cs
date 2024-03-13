using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthWindow : MonoBehaviour
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
    public void BuyHealth()
    {
        if (playerController.Coin < Cost) return;

        playerController.Coin = playerController.Coin - Cost;
        playerController.AddHealth();
    }
}
