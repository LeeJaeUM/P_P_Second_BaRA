using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{


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

    Vector3 moveDirection = Vector3.zero;
    public float moveSpeed = 5;
    public float rotationSpeed = 2;

    public float moveX = 0;     //이동 적용되는지 확인용 변수 SetInput에서 사용중
    public float moveZ = 0;

    public Transform mainCameraTr;

    private int IsMoveHash = Animator.StringToHash("isMoving");
    private int InputZHash = Animator.StringToHash("InputZ");
    private int InputXHash = Animator.StringToHash("InputX");
    private int IsAttackHash = Animator.StringToHash("isAttack");
    private int AttackComboHash = Animator.StringToHash("AttackCombo");
    private int AttackSpeedHash = Animator.StringToHash("AttackSpeed");

    private int curCombo = 0;
    private bool isAttack = false;
    
    public bool isLockon = false;

    public Action onParry;

    bool isHitAble = true;

    PlayerInputActions inputActions;
    Rigidbody rigid;
    Animator anim;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        HP = maxHP;
    }

    private void Update()
    {
        ParryTimer();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Attack.performed += OnAttackInput;
        inputActions.Player.StrongAttack.performed += OnStrongAttackInput;
        inputActions.Player.Guard.started += OnGuardInput;
        inputActions.Player.Guard.canceled += OnGuardInput;
        inputActions.Player.Lockon.performed += OnLockonInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Guard.canceled -= OnGuardInput;
        inputActions.Player.Guard.started -= OnGuardInput;
        inputActions.Player.StrongAttack.performed -= OnStrongAttackInput;
        inputActions.Player.Attack.performed -= OnAttackInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        SetInput(context.ReadValue<Vector2>(), !context.canceled);
    }
    private void OnLockonInput(InputAction.CallbackContext context)
    {
        isLockon = !isLockon;
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
        AttackStart();
    }
    private void OnGuardInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //Debug.Log("가드 누름");
            isGuardAble = true;
            isParryAble = true;
        }
        
        if(context.canceled)
        {
            //Debug.Log("가드 뗌__");
            isGuardAble = false;
            isParryAble = false;
        }
    }
    /// <summary>
    /// 피격 시 관리
    /// </summary>
    /// <param name="other"></param>

    public void PlayerHited()
    {
        if (isParryAble)
        {
            onParry?.Invoke();
            ParryTimerReset();
            Debug.Log("패리성공");
        }
        else if (isGuardAble)
        {
            GuardComplete();
            Debug.Log("가드로 막음");
        }
        else
        {
            ParryTimerReset();
            Debug.Log("그냥 맞아버림");
        }
    }

    #region 이동관련 함수
    void Move()
    {
        // 카메라의 전방 방향을 기준으로 이동 방향을 조정
        Vector3 cameraForward = mainCameraTr.forward;
        Vector3 cameraRight = mainCameraTr.right;
        cameraForward.y = 0; // 수평 방향으로만 이동해야 하므로 y값은 0으로 설정
        cameraRight.y = 0; // 수평 방향으로만 이동해야 하므로 y값은 0으로 설정

        Vector3 movement = (cameraForward * moveZ +  cameraRight * moveX).normalized;
        moveDirection = movement;
        if (!isAttack)
        {
            rigid.velocity = movement * moveSpeed;
        }

        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }
    }

    /// <summary>
    /// 이동 입력 처리용 함수 
    /// </summary>
    /// <param name="input">입력된 방향</param>
    /// <param name="isMove">이동 중이면 true, 이동 중이 아니면 false</param>
    void SetInput(Vector2 input, bool isMove)
    {
        moveX = input.x;
        moveZ = input.y;
        //moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
        //Debug.Log($"moveDirection.x = {moveDirection.x}, moveDirection.y = {moveDirection.y}, moveDirection.z = {moveDirection.z}");
        
        anim.SetBool(IsMoveHash, isMove);
        anim.SetFloat(InputZHash, moveZ);
        anim.SetFloat(InputXHash, moveX);
    }
    #endregion


    #region 가드패리 함수 OnGuardInput, ontriggerEnter에서 사용
    void ParryTimer()   //패리 가능한 시간 계산 함수
    {
        if (isParryAble)    //패리가 가능할 때 parryTime_cur은 타이머 처럼 상승
            parryTime_cur += Time.deltaTime;

        if (parryTime_cur > parryTime_origin)    //패리 가능 시간을 넘기면 패리 불가능
            isParryAble = false;
    }

    void ParryTimerReset()  //패리 성공 또는 피격 후 패리 가능 시간 초기화
    {
        parryTime_cur = 0;
        isParryAble = false;
    }

    void GuardComplete()    //가드 성공 시 패리 가능 시간 소폭 연장
    {
        parryTime_cur -= 0.1f;
    }
    #endregion

    #region 공격관련 함수
            
    void AttackStart()
    {
        if (isAttack)   //이미 공격중이라면 = 콤보공격 시전
        {
            Debug.Log("TTTTTETE 1");
            if (anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f
                && anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
            {
                Debug.Log("TTTTTETE 2");
                isAttack = true;
                curCombo++;
                anim.SetInteger(AttackComboHash, curCombo);
            }
        }
        else            //처음 공격 하는 거라면
        {
            Debug.Log("TTTTTETE 3");
            isAttack = true;
            curCombo++;
            anim.SetInteger(AttackComboHash, curCombo);
            anim.SetBool(IsAttackHash, isAttack);
        }
        Debug.Log("TTTTTETE 4");
    }

    /// <summary>
    /// 애니메이션 종료 시점에실행할 애니메이션 이벤트 함수
    /// </summary>
    void AttackEnd()
    {
        isAttack = false;
        curCombo = 0;
        anim.SetInteger(AttackComboHash, curCombo);
        anim.SetBool(IsAttackHash, isAttack);
    }

    public void AttackMove()
    {
        rigid.AddForce(moveDirection * 3, ForceMode.Impulse);
    }

    public void ComboReset()
    {
        curCombo = 0;
        anim.SetInteger(AttackComboHash, curCombo);
    }

    #endregion

}
