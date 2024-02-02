using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    private void Awake()
    {
        if(Inst != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
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
