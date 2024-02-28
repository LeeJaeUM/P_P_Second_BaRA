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
    /// �񵿱� ��� ó���� ���� �ʿ��� Ŭ����
    /// </summary>
    AsyncOperation async;

    [SerializeField]
    float loadRatio;

    /// <summary>
    /// slider�� value�� �����ϴ� �ӵ�
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
        // �����̴��� value�� loadRatio�� �� ������ ��� ����
        if (loadingSlider.value < loadRatio)
        {
            loadingSlider.value += Time.deltaTime * loadingBarSpeed;
        }
    }
    private void Press(InputAction.CallbackContext context)
    {
        async.allowSceneActivation = loadingDone; // ������ loadingDone�� true�϶� �� ����
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
    /// �񵿱�� ���� �ε��ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator AsyncLoadScene()
    {
        loadRatio = 0.0f;
        loadingSlider.value = loadRatio;

        async = SceneManager.LoadSceneAsync(nextSceneName);     //�񵿱� �ε� ����
        async.allowSceneActivation = false;                     // �ڤ��Ǥ����� �� ��ȯ���� �ʵ��� �ϱ�

        while(loadRatio < 1.0f)
        {
            loadRatio = async.progress+ 0.1f;  //�ε� ������� ���� loadRatio����
            yield return null;
        }

        //�����ִ� �����̴��� �� �� �� ���� ��ٸ���
        yield return new WaitForSeconds((1 - loadingSlider.value) / loadingBarSpeed);

        StopCoroutine(loadingTextCoroutine);        //���� ���� �ȵǰ� �����
        loadingText.text = "Loading\nComplete";     //�Ϸ���� ���
        loadingDone = true;                         //�ε��Ϸ�
    }
}
