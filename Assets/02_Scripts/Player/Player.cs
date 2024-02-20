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

    public float moveX = 0;     //�̵� ����Ǵ��� Ȯ�ο� ���� SetInput���� �����
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
            Debug.Log("����  �� ����");
        }
        if (context.canceled)
        {
            Debug.Log("���� �ð� �ٵ�!!_");
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
            //Debug.Log("���� ����");
            isGuardAble = true;
            isParryAble = true;
        }
        
        if(context.canceled)
        {
            //Debug.Log("���� ��__");
            isGuardAble = false;
            isParryAble = false;
        }
    }
    /// <summary>
    /// �ǰ� �� ����
    /// </summary>
    /// <param name="other"></param>

    public void PlayerHited()
    {
        if (isParryAble)
        {
            onParry?.Invoke();
            ParryTimerReset();
            Debug.Log("�и�����");
        }
        else if (isGuardAble)
        {
            GuardComplete();
            Debug.Log("����� ����");
        }
        else
        {
            ParryTimerReset();
            Debug.Log("�׳� �¾ƹ���");
        }
    }

    #region �̵����� �Լ�
    void Move()
    {
        // ī�޶��� ���� ������ �������� �̵� ������ ����
        Vector3 cameraForward = mainCameraTr.forward;
        Vector3 cameraRight = mainCameraTr.right;
        cameraForward.y = 0; // ���� �������θ� �̵��ؾ� �ϹǷ� y���� 0���� ����
        cameraRight.y = 0; // ���� �������θ� �̵��ؾ� �ϹǷ� y���� 0���� ����

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
    /// �̵� �Է� ó���� �Լ� 
    /// </summary>
    /// <param name="input">�Էµ� ����</param>
    /// <param name="isMove">�̵� ���̸� true, �̵� ���� �ƴϸ� false</param>
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


    #region �����и� �Լ� OnGuardInput, ontriggerEnter���� ���
    void ParryTimer()   //�и� ������ �ð� ��� �Լ�
    {
        if (isParryAble)    //�и��� ������ �� parryTime_cur�� Ÿ�̸� ó�� ���
            parryTime_cur += Time.deltaTime;

        if (parryTime_cur > parryTime_origin)    //�и� ���� �ð��� �ѱ�� �и� �Ұ���
            isParryAble = false;
    }

    void ParryTimerReset()  //�и� ���� �Ǵ� �ǰ� �� �и� ���� �ð� �ʱ�ȭ
    {
        parryTime_cur = 0;
        isParryAble = false;
    }

    void GuardComplete()    //���� ���� �� �и� ���� �ð� ���� ����
    {
        parryTime_cur -= 0.1f;
    }
    #endregion

    #region ���ݰ��� �Լ�
            
    void AttackStart()
    {
        if (isAttack)   //�̹� �������̶�� = �޺����� ����
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
        else            //ó�� ���� �ϴ� �Ŷ��
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
    /// �ִϸ��̼� ���� ������������ �ִϸ��̼� �̺�Ʈ �Լ�
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
