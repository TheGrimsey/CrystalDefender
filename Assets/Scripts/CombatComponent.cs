using System.Collections.Generic;
using UnityEngine;

/*
 * COMBAT COMPONENT
 * Handles damaging other characters.
 */
[RequireComponent(typeof(StatsComponent))]
public class CombatComponent : MonoBehaviour
{
    //CACHED Component
    StatsComponent _statsComponent;

    //Time we lasted attacked. Used for cooldown.
    [SerializeField]
    float _lastAttackTime;

    // Start is called before the first frame update
    void Start()
    {
        //Cache component
        _statsComponent = GetComponent<StatsComponent>();

        _lastAttackTime = float.MinValue;
    }

    //Damages the first enemy infront of our character and within our attack reach. Returns true if we hit something.
    public bool AttackInfrontOfCharacter()
    {
        //Check so we can attack.
        if (!CanAttack())
            return false;

        //Save hit objects.
        List<RaycastHit2D> results = new List<RaycastHit2D>();

        //Raycast infront of our character.
        Physics2D.queriesHitTriggers = true;
        Physics2D.Raycast(transform.position, _statsComponent.FaceDirection, new ContactFilter2D(), results, _statsComponent.AttackReach);
        
        //Go through all hit colliders.
        foreach (RaycastHit2D hit in results)
        {
            //Attack the first object that isn't us.
            if(hit.collider.gameObject != gameObject)
            {
                return AttackTarget(hit.collider.gameObject);
            }
        }

        return false;
    }

    //Damages the gameobject. Does not account for attackreach. Returns true if we hit something.
    public bool AttackTarget(GameObject Target)
    {
        //Check so we can attack.
        if (!CanAttack())
            return false;

        //Get target stats.
        StatsComponent TargetStats = Target.GetComponent<StatsComponent>();
        
        //Check so target has a statscomponent and is an enemy.
        if (TargetStats == null || TargetStats.Team == _statsComponent.Team)
            return false;
        
        //Deal damage to target.
        TargetStats.Damage(_statsComponent.AttackPower, gameObject);

        //Update attack time.
        _lastAttackTime = Time.time;

        return true;
    }

    //Returns true if we can attack at the current time.
    bool CanAttack()
    {
        return _lastAttackTime + _statsComponent.AttackCooldown <= Time.time;
    }
}
