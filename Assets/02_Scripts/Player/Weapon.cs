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

    //플레이어의 공격력으로 세팅하기 위한 함수
    public void SetDamage(float ATK)
    {
        DefaultDamage = ATK * weaponDamageMultiplier;
    }
}
