using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Setting : MonoBehaviour
{
    [SerializeField]
    Sound sound;
    Scrollbar backgroundSoundScrollbar;
    Scrollbar effectSoundScrollbar;

    Button exitBtn;

    float backgroundSound = 0f;
    float effectSound = 0f;
    private void Awake()
    {
        backgroundSoundScrollbar = transform.GetChild(2).GetChild(0).GetComponent<Scrollbar>();
        effectSoundScrollbar = transform.GetChild(3).GetChild(0).GetComponent<Scrollbar>();
        exitBtn = transform.GetChild(5).GetComponent<Button>();

        backgroundSoundScrollbar.onValueChanged.AddListener(OnBackgroundValueChanged);
        effectSoundScrollbar.onValueChanged.AddListener(OnEffectValueChanged);
    }

    private void Start()
    {
        sound = Camera.main.GetComponent<Sound>();
        exitBtn.onClick.AddListener(PlayerController.SPlayerController.OpenConfirmPanel);

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
                CloseSettingWindow();
            }
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                CloseSettingWindow();
            }
        }
#endif
    }

    public void OpenSettingWindow()
    {
        gameObject.SetActive(true);
    }

    public void CloseSettingWindow()
    {
        gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            Time.timeScale = 1f;
        }
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
