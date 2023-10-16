using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BaitWindow : MonoBehaviour
{
    public const int BAIT_BOX_SIZE = 50;
    public const int BAIT_IMAGE_SIZE = 5;
    [SerializeField]
    GameObject boxPrefab;

    [SerializeField]
    public PlayerController playerController;

    GameObject content;
    public GameObject[] boxes = new GameObject[BAIT_BOX_SIZE];
    public int ownBaitSize = 0;

    [SerializeField]
    public Sprite defaultBoxImage;

    [SerializeField]
    Sprite[] baitImages = new Sprite[BAIT_IMAGE_SIZE];

    public Sprite selectedBaitImage;
    Button baitUpBtn;

    private void Start()
    {
        content = transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        InitBoxes();
        baitUpBtn = transform.Find("BaitUpBtn").GetComponent<Button>();
        baitUpBtn.onClick.AddListener(ChangeBaitImage);
        gameObject.SetActive(false);
    }

    void InitBoxes()
    {
        for (int i = 0; i < BAIT_BOX_SIZE; i++)
        {
            GameObject newBox = Instantiate(boxPrefab, content.transform);
            boxes[i] = newBox;
        }
    }

    public void AddBaitImageInBox(int idx)
    {
        Image boxInnerImg;
        //같은 이미지가 이미 있다면 리턴
        for (int i = 0; i < ownBaitSize; i++)
        {
            boxInnerImg = boxes[i].transform.GetChild(0).GetComponent<Image>();
            if (boxInnerImg.sprite == baitImages[idx]) return;
        }

        boxInnerImg = boxes[ownBaitSize].transform.GetChild(0).GetComponent<Image>();
        boxInnerImg.sprite = baitImages[idx];
        
        ownBaitSize++;
    }

    public void AddRandomBait()
    {
        if (ownBaitSize == BAIT_IMAGE_SIZE) return;

        int preBaitSize = ownBaitSize;
        while (preBaitSize == ownBaitSize)
        {
            int idx = Random.Range(0, BAIT_IMAGE_SIZE);

            AddBaitImageInBox(idx);
        }
    }

    void ChangeBaitImage()
    {
        if (selectedBaitImage == defaultBoxImage) return;

        playerController.SetBaitImage(selectedBaitImage);
    }
}
