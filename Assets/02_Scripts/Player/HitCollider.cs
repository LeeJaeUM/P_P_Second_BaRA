using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public CapsuleCollider hitCollider;
    //[SerializeField]
    //데미지 배율을 정할 타입
    public enum HitType
    {
        Body,
        Head
    }
    public HitType hitType;
    //[SerializeField]
    public float damageMultiplier = 1;

    [SerializeField]
    private float hitDamage = 10;

    public Action<string, float, float> OnHit; //playerHit에서 사용
    private void Start()
    {
        //이 스크립트를 가진 오브제의 콜라이더 가져오기
        hitCollider = GetComponent<CapsuleCollider>();
        //타입에 따라 데미지 배율 변경
        if(hitType == HitType.Body)
        {
            damageMultiplier = 1;
        }
        else if(hitType == HitType.Head)
        {
            damageMultiplier = 1.5f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemyBase = other.gameObject.GetComponentInParent<EnemyBase>();
        if(enemyBase != null)
        {
            Debug.Log("적의 공격이라고 판단됨!");
            hitDamage = enemyBase.ATK;
            if (!GameManager.Instance.isPlayerHit)
                OnHit?.Invoke(gameObject.name, damageMultiplier, hitDamage);
        }

        ///if (other.CompareTag("EnemyAttack"))
        ///{
        ///    //여기서 적 데미지를 가져오는 게 필요함
        ///    if (!GameManager.Instance.isPlayerHit)
        ///        OnHit?.Invoke(gameObject.name, damageMultiplier, hitDamage);
        ///}
    }

}
