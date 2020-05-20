using System.Collections;
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

        _statsComponent = GetComponent<StatsComponent>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _navMeshAgent.speed = _statsComponent.MovementSpeed;
        _navMeshAgent.stoppingDistance = _statsComponent.AttackReach * 0.9f;

        Threat = new List<ThreatTarget>();
        Threat.Add(new ThreatTarget(100f, _gameKeeper.Crystal));
    }

    // Update is called once per frame
    void Update()
    {
        GameObject Target = GetTarget();

        if (Target == null)
            return;

        //Go to Threat target.
        _navMeshAgent.SetDestination(Target.transform.position);
        
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
                Threat[i] = new ThreatTarget(Threat[i].Threat, Target);
                FoundTarget = true;
                break;
            }
        }

        if (!FoundTarget)
        {
            Threat.Add(new ThreatTarget(Amount, Target));
        }

        SortThreat();
    }
}
