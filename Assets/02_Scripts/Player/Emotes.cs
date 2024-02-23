using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Emotes : MonoBehaviour
{

    EmoteInputActions inputActions;
    GameManager gameManager;
    Animator anim;
    bool isTest = false;

    private void Awake()
    {
        inputActions = new EmoteInputActions();
        gameManager = GameManager.Instance;
        anim = GameManager.Instance.Player.anim;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Emote.EmoteAble.performed += Emote_performed;
    }

    private void Emote_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isTest = !isTest;
        Debug.Log("L123");
        if (isTest)
        {
            // 왼쪽 Alt 키가 눌려 있는 경우에만 이 코드 블록이 실행됩니다.
            Debug.Log("Left Alt key is pressed!");
           // anim.SetBool("isLie", true);
        }
    }
}
