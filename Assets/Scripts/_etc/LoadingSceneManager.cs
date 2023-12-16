using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LoadingSceneManager : MonoBehaviour
{
    float timer = 0f;
    float requireLoadTime = 5f;
    float pointTurnOnTerm = 0.5f;
    int pointIdx = 1;

    GameObject loadText;
    GameObject requireTouchText;

    string[] deactiveGameObjList = { "Text_RequireTouch" };

    private void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        loadText = canvas.transform.GetChild(1).gameObject;
        requireTouchText = canvas.transform.GetChild(2).gameObject;

        Text[] textList = FindObjectsOfType<Text>();
        for(int i = 0; i < textList.Length; i++)
        {
            textList[i].font = Resources.Load<Font>("Fonts/DaPretty");
            
            for(int j = 0; j < deactiveGameObjList.Length; j++)
            {
                if (textList[i].name == deactiveGameObjList[j]) textList[i].gameObject.SetActive(false);
            }
        }

        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("MainScene");
        op.allowSceneActivation = false;

        while(!op.isDone)
        {
            yield return null;

            if(timer > requireLoadTime && op.progress >= 0.9f)
            {
                requireTouchText.SetActive(true);
                loadText.SetActive(false);
                if(Input.GetMouseButtonDown(0))
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
            else
            {
                
                yield return new WaitForSeconds(pointTurnOnTerm);

                SetLodingPoint(pointIdx);
                pointIdx = pointIdx + 1 > 3 ? 1 : pointIdx + 1;

                timer += pointTurnOnTerm;
            }
            timer += Time.deltaTime;
        }
    }
    
    void SetLodingPoint(int count)
    {
        GameObject point1 = loadText.transform.GetChild(1).gameObject;
        GameObject point2 = loadText.transform.GetChild(2).gameObject;
        GameObject point3 = loadText.transform.GetChild(3).gameObject;

        switch(count)
        {
            case 0:
                {
                    point1.SetActive(false);
                    point2.SetActive(false);
                    point3.SetActive(false);
                    break;
                }
            case 1:
                {
                    point1.SetActive(true);
                    point2.SetActive(false);
                    point3.SetActive(false);
                    break;
                }
            case 2:
                {
                    point1.SetActive(true);
                    point2.SetActive(true);
                    point3.SetActive(false);
                    break;
                }
            case 3:
                {
                    point1.SetActive(true);
                    point2.SetActive(true);
                    point3.SetActive(true);
                    break;
                }
        }
    }
}
