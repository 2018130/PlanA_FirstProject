using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public enum ESelectedWindow
{
    FISHINGLINE,
    HEALTH,
    BAIT,
    FISHBOWL
}
public class Bag : MonoBehaviour
{
    int childCount = 4;
    GameObject[] windows;
    GameObject backgroundPanel;

    ESelectedWindow eSelectedWindow = ESelectedWindow.FISHINGLINE;


    // Start is called before the first frame update
    void Start()
    {
        windows = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            windows[i] = transform.GetChild(i + 1).gameObject;
        }
        backgroundPanel = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                CloseWindow();
            }
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                CloseWindow();
            }
        }
#endif
    }

    void CloseWindow()
    {
        ActiveOnlySelectedWindow(-1);
        windows[3].GetComponent<Fishbowl>().SaveItemInfoToPlayerPrefs();
        backgroundPanel.SetActive(false);
    }

    public void OpenWindow(int selectedWindow)
    {
        backgroundPanel.SetActive(true);

        switch (selectedWindow)
        {
            //FISHINGLINE
            case 0:
                {
                    ActiveOnlySelectedWindow(0);
                    break;
                }
            //HEALTH
            case 1:
                {
                    ActiveOnlySelectedWindow(1);
                    break;
                }
            //BAIT
            case 2:
                {
                    ActiveOnlySelectedWindow(2);
                    break;
                }
            //FISHBOWL
            case 3:
                {
                    windows[3].GetComponent<Fishbowl>().VisualizeBoxesWithItemInfo();
                    ActiveOnlySelectedWindow(3);
                    break;
                }
        }
    }

    void ActiveOnlySelectedWindow(int index)
    {
        for(int i = 0; i < childCount; i++)
        {
            if(i == index)
            {
                windows[i].SetActive(true);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }
    }
}
