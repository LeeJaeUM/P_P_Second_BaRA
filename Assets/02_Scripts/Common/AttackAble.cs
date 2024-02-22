using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAble : MonoBehaviour
{
    [SerializeField] private Collider attackCollider;
    [SerializeField] protected float defaultDamage = 10;
    public float DefaultDamage
    {
        get { return defaultDamage; }
        protected set 
        {
            if(defaultDamage != value)
            {
                defaultDamage = value;
            }
        }
    }

    protected virtual void Awake()
    {
        attackCollider = GetComponent<Collider>();  
    }
}
