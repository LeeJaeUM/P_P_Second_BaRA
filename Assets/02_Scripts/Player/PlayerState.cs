using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] float curNanoGauge = 0;
    [SerializeField] float maxNanoGuage = 10;
    [SerializeField] private int gold = 100;

    public Action<float> OnNanoChange;
    public int Gold
    {
        get { return gold; } 
        private set 
        { 
            if(gold != value) 
            { 
                gold = value;
            } 
        }
    }


    private void Awake()
    {
        Player player = GetComponent<Player>();
        player.onParry += Tets;
    }

    private void Tets()
    {
        Debug.Log("패링 성공 테스트 액션");
        NanoChange(1);
    }

    private void NanoChange(float nano)
    {
        curNanoGauge += nano;
        float sliderValue = Mathf.InverseLerp(0, maxNanoGuage, curNanoGauge);
        OnNanoChange?.Invoke(sliderValue);
    }

    public void AddGold(int addGold)
    {
        gold += addGold;
    }
}
