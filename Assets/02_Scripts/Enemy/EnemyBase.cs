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
    PlayerState playerState;

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
    }

    private void OnEnable()
    {
        if (playerState == null)
        {
            playerState = GameManager.Instance.PlayerState;

        }
        if(playerState != null) //enemy 사망 시 골드를 상승 시키기위한 액션 사용
        {
            onEnemyDie += AddPlayerGold;
        }
    }


    void EnemyHit(float damage)
    {
        HP -= damage;

        if (HP < 0.2f)
        {
            EnemyDie();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            EnemyHit(10);
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
