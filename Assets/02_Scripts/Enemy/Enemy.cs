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
        Battle
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
    readonly int IsDieHash = Animator.StringToHash("isDie");



    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();    
    }
    private void Start()
    {
        if(target == null)
        {
            Debug.Log("타겟이 없음!");
        }
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

        if (distanceToTarget <= distance_MoveToBattle)  //공격가능 거리 안에 들어오면 Battle로 변경
        {
            EnterBattle(); // Battle 상태로 전환 시 추가적인 동작 수행
        }
    }

    void Battle()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        // 플레이어를 감지하기 위해 Ray를 사용
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Player"))
            {
                EnterBattle();
            }
        }
        else
        {
            EnterMove();
        }
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
        Debug.Log("Entering Move state.");
    }

    private void EnterBattle()
    {
        state = State.Battle;

        anim.SetBool(IsMoveHash, false);
        Debug.Log("Entering Battle state.");
    }
    #endregion
}
