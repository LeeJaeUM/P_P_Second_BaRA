using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float curHp = 0;
    [SerializeField] protected float maxHp = 100;
    [SerializeField] protected int gold = 10;
    public float ATK = 10;
    public bool isAttacking = false;

    public GameObject hitParicle;

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
        ParticleRandomRotate(hitParicle);
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

    protected virtual void EnemyDie()
    {
        hitCollider.enabled = false;
        onEnemyDie?.Invoke();
        Destroy(gameObject, 2f);
    }

    void AddPlayerGold()
    {
        playerState.AddGold(gold);
    }

    void ParticleRandomRotate(GameObject particleObj)
    {
        if (particleObj != null)
        {
            Debug.Log("Test_Enemy_Particle!!!");
            // ������ ȸ�� ���� �����մϴ�.
            Vector3 randomRotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

            // ������ ���� ȸ�� ���� �����մϴ�.
            particleObj.transform.rotation = Quaternion.Euler(randomRotation);
            ParticleSystem ps = particleObj.GetComponent<ParticleSystem>();
            ps.Play();
        }
    }
}
