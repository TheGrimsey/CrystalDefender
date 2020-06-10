using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * DEATH RESTARTER
 * Restarts the game when this character dies.
 */
[RequireComponent(typeof(StatsComponent))]
public class DeathRestarter : MonoBehaviour
{
    void Start()
    {
        //We don't need to cache the components since we only ever use them once.
        
        //Register to health change event.
        GetComponent<StatsComponent>().OnHealthChanged += OnHealthChanged;
    }

    //Called when Health is Changed.
    void OnHealthChanged(int newHealth)
    {
        //If we are dead then restart the game.
        if (newHealth <= 0)
            GameKeeper.Get().Restart();
    }
}
