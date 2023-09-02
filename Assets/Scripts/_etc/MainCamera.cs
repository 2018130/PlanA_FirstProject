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
        //카메라 게임뷰로 다 내려가면 패널 전시
        if (isFistOfToGameScreenChange)
        {
            transform.position = new Vector3(0, gameViewPositionY, -10);
            transform.position = new Vector3(transform.position.x, gameViewPositionY, transform.position.z);
            questionUseBait.ActiveToViewport();
            isFistOfToGameScreenChange = false;
        }

        //카메라 메인메뉴로 거의 다 올라가면 fishing삭제
        if (isFistOfToMainScreenChange)
        {
            transform.position = new Vector3(0, mainViewPositionY, -10);
            transform.position = new Vector3(transform.position.x, mainViewPositionY, transform.position.z);
            fishing.DeactiveGameObject();
            Camera.main.orthographicSize = maxCameraSize;
            isFistOfToMainScreenChange = false;
        }

        //낚시화면에서 카메라 낚시바늘에 고정
        if (fishing.gameObject.activeSelf)
        {
            float topCamPosY = -27f, bottomCamPosY = -53f;
            float leftCamPosX = -6f, rightCamPosX = 6f;
            if (fishingFloats.gameObject.transform.position.y > bottomCamPosY &&
                fishingFloats.gameObject.transform.position.y < topCamPosY)
            {
                //모바일 환경에서 카메라의 x좌표를 낚시찌에 맞춘 경우 유저의 터치에 의해 
                //낚시찌의 속도가 반대가 되는 경우 이질감이 듬 따라서 
                //카메라 lag을 주기위해 이전프레임의 x좌표를 따름
                transform.position = new Vector3(gameObject.transform.position.x, fishingFloats.gameObject.transform.position.y, transform.position.z);
            }

            if (fishingFloats.gameObject.transform.position.x > leftCamPosX &&
                fishingFloats.gameObject.transform.position.x < rightCamPosX)
            {
                transform.position = new Vector3(fishingFloats.gameObject.transform.position.x, gameObject.transform.position.y, transform.position.z);
            }

            //낚시바늘이 화면 가운데로 가면 화면 확대
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
