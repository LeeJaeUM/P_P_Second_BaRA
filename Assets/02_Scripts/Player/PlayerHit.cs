using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField]
    private float hitAbleTime = 0.2f;

    [SerializeField] HitCollider[] hitCols;
    //[SerializeField] CapsuleCollider[] hitColliders;

    GameManager gameManager;

    public Action<float> OnPlayerHit;    //Player���� �и��� ���

    private void Awake()
    {
        hitCols = GameManager.Instance.Player.GetComponentsInChildren<HitCollider>();
       // hitColliders = new CapsuleCollider[hitCols.Length];
        gameManager = GameManager.Instance;


    }
    private void Start()
    {
        for (int i = 0; i < hitCols.Length; i++)    //�ǰ� �ݶ��̴� �迭
        {
            //hitColliders[i] = hitCols[i].hitCollider;
            hitCols[i].OnHit += OnHit;
        }
    }
    //private void OnDisable()
    //{
    //    for (int i = hitCols.Length; i > 0; i--)    //�ǰ� �ݶ��̴� �迭
    //    {
    //        hitCols[i].OnHit -= OnHit;
    //    }
    //}

    void OnHit(string name, float damageMul, float damage)
    {
        //�ߺ����ظ� �����ϱ� ���� ���ӸŴ��� �� ���� true�� ����
        gameManager.isPlayerHit = true;
        StartCoroutine(ResetHitAbleTimeCo());
        //���������� (�ܼ�)
        float finalDamage = damage * damageMul; 
        Debug.Log($"���� {damageMul}, {name} �� �¾Ҵ�. �������� {finalDamage}");
        OnPlayerHit?.Invoke(finalDamage);
    }

    //�ǰ� Ȱ��ȭ �ڷ�ƾ
    IEnumerator ResetHitAbleTimeCo()
    {
        yield return new WaitForSeconds(hitAbleTime);
        gameManager.isPlayerHit = false;
    }

}
