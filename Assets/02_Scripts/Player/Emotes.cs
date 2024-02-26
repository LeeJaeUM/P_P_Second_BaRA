using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Emotes : MonoBehaviour
{

    EmoteInputActions inputActions;
    Animator anim;
    [SerializeField]bool isLie = false;

    private void Awake()
    {
        inputActions = new EmoteInputActions();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Emote.EmoteAble.performed += EmoteAble;
        inputActions.Emote.Lie.performed += OnLie;
    }
    private void OnDisable()
    {
        inputActions.Emote.Lie.performed -= OnLie;
        inputActions.Emote.EmoteAble.performed -= EmoteAble;
        inputActions.Disable();
    }

    private void OnLie(InputAction.CallbackContext context)
    {
        
        anim.SetBool("isLie", isLie);
    }

    private void EmoteAble(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isLie = !isLie;
        //Debug.Log("L123");
        //if (isTest)
        //{
        //    // ���� Alt Ű�� ���� �ִ� ��쿡�� �� �ڵ� ����� ����˴ϴ�.
        //    Debug.Log("Left Alt key is pressed!");
        //   // anim.SetBool("isLie", true);
        //}
    }
}
