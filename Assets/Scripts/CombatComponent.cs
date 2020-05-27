using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatsComponent))]
public class CombatComponent : MonoBehaviour
{
    StatsComponent _statsComponent;

    [SerializeField]
    float _lastAttackTime = float.MinValue;

    // Start is called before the first frame update
    void Start()
    {
        _statsComponent = GetComponent<StatsComponent>();
    }

    public bool AttackInfrontOfCharacter()
    {
        if (!CanAttack())
            return false;

        List<RaycastHit2D> results = new List<RaycastHit2D>();
        Physics2D.queriesHitTriggers = true;
        Physics2D.Raycast(transform.position, _statsComponent.FaceDirection, new ContactFilter2D(), results, _statsComponent.AttackReach);
        //TODO Raycast in FaceDirection. Then AttackTarget();

        Debug.Log(results.Count);
        foreach (RaycastHit2D hit in results)
        {
            Debug.Log(hit.collider.name);

            //Attack the first object that isn't us.
            if(hit.collider.gameObject != gameObject)
            {
                return AttackTarget(hit.collider.gameObject);
            }
        }

        return false;
    }

    public bool AttackTarget(GameObject Target)
    {
        if (!CanAttack())
            return false;

        StatsComponent TargetStats = Target.GetComponent<StatsComponent>();
        
        if (TargetStats == null || TargetStats.Team == _statsComponent.Team)
            return false;

        TargetStats.Damage(_statsComponent.AttackPower, gameObject);

        //Update attack time.
        _lastAttackTime = Time.time;

        return true;
    }

    bool CanAttack()
    {
        return _lastAttackTime + _statsComponent.AttackCooldown <= Time.time;
    }
}
