using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public static Sound Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public AudioSource backgroundAudioSource;

    public float afternoonTime;
    public float nightTime;

    [SerializeField]
    public AudioClip afternoonClip;

    [SerializeField]
    public AudioClip NightClip;

    [SerializeField]
    public AudioClip WavesClip;

    [SerializeField]
    public AudioClip ChangeSceneToFishingClip;

    [SerializeField]
    public AudioClip TurnTheColletionClip;

    [SerializeField]
    public AudioClip UserTouchClip;

    [SerializeField]
    public List<AudioClip> FishingClips;


    private void Start()
    {
        DontDestroyOnLoad(this);

        backgroundAudioSource = GetComponent<AudioSource>();

        afternoonTime = 65f;
        nightTime = 81f;

        StartCoroutine(PlayAudioClipForDuration(afternoonTime, afternoonClip));
    }

    public IEnumerator PlayAudioClipForDuration(float time, AudioClip clip)
    {
        backgroundAudioSource.clip = clip;
        backgroundAudioSource.Play();
        backgroundAudioSource.PlayOneShot(WavesClip);

        yield return new WaitForSeconds(time);

        if(clip.name == "Afternoon")
        {
            StartCoroutine(PlayAudioClipForDuration(nightTime, NightClip));
        }else if (clip.name == "Night")
        {
            StartCoroutine(PlayAudioClipForDuration(afternoonTime, afternoonClip));
        }
    }

    public void ChangeSceneToFishing()
    {
        StopAllCoroutines();
        backgroundAudioSource.Stop();
        AudioSource.PlayClipAtPoint(ChangeSceneToFishingClip, transform.position, Setting.EffectSound);
    }

    public void PlayTurnTheCollectionClip()
    {
        AudioSource.PlayClipAtPoint(TurnTheColletionClip, transform.position, Setting.EffectSound);
    }

    public void PlayUserTouchClip()
    {
        AudioSource.PlayClipAtPoint(UserTouchClip, transform.position, Setting.EffectSound);
    }
}
