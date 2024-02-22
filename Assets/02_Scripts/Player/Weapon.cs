using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : AttackAble
{
    [SerializeField] private float weaponDamageMultiplier = 1.0f;
    protected override void Awake()
    {
        base.Awake();        
        SetDamage(GameManager.Instance.Player.ATK);
    }

    //�÷��̾��� ���ݷ����� �����ϱ� ���� �Լ�
    public void SetDamage(float ATK)
    {
        DefaultDamage = ATK * weaponDamageMultiplier;
    }
}
