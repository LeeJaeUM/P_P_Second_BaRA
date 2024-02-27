using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float curHp = 0;
    [SerializeField] protected float maxHp = 100;
    [SerializeField] public float ATK = 10;
    [SerializeField] protected int gold = 10;
    public ParticleSystem particle_Hit;
    PlayerState playerState;
    Player player;

    Collider hitCollider;

    public Action onEnemyDie;

    public float HP
    {
        get { return curHp; }
        protected set
        {
            if(curHp != value)
            {
                curHp = value;
            }
        }
    }

    protected virtual void Awake()
    {
        HP = maxHp;
        hitCollider = GetComponent<Collider>();
        particle_Hit = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        if (playerState == null)
        {
            playerState = GameManager.Instance.PlayerState;

        }
        if(playerState != null) //enemy ��� �� ��带 ��� ��Ű������ �׼� ���
        {
            onEnemyDie += AddPlayerGold;
        }
        if(player == null)
        {
            player = GameManager.Instance.Player;
        }
    }


    void EnemyHit(float damage)
    {
        HP -= damage;
        if (HP < 0.2f)
        {
            EnemyDie();
        }

        if(particle_Hit != null)
        {
            particle_Hit.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.CompareTag("PlayerAttack"))
        //    EnemyHit(10);
        AttackAble attackAble = other.gameObject.GetComponent<AttackAble>();
        if(attackAble != null)
        {
            EnemyHit(attackAble.DefaultDamage);
        }
    }

    void EnemyDie()
    {
        hitCollider.enabled = false;
        onEnemyDie?.Invoke();
        Destroy(gameObject, 2f);
    }

    void AddPlayerGold()
    {
        playerState.AddGold(gold);
    }
}
