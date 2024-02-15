using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    CapsuleCollider atkCollider;

    private void Awake()
    {
        atkCollider = GetComponent<CapsuleCollider>();
    }
}
