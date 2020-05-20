using System;
using UnityEngine;

[RequireComponent(typeof(StatsComponent))]
public class CombatComponent : MonoBehaviour
{
    StatsComponent _statsComponent;

    float _lastAttackTime = float.MinValue;

    // Start is called before the first frame update
    void Start()
    {
        _statsComponent = GetComponent<StatsComponent>();
    }

    public void AttackTarget(GameObject Target)
    {
        if (!CanAttack())
            return;

        StatsComponent TargetStats = Target.GetComponent<StatsComponent>();
        
        if (TargetStats == null)
            return;

        TargetStats.Damage(_statsComponent.AttackPower, this.gameObject);

        //Update attack time.
        _lastAttackTime = Time.time;
    }

    bool CanAttack()
    {
        return _lastAttackTime + _statsComponent.AttackCooldown <= Time.time;
    }
}
