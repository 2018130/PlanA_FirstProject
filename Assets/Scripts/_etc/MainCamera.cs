using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    Fishing fishing;
    [SerializeField]
    QuestionUseBait questionUseBait;
    [SerializeField]
    PlayerController playerController;

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

    const float mainViewPositionY = 0f;
    const float gameViewPositionY = -26f;

    public float GameViewPostionY
    {
        get { return gameViewPositionY; }
    }

    private void Start()
    {
        Camera.main.fieldOfView = 104f;
    }
    private void Update()
    {
        //ī�޶� ���Ӻ�� �� �������� �г� ����
        if (isFistOfToGameScreenChange)
        {
            transform.position = new Vector3(0, gameViewPositionY, -10);
            transform.position = new Vector3(transform.position.x, gameViewPositionY, transform.position.z);
            questionUseBait.ActiveToViewport();
            isFistOfToGameScreenChange = false;
        }

        //ī�޶� ���θ޴��� ���� �� �ö󰡸� fishing����
        if (isFistOfToMainScreenChange)
        {
            transform.position = new Vector3(0, mainViewPositionY, -10);
            transform.position = new Vector3(transform.position.x, mainViewPositionY, transform.position.z);
            fishing.DeactiveGameObject();
            Camera.main.orthographicSize = maxCameraSize;
            isFistOfToMainScreenChange = false;
        }

        //����ȭ�鿡�� ī�޶� ���ùٴÿ� ����
        if (fishing.gameObject.activeSelf)
        {
            float topCamPosY = -27f, bottomCamPosY = -53f;
            float leftCamPosX = -6f, rightCamPosX = 6f;
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
    }

    public void MoveToMainScreen()
    {
        isFistOfToMainScreenChange = true;
    }
}
