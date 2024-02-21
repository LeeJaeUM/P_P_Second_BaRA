using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public CapsuleCollider hitCollider;
    [SerializeField]
    public int colliderCode = 0;
    [SerializeField]
    private float damageMultiplier = 1;

    public Action<string, float> OnHit;
    private void Start()
    {
        hitCollider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            if(!GameManager.Instance.isPlayerHit)
                OnHit?.Invoke(gameObject.name, damageMultiplier);        
        }
    }

}
