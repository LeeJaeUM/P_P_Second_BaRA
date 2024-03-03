using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : EnemyBase
{
    Animator anim;
    enum State
    {
        Idle,
        Move,
        Battle,
        Die
    }
    State state = State.Idle;

    NavMeshAgent agent;
    [Header("AI")]
    public Transform target;
    private float distance_IdleToMove = 10f; // Idle에서 Move로 전환하는 거리 임계값
    private float distance_MoveToBattle = 4f; // Move에서 Battle로 전환하는 거리 임계값

    readonly int IsMoveHash = Animator.StringToHash("isMove");
    readonly int IsBattleInHash = Animator.StringToHash("isBattleIn");
    readonly int AttackPatternHash = Animator.StringToHash("AttackPattern");
    readonly int DieHash = Animator.StringToHash("Die");
    [SerializeField]
    private bool isInteract = false;    //공격 패턴 시 연속 공격 제한
    [SerializeField]
    private bool isRotateAble = true;
    public float rotationSpeed = 2;

    public int testPattern = 0;

    [SerializeField]
    public CapsuleCollider leftHandCollider;
    public CapsuleCollider rightHandCollider;
    public Collider groundAttackCollider;

    // ray의 길이
    [SerializeField]
    private float _maxDistance = 3.0f;      //Ray의 최대 길이

    // ray의 색상
    [SerializeField]
    private Color _rayColor = Color.red;

    void OnDrawGizmos()
    {
        Gizmos.color = _rayColor;

        // 함수 파라미터 : 현재 위치, Box의 절반 사이즈, Ray의 방향, RaycastHit 결과, Box의 회전값, BoxCast를 진행할 거리
        if (true == Physics.BoxCast(transform.position, transform.lossyScale / 2.0f, transform.forward, out RaycastHit hit, transform.rotation, _maxDistance))
        {
            // Hit된 지점까지 ray를 그려준다.
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);

            // Hit된 지점에 박스를 그려준다.
            Gizmos.DrawWireCube(transform.position + transform.forward * hit.distance, transform.lossyScale);
        }
        else
        {
            // Hit가 되지 않았으면 최대 검출 거리로 ray를 그려준다.
            Gizmos.DrawRay(transform.position, transform.forward * _maxDistance);
        }
    }
    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //AttackCollisers = GetComponentsInChildren<CapsuleCollider>();
    }
    private void Start()
    {
        if(target == null)
        {
            Debug.Log("타겟이 없음!");
        }

        agent.SetDestination(target.position);
    }

    private void Update()
    {
        if (target == null)  //플레이어가 사라지면 Idle로 변경
        {
            EnterIdle();
        }

        switch (state)
        {
            case State.Idle:    Idle();
                break;
            case State.Move:    Move();
                break; 
            case State.Battle:  Battle();
                break;
            case State.Die:    // EnemyDie();
                break;
        }

    }

    void Idle()
    {
        // 플레이어나 추적 대상의 위치와 현재 위치 사이의 거리를 계산
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // 거리가 일정 값 이하면 Move 상태로 전환
        if (distanceToTarget <= distance_IdleToMove)
        {
            EnterMove();
            Debug.Log("Switching to Move state.");
        }
    }

    void Move()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        agent.SetDestination(target.position);
        agent.stoppingDistance = 2;                     //enemy가 멈추는 거리
        if (distanceToTarget <= distance_MoveToBattle)  //공격가능 거리 안에 들어오면 Battle로 변경
        {
            EnterBattle(); // Battle 상태로 전환 시 추가적인 동작 수행
        }
    }

    void Battle()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);       
        //Battle중에서의 상태 변경
        if (!isInteract && distanceToTarget > distance_MoveToBattle + 0.5f)  //플레이어가 멀어지면 Move로 변경, 공격(행동) 중이 아닐 때
        {
            EnterMove();
        }

        if (isRotateAble)     //공격 중이 아닐 때 플레이어 바라보기
            LookAtPlayer();
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, transform.lossyScale * 0.4f, transform.forward, out hit, transform.rotation, _maxDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (!isInteract)    //중복 공격 방지
                    StartCoroutine(EnemyAttack());
            }
        }
    }
    
    IEnumerator EnemyAttack()
    {
        isInteract = true;          //중복 공격 방지
        int pattern = Random.Range(1, 5);
        testPattern = pattern;
        anim.SetInteger(AttackPatternHash, pattern);
        yield return null;
    }

    //Battle에서 사용하는 플레이어를 바라보는 함수
    void LookAtPlayer()
    {
        Vector3 followPlayer = (target.position - transform.position).normalized;
        followPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(followPlayer);
        //transform.rotation = targetRotation;
        // 현재 회전에서 목표 회전까지 부드럽게 회전시키기
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
    #region AttackAnimation Clip 애니메이션 이벤트 함수
    void AttackEnd()
    {
        isInteract = false;
        isRotateAble = true;
    }

    void StopLookAtPlayer()
    {
        isRotateAble = false;
    }

    //공격 범위 온 오프
    void Attack_Right()
    {
        StartCoroutine(Attack_Co(rightHandCollider));
    }
    void Attack_Left()
    {
        StartCoroutine(Attack_Co(leftHandCollider));
    }
    void Attack_Gound()
    {
        StartCoroutine(Attack_Co(groundAttackCollider));
    }

    IEnumerator Attack_Co(Collider atkCollider)
    {
        atkCollider.enabled = true;
        isAttacking = true;
        yield return new WaitForSeconds(0.15f);
        atkCollider.enabled = false;
        isAttacking = false;
    }
    #endregion


    protected override void EnemyDie()
    {
        base.EnemyDie();
        isAttacking = false;
        isInteract = true;
        state = State.Die;
        anim.SetTrigger(DieHash);
    }

    #region State 변경 함수
    private void EnterIdle()
    {
        state = State.Idle;

        anim.SetBool(IsMoveHash, false);
        anim.SetBool(IsBattleInHash, false);
        anim.SetInteger(AttackPatternHash, 0);  //콤보 0으로 초기화
        Debug.Log("Entering Idle state.");
    }

    private void EnterMove()
    {
        state = State.Move;

        anim.SetBool(IsMoveHash, true);
        anim.SetBool(IsBattleInHash, false);
        Debug.Log("Entering Move state.");
    }

    private void EnterBattle()
    {
        state = State.Battle;

        anim.SetBool(IsMoveHash, false);
        anim.SetBool(IsBattleInHash, true);
        Debug.Log("Entering Battle state.");
    }
    #endregion

}
