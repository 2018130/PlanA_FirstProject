using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGameSceneBtn : MonoBehaviour
{
    [SerializeField]
    GameObject fishing;

    [Space(10f)]
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject toMainMenuBtn;
    [SerializeField]
    GameObject questionUseHealthPanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if(playerController != null)
        {
            float duration = playerController.SetAnimation("Fishing");

            Invoke("DisplayQuestionUseHealthPanel", duration);
        }
    }

    void DisplayQuestionUseHealthPanel()
    {
        questionUseHealthPanel.GetComponent<QuestionUseHealth>().ActiveToViewport();
    }
}
