using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * EXPLOSIVE TRAP
 * A trap that explodes dealing damage in a radius when stepped on.
 */
public class ExplosiveTrap : MonoBehaviour
{
    //Radius to deal damage within.
    [SerializeField]
    float _explosionRadius = 3f;

    //Damage to deal to hit enemies.
    [SerializeField]
    int _explosionDamage = 50;

    //Wether or not to do distance falloff reducing damage dealt based on how far the enemy is away from the explosive.
    [SerializeField]
    bool _distanceFalloff = false;

    //Called when anyone enters the trigger.
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Get other's stats component.
        StatsComponent otherStats = other.gameObject.GetComponent<StatsComponent>();
        
        //Check so it has a stats component.
        if(otherStats != null)
        {
            //Check if it is an enemy.
            if (otherStats.Team == ETeam.Enemy)
            {
                //Blow up.
                Explode();
            }
        }
    }

    // Blows up the trap dealing damage to everyone within radius and destroying the trap.
    void Explode()
    {
        List<Collider2D> results = new List<Collider2D>();
        Physics2D.queriesHitTriggers = true;
        Physics2D.OverlapCircle(transform.position, _explosionRadius, new ContactFilter2D(), results);

        foreach(Collider2D collider in results)
        {
            StatsComponent statsComponent = collider.GetComponent<StatsComponent>();
            if(statsComponent != null && statsComponent.Team == ETeam.Enemy)
            {
                float damageMultiplier = 1f;

                if(_distanceFalloff)
                {
                    Vector2 cPos = collider.transform.position;
                    Vector2 tPos = transform.position;
                    float distanceNormalized =  (Mathf.Pow(cPos.x - tPos.x, 2) + Mathf.Pow(cPos.y - tPos.y, 2)) / (_explosionRadius * _explosionRadius);

                    damageMultiplier -= distanceNormalized;
                }

                Debug.Log("Dealing " + damageMultiplier * _explosionDamage + " damage to " + collider.gameObject.name);
                statsComponent.Damage((int)(_explosionDamage * damageMultiplier), null);
            }
        }

        Destroy(gameObject);
    }
}
