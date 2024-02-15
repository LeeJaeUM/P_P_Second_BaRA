using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //public static GameManager Inst;

    //private void Awake()
    //{
    //    if(Inst != null)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    else
    //    {
    //        Inst = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //}
    Player player;
    public Player Player
    {
        get
        {
            if (player == null)
                player = FindAnyObjectByType<Player>();
            return player;
        }
    }
    PlayerState playerState;
    public PlayerState PlayerState
    {
        get
        {
            if(playerState == null)
            {
                playerState = FindAnyObjectByType<PlayerState>();
            }
            return playerState;
        }
    }

    private int score;

    // ������ ���� ������ �������ų� �����ϴ� ������Ƽ
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            // ������ ����� ���� �߰� ������ ���⿡ �߰��� �� �ֽ��ϴ�.
        }
    }

}
