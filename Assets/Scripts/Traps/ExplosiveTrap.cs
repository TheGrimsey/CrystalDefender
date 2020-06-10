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
        //List of hit colliders.
        List<Collider2D> results = new List<Collider2D>();
        
        //Do overlap for finding targets.
        Physics2D.queriesHitTriggers = true;
        Physics2D.OverlapCircle(transform.position, _explosionRadius, new ContactFilter2D(), results);

        //Check all targets and deal damage.
        foreach(Collider2D collider in results)
        {
            //Make sure collider has statscomponent and is an enemy.
            StatsComponent statsComponent = collider.GetComponent<StatsComponent>();
            if(statsComponent != null && statsComponent.Team == ETeam.Enemy)
            {
                //Damage multiplier. Used for modifying damage.
                float damageMultiplier = 1f;

                //Handle distance falloff.
                if(_distanceFalloff)
                {
                    Vector2 cPos = collider.transform.position;
                    Vector2 tPos = transform.position;
                    
                    //Calculate how far along from the center of the trap to the radius.
                    float distanceNormalized =  (Mathf.Pow(cPos.x - tPos.x, 2) + Mathf.Pow(cPos.y - tPos.y, 2)) / (_explosionRadius * _explosionRadius);

                    //Scale down damage multiplier based on distance.
                    damageMultiplier -= distanceNormalized;
                }

                //Damage enemy. Put in null as gameobject to make sure we don't mess up the threat.
                statsComponent.Damage((int)(_explosionDamage * damageMultiplier), null);
            }
        }

        //We exploded so destroy us.
        Destroy(gameObject);
    }
}
