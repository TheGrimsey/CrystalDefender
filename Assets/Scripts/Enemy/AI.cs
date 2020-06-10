using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/*
 * THREAT TARGET
 * Holds current threatlevel of an object.
 */
struct ThreatTarget
{
    public float Threat;
    public GameObject Target;

    public ThreatTarget(float InThreat, GameObject InGameObject)
    {
        Threat = InThreat;
        Target = InGameObject;
    }
}

/*
 * AI
 * Handles Enemy AI.
 */
[RequireComponent(typeof(CombatComponent))]
[RequireComponent(typeof(StatsComponent))]
[RequireComponent(typeof(NavMeshAgent))]
public class AI : MonoBehaviour
{
    //CACHED GameKeeper
    GameKeeper _gameKeeper;
    
    //Cached components.
    CombatComponent _combatComponent;
    StatsComponent _statsComponent;
    NavMeshAgent _navMeshAgent;

    //List of all targets and their threat level.
    List<ThreatTarget> _threat;

    //Current target.
    [SerializeField]
    GameObject _target;

    // Start is called before the first frame update
    void Start()
    {
        _gameKeeper = GameKeeper.Get();

        //Cache our combat component.
        _combatComponent = GetComponent<CombatComponent>();

        //Cache our statsComponent & register damaged event.
        _statsComponent = GetComponent<StatsComponent>();
        _statsComponent.OnDamaged += OnDamaged;
        _statsComponent.OnHealthChanged += OnHealthChanged;

        //Cache & set up NavMesh agent.
        _navMeshAgent = GetComponent<NavMeshAgent>();
        //Set this so we don't rotate.
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        //Set speed & stopping distance based on our stats.
        _navMeshAgent.speed = _statsComponent.MovementSpeed;
        _navMeshAgent.stoppingDistance = _statsComponent.AttackReach * 0.95f;

        //Threat set up.
        _threat = new List<ThreatTarget>();
        _target = null;

        //Add default threat to Crystal so enemies have somewhere they want to go from the start.
        _threat.Add(new ThreatTarget(10f, _gameKeeper.Crystal));
    }

    // Update is called once per frame
    void Update()
    {
        //Set our target to object with most threat.
        _target = GetTarget();

        //Do nothing if we have no target.
        if (_target == null)
            return;

        //Check if we are out of range of target. If we are set destination.
        if (Vector2.Distance(transform.position, _target.transform.position) >= _navMeshAgent.stoppingDistance)
        {
            //Go to Threat target.
            _navMeshAgent.SetDestination(_target.transform.position);
            _navMeshAgent.isStopped = false;

            //Face next walk point.
            _statsComponent.FaceDirection = (_navMeshAgent.steeringTarget - transform.position).normalized;
        }
        else
        {
            //Stop agent movement.
            _navMeshAgent.isStopped = true;

            //Face target.
            _statsComponent.FaceDirection = (_target.transform.position - transform.position).normalized;
        }

        //Make sure we stay at Z=0.
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        //If the agent isn't stopped then we are moving.
        _statsComponent.IsMoving = _navMeshAgent.isStopped;

        //This is true if we are in reach.
        if(_navMeshAgent.isStopped)
        {
            //Attack target.
            if(_combatComponent.AttackTarget(_target))
            {
                Debug.Log("Attacking " + _target.name);
            }
        }
    }
    
    //Returns our most threatening target. Requires threat to be sorted to be accurate.
    GameObject GetTarget()
    {
        return _threat.Count > 0 ? _threat[0].Target : null;
    }

    //Sorts targets based on threat.
    void SortThreat()
    {
        _threat.Sort((first, second) => second.Threat.CompareTo(first.Threat));
    }

    //Adds threat for object.
    public void AddThreat(float Amount, GameObject Target)
    {
        bool FoundTarget = false;

        //Try to find target in threat list.
        for(int i = 0; i < _threat.Count(); i++)
        {
            if (Target == _threat[i].Target)
            {
                //If it exists update it's threat.
                _threat[i] = new ThreatTarget(_threat[i].Threat + Amount, Target);
                FoundTarget = true;
                break;
            }
        }

        if (!FoundTarget)
        {
            _threat.Add(new ThreatTarget(Amount, Target));
        }

        //Sort threat so we attack the most threatening target next time.
        SortThreat();
    }

    void OnDamaged(int Damage, GameObject Damager)
    {
        if (Damager == null)
            return;

        //Add threat equal to the damage.
        AddThreat(Damage, Damager);
    }
    void OnHealthChanged(int newHealth)
    {
        //Destroy this character if our health is 0 (because we are dead)
        if (newHealth <= 0)
            Destroy(gameObject);
    }
}
