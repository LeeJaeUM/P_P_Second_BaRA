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
    private float distance_IdleToMove = 10f; // Idle���� Move�� ��ȯ�ϴ� �Ÿ� �Ӱ谪
    private float distance_MoveToBattle = 4f; // Move���� Battle�� ��ȯ�ϴ� �Ÿ� �Ӱ谪

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
            Debug.Log("Ÿ���� ����!");
        }
    }

    private void Update()
    {
        if (target == null)  //�÷��̾ ������� Idle�� ����
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
        // �÷��̾ ���� ����� ��ġ�� ���� ��ġ ������ �Ÿ��� ���
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // �Ÿ��� ���� �� ���ϸ� Move ���·� ��ȯ
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

        if (distanceToTarget <= distance_MoveToBattle)  //���ݰ��� �Ÿ� �ȿ� ������ Battle�� ����
        {
            EnterBattle(); // Battle ���·� ��ȯ �� �߰����� ���� ����
        }
    }

    void Battle()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        // �÷��̾ �����ϱ� ���� Ray�� ���
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

    #region State ���� �Լ�
    private void EnterIdle()
    {
        state = State.Idle;

        anim.SetBool(IsMoveHash, false);
        anim.SetBool(IsBattleInHash, false);
        anim.SetInteger(AttackPatternHash, 0);  //�޺� 0���� �ʱ�ȭ
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
