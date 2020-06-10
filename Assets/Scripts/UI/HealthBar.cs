using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * HEALTH BAR
 * Updates slider widget with health of character.
 */
public class HealthBar : MonoBehaviour
{
    //StatsComponent to pull health value from.
    [SerializeField]
    StatsComponent _statsComponent;

    //Slider widget to modify
    [SerializeField]
    Slider _slider;

    // Start is called before the first frame update
    void Start()
    {
        _slider.minValue = 0;
        _slider.maxValue = _statsComponent.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Update value to current health.
        _slider.value = _statsComponent.Health;
    }
}
