using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField] HitCollider[] hitCols;
    [SerializeField] CapsuleCollider[] hitColliders;

    GameManager gameManager;

    public Action playerHit;

    private void Awake()
    {
        hitCols = GetComponentsInChildren<HitCollider>();
        hitColliders = new CapsuleCollider[hitCols.Length];
        gameManager = GameManager.Instance;


    }
    private void Start()
    {
        for (int i = 0; i < hitCols.Length; i++)    //피격 콜라이더 배열
        {
            hitColliders[i] = hitCols[i].hitCollider;
            hitCols[i].OnHit += OnHit;
        }
    }
    private void OnDisable()
    {
        for (int i = hitCols.Length; i > 0; i--)    //피격 콜라이더 배열
        {
            hitCols[i].OnHit -= OnHit;
        }
    }

    void OnHit(string name, float damageMul)
    {
        gameManager.isPlayerHit = true;
        Debug.Log($"배율 {damageMul}, {name} 이 맞았다. ");
        playerHit?.Invoke();
    }

}
