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
        //카메라 게임뷰로 다 내려가면 패널 전시
        if (isFistOfToGameScreenChange)
        {
            transform.position = new Vector3(0, gameViewPositionY, -10);
            transform.position = new Vector3(transform.position.x, gameViewPositionY, transform.position.z);
            questionUseBait.ActiveToViewport();
            isFistOfToGameScreenChange = false;
            fishSpawnerCS.RestartSpawn();
        }

        //카메라 메인메뉴로 거의 다 올라가면 fishing삭제
        if (isFistOfToMainScreenChange)
        {
            transform.position = new Vector3(0, mainViewPositionY, -10);
            hookCtrl.MoveDefault();
            transform.position = new Vector3(transform.position.x, mainViewPositionY, transform.position.z);
            fishSpawnerCS.ResetSpawn();
            fishing.DeactiveGameObject();
            isFistOfToMainScreenChange = false;
        }

        //낚시화면에서 카메라 낚시바늘에 고정
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
