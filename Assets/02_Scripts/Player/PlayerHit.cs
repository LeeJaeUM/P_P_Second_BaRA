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
    public Action<float, Transform> OnPlayerHit;    //Player에서 패리에 사용
    public  Rigidbody rigid;

    private void Awake()
    {
        hitCols = GameManager.Instance.Player.GetComponentsInChildren<HitCollider>();
        gameManager = GameManager.Instance;

        rigid = GameManager.Instance.Player.GetComponentInParent<Rigidbody>();
    }
    private void Start()
    {
        for (int i = 0; i < hitCols.Length; i++)    //피격 콜라이더 배열
        {
            hitCols[i].OnHit += OnHit;
        }
        pushPlayerVec = -transform.forward;
    }

    void OnHit(int attackType, float damageMul, float damage, Transform particleTr)
    {
        //중복피해를 방지하기 위한 게임매니저 불 변수 true로 변경
        gameManager.isPlayerHit = true;
        StartCoroutine(ResetHitAbleTimeCo());

        //데미지계산식 (단순)
        float finalDamage = damage * damageMul; 

        //Debug.Log($"배율 {damageMul}, {name} 이 맞았다. 데미지는 {finalDamage}");
        Debug.Log($"피격 배율 {damageMul},  데미지는 {finalDamage}");
        OnPlayerHit?.Invoke(finalDamage, particleTr);

        //pushPlayerVec =  transform.position - particleTr.position;
        StartCoroutine(PushPlayer(attackType));
    }
    //피격 활성화 코루틴
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
