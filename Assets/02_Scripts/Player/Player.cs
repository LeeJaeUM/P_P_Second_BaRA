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
    public float parryTime_origin = 2.0f;
    public float parryTime_cur = 0.0f;


    private void Awake()
    {
        inputActions = new PlayerInputActions();
        HP = maxHP;
    }

    private void Update()
    {
        ParryTimer();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Attack.performed += OnAttackInput;
        inputActions.Player.StrongAttack.performed += OnStrongAttackInput;
        inputActions.Player.Guard.started += OnGuardInput;
        inputActions.Player.Guard.canceled += OnGuardInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Guard.canceled -= OnGuardInput;
        inputActions.Player.Guard.started -= OnGuardInput;
        inputActions.Player.StrongAttack.performed -= OnStrongAttackInput;
        inputActions.Player.Attack.performed -= OnAttackInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void OnStrongAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("강공  꾹 누름");
        }
        if (context.canceled)
        {
            Debug.Log("강공 시간 다됨!!_");
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

    private void OnGuardInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("가드 누름");
            isGuardAble = true;
            isParryAble = true;
        }
        
        if(context.canceled)
        {
            Debug.Log("가드 뗌__");
            isGuardAble = false;
            isParryAble = false;
        }
    }
    void ParryTimer()   //패리 가능한 시간 계산 함수
    {
        if (isParryAble)    //패리가 가능할 때 parryTime_cur은 타이머 처럼 상승
            parryTime_cur += Time.deltaTime;
        
        if(parryTime_cur > parryTime_origin)    //패리 가능 시간을 넘기면 패리 불가능
            isParryAble = false;
    }

    void ParryTimerReset()  //패리 성공 또는 피격 후 패리 가능 시간 초기화
    {
        parryTime_cur = 0;
        isParryAble = false;
        Debug.Log("ParryTimerReset");
    }

    void GuardComplete()    //가드 성공 시 패리 가능 시간 소폭 연장
    {
        parryTime_cur -= 0.1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            ParryTimerReset();
        }
    }
}
