using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    StatsComponent _statsComponent;

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
        _slider.value = _statsComponent.Health;
    }
}
