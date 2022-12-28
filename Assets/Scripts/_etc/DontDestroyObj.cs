using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyObj : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyObj[] dontDestroyObjs = FindObjectsOfType<DontDestroyObj>();

        if(dontDestroyObjs.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
