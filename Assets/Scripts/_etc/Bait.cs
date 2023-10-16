using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public class Bait : MonoBehaviour
{
    BaitWindow baitWindow;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(GetBaitImage);
    }

    private void Start()
    {
        baitWindow = transform.parent.parent.parent.parent.GetComponent<BaitWindow>();
    }

    public void GetBaitImage()
    {
        Sprite baitImage = transform.GetChild(0).GetComponent<Image>().sprite;

        baitWindow.selectedBaitImage = baitImage;

        if(baitImage != baitWindow.defaultBoxImage)
        {
            for (int i = 0; i < baitWindow.ownBaitSize; i++)
            {
                baitWindow.boxes[i].GetComponent<Image>().color = new Color(1, 1, 1);
            }

            GetComponent<Image>().color = new Color(0.58f, 0.58f, 0.58f);
        }
    }
}
