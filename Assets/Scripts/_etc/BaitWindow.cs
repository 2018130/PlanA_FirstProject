using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BaitWindow : MonoBehaviour
{
    public const int Bait_BOX_SIZE = 50;
    public const int Bait_IMAGE_SIZE = 5;
    [SerializeField]
    GameObject boxPrefab;

    [SerializeField]
    public PlayerController playerController;

    GameObject content;
    public GameObject[] boxes = new GameObject[Bait_BOX_SIZE];
    int ownBaitSize = 0;

    [SerializeField]
    public Sprite defaultBoxImage;

    [SerializeField]
    Sprite[] baitImages = new Sprite[Bait_IMAGE_SIZE];

    private void Start()
    {
        content = transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        InitBoxes();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        bool[] inputs = new bool[5];
        inputs[0] = Input.GetKeyDown(KeyCode.Alpha1);
        inputs[1] = Input.GetKeyDown(KeyCode.Alpha2);
        inputs[2] = Input.GetKeyDown(KeyCode.Alpha3);
        inputs[3] = Input.GetKeyDown(KeyCode.Alpha4);
        inputs[4] = Input.GetKeyDown(KeyCode.Alpha5);

        for(int i = 0; i < 5; i++)
        {
            if (inputs[i])
            {
                AddBaitImageInBox(i);
            }
        }
    }

    void InitBoxes()
    {
        for (int i = 0; i < Bait_BOX_SIZE; i++)
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


}
