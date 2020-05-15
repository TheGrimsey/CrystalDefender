using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsComponent : MonoBehaviour
{
    //Current Health
    [SerializeField]
    int _health;
    public int Health => _health;
    
    //Max Health.
    [SerializeField]
    int _maxHealth = 100;
    public int MaxHealth => _maxHealth;

    //Amount of Damage to deal when attacking.
    [SerializeField]
    int _attackPower = 8;
    public int AttackPower => _attackPower;

    //How far we can reach when attacking (16 = 1 tile)
    [SerializeField]
    int _attackReach = 16;
    public int AttackReach => _attackReach;

    //How fast we move (16 = 1 tile)
    [SerializeField]
    int _movementSpeed = 16;
    public int MovementSpeed => _movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        ResetHealth(); 
    }

    public void ResetHealth()
    {
        _health = _maxHealth;
    }

    public void Damage(int damage)
    {
        _health = Mathf.Clamp(Health - damage, 0, MaxHealth); 
    }
}
