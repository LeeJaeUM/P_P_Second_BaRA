using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private void Awake()
    {
        Player player = GetComponent<Player>();
        player.onParry += Tets;
    }

    private void Tets()
    {
        Debug.Log("테스트 액션");
    }
}
