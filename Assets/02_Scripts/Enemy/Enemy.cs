using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : EnemyBase
{
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



    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
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
        if(target == null)  //�÷��̾ ������� Idle�� ����
        {
            EnterIdle();
        }
    }

    #region State ���� �Լ�
    private void EnterIdle()
    {
        state = State.Idle;
        Debug.Log("Entering Idle state.");
    }

    private void EnterMove()
    {
        state = State.Move;
        Debug.Log("Entering Move state.");
    }

    private void EnterBattle()
    {
        state = State.Battle;
        Debug.Log("Entering Battle state.");
    }
    #endregion
}
