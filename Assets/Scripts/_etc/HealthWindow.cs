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
        const int addedHealth = 100;
        if (playerController.Coin < Cost || playerController.MaxHealth < addedHealth + playerController.Health)
        {
            return;
        }

        playerController.Health += addedHealth;
        playerController.Coin = playerController.Coin - Cost;

        playerController.SavePlayerInfoToJson();
    }
}
