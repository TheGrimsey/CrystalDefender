using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

[RequireComponent(typeof(StatsComponent))]
public class TrapPlacer : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _traps;

    [SerializeField]
    int _selectedTrap = 0;

    [SerializeField]
    float _lastPlaceTime;

    StatsComponent _statsComponent;

    // Start is called before the first frame update
    void Start()
    {
        _lastPlaceTime = float.MinValue;

        _statsComponent = GetComponent<StatsComponent>();
    }
    
    public void PlaceTrap()
    {
        if (_traps.Count <= 0 || !CanPlaceTrap())
            return;

        Vector2 SpawnPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + _statsComponent.FaceDirection;

        GameObject Trap = Instantiate<GameObject>(_traps[_selectedTrap], SpawnPosition, Quaternion.identity);

        _lastPlaceTime = Time.time;
    }
    bool CanPlaceTrap()
    {
        return _lastPlaceTime + _statsComponent.TrapCooldown <= Time.time;
    }
}
