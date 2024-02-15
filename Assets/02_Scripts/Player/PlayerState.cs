using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] float curNanoGuage = 0;
    [SerializeField] float maxNanoGuage = 10;

    [SerializeField] private int gold = 100;
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
        Debug.Log("테스트 액션");
        curNanoGuage++;


    }

    public void AddGold(int addGold)
    {
        gold += addGold;
    }
}
