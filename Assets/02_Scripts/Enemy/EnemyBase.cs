using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float curHp = 0;
    [SerializeField] protected float maxHp = 100;

    [SerializeField] protected float gold = 10;

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

        Destroy(gameObject, 2f);
    }
}
