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
