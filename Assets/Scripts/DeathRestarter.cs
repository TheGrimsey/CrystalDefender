using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Restarts the game when this character reaches 0 health.
 */
[RequireComponent(typeof(StatsComponent))]
public class DeathRestarter : MonoBehaviour
{
    GameKeeper _gameKeeper;

    void Start()
    {
        _gameKeeper = GameKeeper.Get();

        GetComponent<StatsComponent>().OnHealthChanged += OnHealthChanged;
    }
    void OnHealthChanged(int newHealth)
    {
        if (newHealth <= 0)
            _gameKeeper.Restart();
    }
}
