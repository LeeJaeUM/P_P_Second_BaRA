using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField] HitCollider[] hitColliders;

    private void Start()
    {
        hitColliders = GetComponentsInChildren<HitCollider>();
    }
}
