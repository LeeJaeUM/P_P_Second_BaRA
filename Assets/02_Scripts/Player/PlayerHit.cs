using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField] HitCollider[] hitCols;
    [SerializeField] CapsuleCollider[] hitColliders;

    GameManager gameManager;

    private void Awake()
    {
        hitCols = GetComponentsInChildren<HitCollider>();
        hitColliders = new CapsuleCollider[hitCols.Length];
        gameManager = GameManager.Instance;


    }
    private void Start()
    {
        for (int i = 0; i < hitCols.Length; i++)    //�ǰ� �ݶ��̴� �迭
        {
            hitColliders[i] = hitCols[i].hitCollider;
            hitCols[i].OnHit += OnHit;
        }
    }
    private void OnDisable()
    {
        for (int i = hitCols.Length; i > 0; i--)    //�ǰ� �ݶ��̴� �迭
        {
            hitCols[i].OnHit -= OnHit;
        }
    }

    void OnHit(string name, float damageMul)
    {
        gameManager.isPlayerHit = true;
        Debug.Log($"{name} �� �¾Ҵ�.");
    }

}
