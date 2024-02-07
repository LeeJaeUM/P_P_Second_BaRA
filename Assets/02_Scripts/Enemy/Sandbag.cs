using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandbag : MonoBehaviour
{
    Animator anim;
    WaitForSeconds delay20 = new WaitForSeconds(2.0f);
    [SerializeField] SphereCollider[] sphereColliders;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(SandbagAttack());
        sphereColliders = GetComponentsInChildren<SphereCollider>();
    }

    IEnumerator SandbagAttack()
    {
        while (true) 
        {
            anim.SetTrigger("Attack");
            yield return delay20;
            anim.SetTrigger("Attack_Db");
            yield return delay20;
        }
    }

    void SandbagAttack_Right_A()
    {
        sphereColliders[0].enabled = true;
    }
    void SandbagAttack_Left_A()
    {
        sphereColliders[1].enabled = true;
    }
    void SandbagAttack_Right_D()
    {
        sphereColliders[0].enabled = false;
    }
    void SandbagAttack_Left_D()
    {
        sphereColliders[1].enabled = false;
    }
}
