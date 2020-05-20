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

[RequireComponent(typeof(StatsComponent))]
[RequireComponent(typeof(NavMeshAgent))]
public class AI : MonoBehaviour
{
    GameKeeper _gameKeeper;

    StatsComponent _statsComponent;
    NavMeshAgent _navMeshAgent;

    List<ThreatTarget> Threat;

    // Start is called before the first frame update
    void Start()
    {
        _gameKeeper = GameKeeper.Get();

        //Cache our statsComponent & register damaged event.
        _statsComponent = GetComponent<StatsComponent>();
        _statsComponent.OnDamaged += OnDamaged;

        //Set up NavMesh agent.
        _navMeshAgent = GetComponent<NavMeshAgent>();
        //Set this so we don't rotate.
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _navMeshAgent.speed = _statsComponent.MovementSpeed;
        _navMeshAgent.stoppingDistance = _statsComponent.AttackReach * 0.9f;

        Threat = new List<ThreatTarget>();

        //Add default threat to Crystal so enemies have somewhere they want to go from the start.
        Threat.Add(new ThreatTarget(1f, _gameKeeper.Crystal));
    }

    // Update is called once per frame
    void Update()
    {
        GameObject Target = GetTarget();

        if (Target == null)
            return;

        //Go to Threat target.
        _navMeshAgent.SetDestination(Target.transform.position);

        Vector2 direction = (_navMeshAgent.pathEndPosition - transform.position);
        Vector2 faceDirection = new Vector2(direction.x, direction.y).normalized;
        _statsComponent.FaceDirection = faceDirection;
    }
    
    GameObject GetTarget()
    {
        return Threat.Count > 0 ? Threat[0].Target : null;
    }

    void SortThreat()
    {
        Threat.Sort((x, y) => y.Threat.CompareTo(x.Threat));
    }

    public void AddThreat(float Amount, GameObject Target)
    {
        bool FoundTarget = false;

        for(int i = 0; i < Threat.Count(); i++)
        {
            if (Target == Threat[i].Target)
            {
                Threat[i] = new ThreatTarget(Threat[i].Threat + Amount, Target);
                FoundTarget = true;
                break;
            }
        }

        if (!FoundTarget)
        {
            Threat.Add(new ThreatTarget(Amount, Target));
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
}
