using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    Fishing fishing;
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    GameObject mainUI;
    [SerializeField]
    GameObject gameUI;

    [SerializeField] FishSpawner fishSpawnerCS;
    [SerializeField] FishingHookController hookCtrl;
    [SerializeField] GameObject fishingFloats;

    bool isFistOfToGameScreenChange = false;
    bool isFistOfToMainScreenChange = false;

    float cameraSpeed = 15f;
    float cameraSizeSpeed = 3f;
    float preCameraPosX = 0f;
    const float maxCameraSize = 12.7f;
    const float minCameraSize = 10f;

    const float mainViewPositionX = 0f;
    const float gameViewPositionX = 100f;

    public float GameViewPostionX
    {
        get { return gameViewPositionX; }
    }

    private void Start()
    {
        Camera.main.fieldOfView = 104f;
    }

    private void Update()
    {
        //ī�޶� ���θ޴��� ���� �� �ö󰡸� fishing����
        if (isFistOfToMainScreenChange)
        {
            transform.position = new Vector3(mainViewPositionX, transform.position.y, transform.position.z);
            fishing.DeactiveGameObject();
            Camera.main.orthographicSize = maxCameraSize;
            isFistOfToMainScreenChange = false;
        }

        //����ȭ�鿡�� ī�޶� ���ùٴÿ� ����
        if (fishing.gameObject.activeSelf)
        {
            float topCamPosY = -100f, bottomCamPosY = 100f;
            float leftCamPosX = -100f, rightCamPosX = 200f;
            if (fishingFloats.gameObject.transform.position.y > bottomCamPosY &&
                fishingFloats.gameObject.transform.position.y < topCamPosY)
            {
                //����� ȯ�濡�� ī�޶��� x��ǥ�� ����� ���� ��� ������ ��ġ�� ���� 
                //�������� �ӵ��� �ݴ밡 �Ǵ� ��� �������� �� ���� 
                //ī�޶� lag�� �ֱ����� ������������ x��ǥ�� ����
                transform.position = new Vector3(gameObject.transform.position.x, fishingFloats.gameObject.transform.position.y, transform.position.z);
            }

            if (fishingFloats.gameObject.transform.position.x > leftCamPosX &&
                fishingFloats.gameObject.transform.position.x < rightCamPosX)
            {
                transform.position = new Vector3(fishingFloats.gameObject.transform.position.x, gameObject.transform.position.y, transform.position.z);
            }

            //���ùٴ��� ȭ�� ����� ���� ȭ�� Ȯ��
            if (Mathf.Abs(0f - preCameraPosX) > Mathf.Abs(0f - transform.position.x))
            {
                if (Camera.main.orthographicSize > minCameraSize)
                {
                    //Camera.main.orthographicSize -= Time.deltaTime * cameraSizeSpeed;
                }
            }
            else
            {
                if (Camera.main.orthographicSize < maxCameraSize)
                {
                    //Camera.main.orthographicSize += Time.deltaTime * cameraSizeSpeed;
                }
            }

            preCameraPosX = transform.position.x;
        }
    }

    public void MoveToGameScreen()
    {
        isFistOfToGameScreenChange = true;
        fishing.gameObject.SetActive(true);
        mainUI.SetActive(false);
        gameUI.SetActive(true);
    }

    public void MoveToMainScreen()
    {
        isFistOfToMainScreenChange = true;
        fishing.gameObject.SetActive(false);
        mainUI.SetActive(true);
        gameUI.SetActive(false);
    }
}
