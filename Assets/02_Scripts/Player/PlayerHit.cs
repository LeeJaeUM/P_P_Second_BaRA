using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField]
    private float hitAbleTime = 0.25f;

    [SerializeField] HitCollider[] hitCols;
    //[SerializeField] CapsuleCollider[] hitColliders;

    GameManager gameManager;

    Vector3 pushPlayerVec = Vector3.zero;
    public Action<float, Transform> OnPlayerHit;    //Player���� �и��� ���
    public  Rigidbody rigid;

    private void Awake()
    {
        hitCols = GameManager.Instance.Player.GetComponentsInChildren<HitCollider>();
        gameManager = GameManager.Instance;

        rigid = GameManager.Instance.Player.GetComponentInParent<Rigidbody>();
    }
    private void Start()
    {
        for (int i = 0; i < hitCols.Length; i++)    //�ǰ� �ݶ��̴� �迭
        {
            hitCols[i].OnHit += OnHit;
        }
        pushPlayerVec = -transform.forward;
    }

    void OnHit(int attackType, float damageMul, float damage, Transform particleTr)
    {
        //�ߺ����ظ� �����ϱ� ���� ���ӸŴ��� �� ���� true�� ����
        gameManager.isPlayerHit = true;
        StartCoroutine(ResetHitAbleTimeCo());

        //���������� (�ܼ�)
        float finalDamage = damage * damageMul; 

        //Debug.Log($"���� {damageMul}, {name} �� �¾Ҵ�. �������� {finalDamage}");
        Debug.Log($"�ǰ� ���� {damageMul},  �������� {finalDamage}");
        OnPlayerHit?.Invoke(finalDamage, particleTr);

        //pushPlayerVec =  transform.position - particleTr.position;
        StartCoroutine(PushPlayer(attackType));
    }
    //�ǰ� Ȱ��ȭ �ڷ�ƾ
    IEnumerator ResetHitAbleTimeCo()
    {
        yield return new WaitForSeconds(hitAbleTime);
        gameManager.isPlayerHit = false;
    }

    IEnumerator PushPlayer(int attackType)
    {
        pushPlayerVec.y = 0;
        switch (attackType)
        {
            case 1:  rigid.AddForce(pushPlayerVec * 2f, ForceMode.Impulse);
                break;
            case 2:  rigid.AddForce(pushPlayerVec * 8f, ForceMode.Impulse);
                break;
        }
        yield return null;
    }
}
