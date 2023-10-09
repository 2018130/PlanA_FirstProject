using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class Setting : MonoBehaviour
{
    [SerializeField]
    Sound sound;
    Scrollbar backgroundSoundScrollbar;
    Scrollbar effectSoundScrollbar;

    float backgroundSound = 0f;
    float effectSound = 0f;
    private void Awake()
    {
        backgroundSoundScrollbar = transform.GetChild(2).GetChild(0).GetComponent<Scrollbar>();
        effectSoundScrollbar = transform.GetChild(3).GetChild(0).GetComponent<Scrollbar>();

        backgroundSoundScrollbar.onValueChanged.AddListener(OnBackgroundValueChanged);
        effectSoundScrollbar.onValueChanged.AddListener(OnEffectValueChanged);
    }

    private void Start()
    {
        OnBackgroundValueChanged(0.5f);
        OnEffectValueChanged(0.5f);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                gameObject.SetActive(false);
            }
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                gameObject.SetActive(false);
            }
        }
#endif
    }

    void OnBackgroundValueChanged(float value)
    {
        float maxAreaSize = 500f;
        RectTransform fillingArea = backgroundSoundScrollbar.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        fillingArea.sizeDelta = new Vector2(value * maxAreaSize, fillingArea.sizeDelta.y);
        backgroundSound = value;
        sound.backgroundAudioSource.volume = value;
    }

    void OnEffectValueChanged(float value)
    {
        float maxAreaSize = 500f;
        RectTransform fillingArea = effectSoundScrollbar.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        fillingArea.sizeDelta = new Vector2(value * maxAreaSize, fillingArea.sizeDelta.y);
        effectSound = value;
    }
}
