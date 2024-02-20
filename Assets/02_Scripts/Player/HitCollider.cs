using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public CapsuleCollider hitCollider;

    private void Start()
    {
        hitCollider = GetComponent<CapsuleCollider>();
    }
}
