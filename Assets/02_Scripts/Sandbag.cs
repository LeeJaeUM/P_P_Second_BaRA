using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandbag : MonoBehaviour
{
    Animator anim;
    WaitForSeconds delay20 = new WaitForSeconds(2.0f);

    private void Awake()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(SandbagAttack());
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
}
