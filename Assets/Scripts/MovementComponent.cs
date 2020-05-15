using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatsComponent))]
public class MovementComponent : MonoBehaviour
{
    StatsComponent _statsComponent;

    // Start is called before the first frame update
    void Start()
    {
        _statsComponent = GetComponent<StatsComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
