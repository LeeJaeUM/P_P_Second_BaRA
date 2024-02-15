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

    // 게임의 현재 점수를 가져오거나 설정하는 프로퍼티
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            // 점수가 변경될 때의 추가 로직을 여기에 추가할 수 있습니다.
        }
    }

}
