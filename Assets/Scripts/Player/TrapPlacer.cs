using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

[RequireComponent(typeof(StatsComponent))]
public class TrapPlacer : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _traps;

    StatsComponent _statsComponent;

    // Start is called before the first frame update
    void Start()
    {
        _statsComponent = GetComponent<StatsComponent>();
    }
    
    public void PlaceTrap()
    {
        if (_traps.Count <= 0)
            return;

        Vector2 SpawnPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + _statsComponent.FaceDirection;

        GameObject Trap = Instantiate<GameObject>(_traps[0], SpawnPosition, Quaternion.identity);
    }
}
