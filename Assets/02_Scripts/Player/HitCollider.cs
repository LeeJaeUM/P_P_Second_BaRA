using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public CapsuleCollider hitCollider;
    //[SerializeField]
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
        hitCollider = GetComponent<CapsuleCollider>();
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
        if (other.CompareTag("EnemyAttack"))
        {   
            //여기서 적 데미지를 가져오는 게 필요함
            if(!GameManager.Instance.isPlayerHit)
                OnHit?.Invoke(gameObject.name, damageMultiplier, hitDamage);        
        }
    }

}
