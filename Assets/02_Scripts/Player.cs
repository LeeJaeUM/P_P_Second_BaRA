using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputActions inputActions;

    public float maxHP = 50;
    [SerializeField] private float curHp;
    public float HP
    {
        get { return curHp; }
        private set 
        {
            if (curHp != value)
            {
                curHp = value;
            }
        }
    }

    public bool isParryAble = false;
    public bool isGuardAble = false;
    public float parryTime_origin = 2f;
    public float parryTime_cur = 0.0f;


    private void Awake()
    {
        inputActions = new();
        HP = maxHP;
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Attack.performed += OnAttackInput;
        inputActions.Player.StrongAttack.performed += OnStrongAttackInput;
        inputActions.Player.Guard.performed += OnGuardInput;
        inputActions.Player.Guard.canceled += OnGuardInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Guard.canceled -= OnGuardInput;
        inputActions.Player.Guard.performed -= OnGuardInput;
        inputActions.Player.StrongAttack.performed -= OnStrongAttackInput;
        inputActions.Player.Attack.performed -= OnAttackInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void OnGuardInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("���� ����");
            StartCoroutine(ParryAbleCo());
        }
        if(context.canceled)
        {
            Debug.Log("���� ��__");
            isGuardAble = false;
        }
    }
    IEnumerator ParryAbleCo()
    {
        isParryAble = true;
        float temp = parryTime_cur; // ��Ÿ�� ���� �и��� ���� ���� ��
        while (temp < parryTime_origin)
        {
            temp += Time.deltaTime; //�и� �ð��� ������ �и� false�� ����
            //parryTime_cur = temp;
        }
        isGuardAble = true;
        isParryAble = false;
        yield return null;
    }

    private void OnStrongAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("����  �� ����");
        }
        if (context.canceled)
        {
            Debug.Log("���� �ð� �ٵ�!!_");
        }
    }

    private void OnAttackInput(InputAction.CallbackContext context)
    {
        Debug.Log("attack");
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Debug.Log("move");
    }
}
