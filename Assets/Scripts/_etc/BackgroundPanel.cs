using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundPanel : MonoBehaviour
{
    GameObject parentPanel;

    private void Awake()
    {
        parentPanel = transform.parent.gameObject;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if(parentPanel.GetComponent<AnimatedPanel>() != null)
            {
                parentPanel.GetComponent<AnimatedPanel>().DeactiveToViewport();
            }
        }
#else
        if (Input.touchCount != 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began && 
            !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            if(parentPanel.GetComponent<AnimatedPanel>() != null)
            {
                parentPanel.GetComponent<AnimatedPanel>().DeactiveToViewport();
            }
        }
#endif
    }
}
