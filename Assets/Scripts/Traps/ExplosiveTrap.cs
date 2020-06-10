using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveTrap : MonoBehaviour
{
    [SerializeField]
    float _explosionRadius = 3f;

    [SerializeField]
    int _explosionDamage = 50;

    [SerializeField]
    bool _distanceFalloff = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered.");

        StatsComponent otherStats = other.gameObject.GetComponent<StatsComponent>();
        
        if(otherStats != null)
        {
            if (otherStats.Team == ETeam.Enemy)
            {
                Debug.Log("Trigger exploding.");
                Explode();
            }
        }
    }

    private void Explode()
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
                statsComponent.Damage((int)(_explosionDamage * damageMultiplier), gameObject);
            }
        }

        Destroy(gameObject);
    }
}
