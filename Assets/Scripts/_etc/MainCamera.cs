using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    Fishing fishing;
    [SerializeField]
    QuestionUseBait questionUseBait;

    [SerializeField] FishSpawner fishSpawnerCS;
    [SerializeField] FishingHookController hookCtrl;

    bool isFistOfToGameScreenChange = false;
    bool isFistOfToMainScreenChange = false;

    float cameraSpeed = 5f;

    const float mainViewPositionY = 0f;
    const float gameViewPositionY = -26f;
    public float GameViewPostionY
    {
        get { return gameViewPositionY; }
    }
    private void Update()
    {
        //카메라 게임뷰로 다 내려가면 패널 전시
        if (isFistOfToGameScreenChange)
        {
            transform.position -= new Vector3(0, Time.unscaledDeltaTime * cameraSpeed, 0);

            if (transform.position.y <= gameViewPositionY)
            {
                transform.position = new Vector3(transform.position.x, gameViewPositionY, transform.position.z);
                questionUseBait.ActiveToViewport();
                isFistOfToGameScreenChange = false;
                fishSpawnerCS.RestartSpawn();
            }
        }

        //카메라 메인메뉴로 거의 다 올라가면 fishing삭제
        if (isFistOfToMainScreenChange)
        {
            transform.position += new Vector3(0, Time.unscaledDeltaTime * cameraSpeed, 0);
            Debug.Log(transform.position);
            hookCtrl.MoveDefault();
            if (transform.position.y >= mainViewPositionY)
            {
                transform.position = new Vector3(transform.position.x, mainViewPositionY, transform.position.z);
                fishSpawnerCS.ResetSpawn();
                fishing.DeactiveGameObject();
                isFistOfToMainScreenChange = false;
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
