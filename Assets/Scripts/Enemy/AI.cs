using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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

[RequireComponent(typeof(CombatComponent))]
[RequireComponent(typeof(StatsComponent))]
[RequireComponent(typeof(NavMeshAgent))]
public class AI : MonoBehaviour
{
    GameKeeper _gameKeeper;

    CombatComponent _combatComponent;
    StatsComponent _statsComponent;
    NavMeshAgent _navMeshAgent;

    List<ThreatTarget> _threat;
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

        //Set up NavMesh agent.
        _navMeshAgent = GetComponent<NavMeshAgent>();
        //Set this so we don't rotate.
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        //Set speed & stopping distance based on our stats.
        _navMeshAgent.speed = _statsComponent.MovementSpeed;
        _navMeshAgent.stoppingDistance = _statsComponent.AttackReach * 0.95f;

        _threat = new List<ThreatTarget>();
        _target = null;

        //Add default threat to Crystal so enemies have somewhere they want to go from the start.
        _threat.Add(new ThreatTarget(10f, _gameKeeper.Crystal));
    }

    // Update is called once per frame
    void Update()
    {
        _target = GetTarget();

        if (_target == null)
            return;

        //Turn towards target.
        Vector2 faceDirection;

        //Check if we are out of range of target. If we are set destination.
        if (Vector2.Distance(transform.position, _target.transform.position) >= _navMeshAgent.stoppingDistance)
        {
            //Go to Threat target.
            _navMeshAgent.SetDestination(_target.transform.position);
            _navMeshAgent.isStopped = false;

            //Face next walk point.
            faceDirection = (_navMeshAgent.steeringTarget - transform.position);
        }
        else
        {
            _navMeshAgent.isStopped = true;

            //Face target.
            faceDirection = (_target.transform.position - transform.position);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        _statsComponent.FaceDirection = faceDirection.normalized;
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
    
    GameObject GetTarget()
    {
        return _threat.Count > 0 ? _threat[0].Target : null;
    }

    void SortThreat()
    {
        _threat.Sort((x, y) => y.Threat.CompareTo(x.Threat));
    }

    public void AddThreat(float Amount, GameObject Target)
    {
        bool FoundTarget = false;

        for(int i = 0; i < _threat.Count(); i++)
        {
            if (Target == _threat[i].Target)
            {
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

        AddThreat(Damage, Damager);
    }
    private void OnHealthChanged(int newHealth)
    {
        if (newHealth <= 0)
            Destroy(gameObject);
    }
}
