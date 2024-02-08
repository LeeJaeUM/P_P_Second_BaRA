using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockonPos : MonoBehaviour
{
    public Transform playerTr;
    public Transform enemyTr;

    void Test()
    {
        Vector3 dir = playerTr.position - enemyTr.position;
    }
}
