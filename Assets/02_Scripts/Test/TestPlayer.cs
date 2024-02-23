using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayer : TestBase
{
    public Player player;
    public NanoGauge nanoGauge;
    [Range(0f, 1f)] public float testNano = 0;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.parryTime_cur = 0; 
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.AttackMove();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        nanoGauge.NanoGaugeUpdate(testNano);
    }
}
