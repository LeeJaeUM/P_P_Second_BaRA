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
            // ���� Alt Ű�� ���� �ִ� ��쿡�� �� �ڵ� ����� ����˴ϴ�.
            Debug.Log("Left Alt key is pressed!");
           // anim.SetBool("isLie", true);
        }
    }
}
