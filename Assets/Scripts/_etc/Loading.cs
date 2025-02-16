using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField]
    public Slider progress;

    private float timer = 0;
    public float requireLoadTime = 1.0f;

    public void OpenScene(string sceneName)
    {
        StartCoroutine(SetActiveChildUI(true));
        StartCoroutine(LoadSceneProcess(sceneName));
    }

    IEnumerator LoadSceneProcess(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;

            if (timer > requireLoadTime && op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
                StartCoroutine(SetActiveChildUI(false, 0.5f));
                yield break;
            }
            else
            {
                progress.value = timer / requireLoadTime;
            }
            timer += Time.deltaTime;
        }
    }

    IEnumerator SetActiveChildUI(bool active, float waitTime = 0)
    {
        Debug.Log("active : " + active + "WaitTime : " + waitTime);
        yield return new WaitForSeconds(waitTime);
        transform.GetChild(0).gameObject.SetActive(active);
    }
}
