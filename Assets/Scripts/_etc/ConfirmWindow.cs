using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ConfirmWindow: MonoBehaviour
{
    PlayerController playerController;

    private void Start()
    {
        playerController = PlayerController.SPlayerController.GetComponent<PlayerController>();
    }
    public void ExitGame()
    {
        playerController.SavePlayerInfoToJson();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void CloseExitPanel()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
