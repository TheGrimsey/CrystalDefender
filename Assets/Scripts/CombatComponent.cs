using System;
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
        //TODO Raycast in FaceDirection. Then AttackTarget();

        return false;
    }

    public bool AttackTarget(GameObject Target)
    {
        if (!CanAttack())
            return false;

        StatsComponent TargetStats = Target.GetComponent<StatsComponent>();
        
        if (TargetStats == null)
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
