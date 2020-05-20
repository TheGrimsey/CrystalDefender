using UnityEngine;

[RequireComponent(typeof(StatsComponent))]
public class CombatComponent : MonoBehaviour
{
    StatsComponent _statsComponent;

    // Start is called before the first frame update
    void Start()
    {
        _statsComponent = GetComponent<StatsComponent>();
    }

    public void AttackTarget(GameObject Target)
    {
        StatsComponent TargetStats = Target.GetComponent<StatsComponent>();
        
        if (TargetStats == null)
            return;

        TargetStats.Damage(_statsComponent.AttackPower, this.gameObject);
    }
}
