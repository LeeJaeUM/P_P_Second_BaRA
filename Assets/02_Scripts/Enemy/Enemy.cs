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
    private float distance_IdleToMove = 10f; // Idle���� Move�� ��ȯ�ϴ� �Ÿ� �Ӱ谪
    private float distance_MoveToBattle = 4f; // Move���� Battle�� ��ȯ�ϴ� �Ÿ� �Ӱ谪

    readonly int IsMoveHash = Animator.StringToHash("isMove");
    readonly int IsBattleInHash = Animator.StringToHash("isBattleIn");
    readonly int AttackPatternHash = Animator.StringToHash("AttackPattern");
    readonly int DieHash = Animator.StringToHash("Die");
    [SerializeField]
    private bool isInteract = false;    //���� ���� �� ���� ���� ����

    public int testPattern = 0;
    // ray�� ����
    [SerializeField]
    private float _maxDistance = 3.0f;      //Ray�� �ִ� ����

    // ray�� ����
    [SerializeField]
    private Color _rayColor = Color.red;

    void OnDrawGizmos()
    {
        Gizmos.color = _rayColor;

        // �Լ� �Ķ���� : ���� ��ġ, Box�� ���� ������, Ray�� ����, RaycastHit ���, Box�� ȸ����, BoxCast�� ������ �Ÿ�
        if (true == Physics.BoxCast(transform.position, transform.lossyScale / 2.0f, transform.forward, out RaycastHit hit, transform.rotation, _maxDistance))
        {
            // Hit�� �������� ray�� �׷��ش�.
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);

            // Hit�� ������ �ڽ��� �׷��ش�.
            Gizmos.DrawWireCube(transform.position + transform.forward * hit.distance, transform.lossyScale);
        }
        else
        {
            // Hit�� ���� �ʾ����� �ִ� ���� �Ÿ��� ray�� �׷��ش�.
            Gizmos.DrawRay(transform.position, transform.forward * _maxDistance);
        }
    }
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
            case State.Die:     EnemyDie();
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
        agent.stoppingDistance = 2;                     //enemy�� ���ߴ� �Ÿ�
        if (distanceToTarget <= distance_MoveToBattle)  //���ݰ��� �Ÿ� �ȿ� ������ Battle�� ����
        {
            EnterBattle(); // Battle ���·� ��ȯ �� �߰����� ���� ����
        }
    }

    void Battle()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);       
        //Battle�߿����� ���� ����
        if (distanceToTarget > distance_MoveToBattle && !isInteract)  //�÷��̾ �־����� Move�� ����, ����(�ൿ) ���� �ƴ� ��
        {
            EnterMove();
        }

        if (!isInteract)     //���� ���� �ƴ� �� �÷��̾� �ٶ󺸱�
            LookAtPlayer();
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, transform.lossyScale / 2.0f, transform.forward, out hit, transform.rotation, _maxDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (!isInteract)    //�ߺ� ���� ����
                    StartCoroutine(EnemyAttack());
            }
        }
    }
    
    IEnumerator EnemyAttack()
    {
        isInteract = true;          //�ߺ� ���� ����
        int pattern = Random.Range(1, 5);
        testPattern = pattern;
        anim.SetInteger(AttackPatternHash, pattern);
        yield return null;
    }

    //Battle���� ����ϴ� �÷��̾ �ٶ󺸴� �Լ�
    void LookAtPlayer()
    {
        Vector3 followPlayer = (target.position - transform.position).normalized;
        followPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(followPlayer);
        transform.rotation = targetRotation;
    }

    void AttackEnd()
    {
        isInteract = false;
    }

    protected override void EnemyDie()
    {
        base.EnemyDie();
        isAttacking = false;
        isInteract = true;
        state = State.Die;
        anim.SetTrigger(DieHash);
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
        anim.SetBool(IsBattleInHash, true);
        Debug.Log("Entering Battle state.");
    }
    #endregion

}