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
    [SerializeField] HookCaptureController hookCapture;

    bool isFistOfToGameScreenChange = false;
    bool isFistOfToMainScreenChange = false;

    float cameraSpeed = 15f;

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
            fishSpawnerCS.RestartSpawn();
        }

        //ī�޶� ���θ޴��� ���� �� �ö󰡸� fishing����
        if (isFistOfToMainScreenChange)
        {
            transform.position = new Vector3(0, mainViewPositionY, -10);
            hookCtrl.MoveDefault();
            transform.position = new Vector3(transform.position.x, mainViewPositionY, transform.position.z);
            fishSpawnerCS.ResetSpawn();
            fishing.DeactiveGameObject();
            isFistOfToMainScreenChange = false;
        }

        //����ȭ�鿡�� ī�޶� ���ùٴÿ� ����
        if (fishing.gameObject.activeSelf)
        {
            float topCamPosY = -27f, bottomCamPosY = -53f;
            float leftCamPosX = -6f, rightCamPosX = 6f;
            if (hookCapture.gameObject.transform.position.y > bottomCamPosY &&
                hookCapture.gameObject.transform.position.y < topCamPosY)
            {
                transform.position = new Vector3(gameObject.transform.position.x, hookCapture.gameObject.transform.position.y, transform.position.z);
            }

            if (hookCapture.gameObject.transform.position.x > leftCamPosX &&
                hookCapture.gameObject.transform.position.x < rightCamPosX)
            {
                transform.position = new Vector3(hookCapture.gameObject.transform.position.x, gameObject.transform.position.y, transform.position.z);
            }
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
