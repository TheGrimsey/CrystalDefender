﻿using System;
using UnityEngine;

/*
 * ETEAM
 * Enum used to identify which team a character is on.
 */
public enum ETeam
{
    Friendly,
    Enemy
}

/*
 * STATS COMPONENT
 * Holds all stats used by characters such as health, speed, attack, etc.
 */
public class StatsComponent : MonoBehaviour
{
    public delegate void OnDamagedDelegate(int damageTaken, GameObject Damager);
    public event OnDamagedDelegate OnDamaged;

    public delegate void OnHealtChangedDelegate(int newHealth);
    public event OnHealtChangedDelegate OnHealthChanged;

    [SerializeField]
    ETeam _team = ETeam.Friendly;
    public ETeam Team => _team;

    [SerializeField]
    bool _isMoving;
    public bool IsMoving
    {
        get { return _isMoving; }
        set { _isMoving = value; }
    }

    //Keep track of which direction we are facing.
    [SerializeField]
    Vector2 _faceDirection;
    public Vector2 FaceDirection
    {
        get { return _faceDirection; }
        set 
        {
            Vector2 newValue = value;

            if (Math.Abs(newValue.y) > Math.Abs(newValue.x))
            {
                newValue.x = 0;
                newValue.Normalize();
            } 
            else
            {
                newValue.y = 0;
                newValue.Normalize();
            }

            _faceDirection = newValue; 
        }
    }

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
    int _attackReach = 2;
    public int AttackReach => _attackReach;

    //Time between each attack.
    [SerializeField]
    int _attackCooldown = 1;
    public int AttackCooldown => _attackCooldown;

    //Time between each attack.
    [SerializeField]
    int _trapCooldown = 4;
    public int TrapCooldown => _trapCooldown;

    //How fast we move expresserd in tile/s)
    [SerializeField]
    int _movementSpeed = 2;
    public int MovementSpeed => _movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _health = _maxHealth;
    }

    //Handles what happens when we take damage.
    public void Damage(int damage, GameObject Damager)
    {
        //Clamp health between MaxHealth & 0.
        _health = Mathf.Clamp(Health - damage, 0, MaxHealth);
        
        //Invoke OnDamaged event.
        if(OnDamaged != null)
            OnDamaged.Invoke(damage, Damager);

        //Invoke OnHealthChanged event.
        if (OnHealthChanged != null)
            OnHealthChanged.Invoke(_health);
    }
}
