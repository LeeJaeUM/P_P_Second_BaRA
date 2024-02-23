using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NanoGauge : MonoBehaviour
{
    PlayerState state;
    Slider nanoSlider;

    private void Awake()
    {
        state = GameManager.Instance.PlayerState.GetComponent<PlayerState>();
        state.OnNanoChange += NanoGaugeUpdate;
        nanoSlider = gameObject.GetComponent<Slider>();
    }



    public void NanoGaugeUpdate(float nano)
    {
        nanoSlider.value = nano;
    }


}
