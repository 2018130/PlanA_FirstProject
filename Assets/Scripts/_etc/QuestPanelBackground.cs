using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestPanelBackground : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;
    QuestPanel questPanel;

    private void Awake()
    {
        questPanel = transform.parent.gameObject.GetComponent<QuestPanel>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            questPanel.DeactiveToViewport();
        }
#else
        if (Input.touchCount != 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began && 
            !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            questPanel.DeactiveToViewport();
        }
#endif
    }
}
