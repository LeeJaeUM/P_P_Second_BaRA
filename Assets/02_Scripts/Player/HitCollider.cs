using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public CapsuleCollider hitCollider;

    public Action OnHit;

    private void Start()
    {
        hitCollider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            OnHit?.Invoke();
            Debug.Log($"{gameObject.name} 이 맞았다.");
        }
    }
}
