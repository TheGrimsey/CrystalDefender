using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

/*
 * TRAP PLACER
 * Handles placing traps.
 */
[RequireComponent(typeof(StatsComponent))]
public class TrapPlacer : MonoBehaviour
{
    //CACHED Components.
    StatsComponent _statsComponent;

    //Prefabs of traps we can place.
    [SerializeField]
    List<GameObject> _traps;

    //Currently selected trap. Represented as an index into the _traps list.
    [SerializeField]
    int _selectedTrap = 0;

    //Time we last placed a trap. Used for cooldown.
    [SerializeField]
    float _lastPlaceTime;

    // Start is called before the first frame update
    void Start()
    {
        //Cache component.
        _statsComponent = GetComponent<StatsComponent>();

        _lastPlaceTime = float.MinValue;
    }
    
    //Places a trap infront of the player if possible.
    public void PlaceTrap()
    {
        //Check so we have traps and can place a trap currently. 
        if (_traps.Count <= 0 || !CanPlaceTrap())
            return;

        //Calculate spawn position of trap, placing it infront of the player.
        Vector2 SpawnPosition = gameObject.transform.position;  
        SpawnPosition += _statsComponent.FaceDirection;

        //Spawn trap.
        GameObject Trap = Instantiate<GameObject>(_traps[_selectedTrap], SpawnPosition, Quaternion.identity);

        //Update last placed time.
        _lastPlaceTime = Time.time;
    }

    //Returns true if we can place a trap at the current time.
    bool CanPlaceTrap()
    {
        return _lastPlaceTime + _statsComponent.TrapCooldown <= Time.time;
    }
}
