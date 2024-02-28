using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoadingScene : MonoBehaviour
{
    PlayerInputActions inputActions;

    public string nextSceneName = "LoadSampleScene";

    /// <summary>
    /// 비동기 명령 처리를 위해 필요한 클래스
    /// </summary>
    AsyncOperation async;

    [SerializeField]
    float loadRatio;

    /// <summary>
    /// slider의 value가 증가하는 속도
    /// </summary>
    public float loadingBarSpeed = 1.0f;

    IEnumerator loadingTextCoroutine;

    bool loadingDone = false;

    [SerializeField]Slider loadingSlider;
    TextMeshProUGUI loadingText;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.UI.Click.performed += Press;
        inputActions.UI.AnyKey.performed += Press;
    }

    private void OnDisable()
    {
        inputActions.UI.AnyKey.performed -= Press;
        inputActions.UI.Click.performed -= Press;
        inputActions.Disable();
    }

    private void Start()
    {
        loadingSlider = FindAnyObjectByType<Slider>();
        loadingText = FindAnyObjectByType<TextMeshProUGUI>();

        loadingTextCoroutine = LoadingTextProgress();

        StartCoroutine(loadingTextCoroutine);
        StartCoroutine(AsyncLoadScene());

    }

    private void Update()
    {
        // 슬라이더의 value는 loadRatio가 될 때까지 계속 증가
        if (loadingSlider.value < loadRatio)
        {
            loadingSlider.value += Time.deltaTime * loadingBarSpeed;
        }
    }
    private void Press(InputAction.CallbackContext context)
    {
        async.allowSceneActivation = loadingDone; // 누르면 loadingDone가 true일때 씬 변경
    }

    IEnumerator LoadingTextProgress()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        string[] texts =
        {
            "Loading",
            "Loading .",
            "Loading . .",
            "Loading . . .",
            "Loading . . . .",
            "Loading . . . . .",
        };

        int index = 0;
        while (true)
        {
            loadingText.text = texts[index];
            index++;
            index %= texts.Length;
            yield return wait;
        }
    }

    /// <summary>
    /// 비동기로 씬을 로딩하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator AsyncLoadScene()
    {
        loadRatio = 0.0f;
        loadingSlider.value = loadRatio;

        async = SceneManager.LoadSceneAsync(nextSceneName);     //비동기 로딩 시작
        async.allowSceneActivation = false;                     // 자ㄷㅗㅇ으로 씬 전환되지 않도록 하기

        while(loadRatio < 1.0f)
        {
            loadRatio = async.progress+ 0.1f;  //로딩 진행률에 따라 loadRatio설정
            yield return null;
        }

        //남아있는 슬라이더가 다 찰 때 까지 기다리기
        yield return new WaitForSeconds((1 - loadingSlider.value) / loadingBarSpeed);

        StopCoroutine(loadingTextCoroutine);        //글자 변경 안되게 만들기
        loadingText.text = "Loading\nComplete";     //완료글자 출력
        loadingDone = true;                         //로딩완료
    }
}
