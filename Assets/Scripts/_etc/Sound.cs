using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioSource backgroundAudioSource;

    private void Start()
    {
        backgroundAudioSource = GetComponent<AudioSource>();
    }
}
