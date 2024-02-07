using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputActions inputActions;
    Rigidbody rigid;

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

    Vector3 moveDirection;
    public float moveSpeed = 5;

    public float moveX = 0;     //�̵� ����Ǵ��� Ȯ�ο� ���� SetInput���� �����
    public float moveZ = 0;

    public Transform mainCameraTr;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody>();
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
    void Move()
    {
        // ī�޶��� ���� ������ �������� �̵� ������ ����
        Vector3 cameraForward = mainCameraTr.forward;
        cameraForward.y = 0; // ���� �������θ� �̵��ؾ� �ϹǷ� y���� 0���� ����
        cameraForward.x = 0; // ���� �������θ� �̵��ؾ� �ϹǷ� x���� 0���� ����
        Quaternion rotation = Quaternion.LookRotation(cameraForward);
        Vector3 _moveDirRTCamera = rotation * moveDirection;
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveSpeed * _moveDirRTCamera);

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
        moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
        //Debug.Log($"moveDirection.x = {moveDirection.x}, moveDirection.y = {moveDirection.y}, moveDirection.z = {moveDirection.z}");
       // animator.SetBool(IsMoveHash, isMove);

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

    private void OnGuardInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("���� ����");
            isGuardAble = true;
            isParryAble = true;
        }
        
        if(context.canceled)
        {
            Debug.Log("���� ��__");
            isGuardAble = false;
            isParryAble = false;
        }
    }
    void ParryTimer()   //�и� ������ �ð� ��� �Լ�
    {
        if (isParryAble)    //�и��� ������ �� parryTime_cur�� Ÿ�̸� ó�� ���
            parryTime_cur += Time.deltaTime;
        
        if(parryTime_cur > parryTime_origin)    //�и� ���� �ð��� �ѱ�� �и� �Ұ���
            isParryAble = false;
    }

    void ParryTimerReset()  //�и� ���� �Ǵ� �ǰ� �� �и� ���� �ð� �ʱ�ȭ
    {
        parryTime_cur = 0;
        isParryAble = false;
        Debug.Log("ParryTimerReset");
    }

    void GuardComplete()    //���� ���� �� �и� ���� �ð� ���� ����
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
