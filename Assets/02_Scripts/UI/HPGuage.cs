using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGuage : MonoBehaviour
{
    GameManager gameManager;
    Slider hpSlider;
    float sliderValue = 0;
    private void Awake()
    {
        gameManager = GameManager.Instance;
        hpSlider = GetComponent<Slider>();  
    }
    private void Update()
    {
        sliderValue = Mathf.InverseLerp(0, gameManager.Player.maxHP, gameManager.Player.HP);
        hpSlider.value = sliderValue;
    }

}
