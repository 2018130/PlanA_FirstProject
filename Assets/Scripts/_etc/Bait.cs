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
        GetComponent<Button>().onClick.AddListener(SetBaitImage);
    }

    private void Start()
    {
        baitWindow = transform.parent.parent.parent.parent.GetComponent<BaitWindow>();
    }

    public void SetBaitImage()
    {
        Sprite baitImage = transform.GetChild(0).GetComponent<Image>().sprite;

        if (baitImage == baitWindow.defaultBoxImage) return;

        baitWindow.playerController.SetBaitImage(baitImage);
    }
}
