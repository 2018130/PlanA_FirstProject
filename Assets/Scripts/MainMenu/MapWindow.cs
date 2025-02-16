using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapWindow : MonoBehaviour
{
    [SerializeField] private GameObject backgroundPanel;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("1111");
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("2222");
                ToggleWindow(false);
            }
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                ToggleWindow(false);
            }
        }
#endif
    }
    public void ToggleWindow(bool active)
    {
        backgroundPanel.SetActive(active);
    }
}
