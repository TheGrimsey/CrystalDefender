using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(CombatComponent))]
[RequireComponent(typeof(TrapPlacer))]
public class PlayerInput : MonoBehaviour
{
    MovementComponent _movementComponent;
    CombatComponent _combatComponent;
    TrapPlacer _trapPlacer;

    // Start is called before the first frame update
    void Start()
    {
        _movementComponent = GetComponent<MovementComponent>();
        _combatComponent = GetComponent<CombatComponent>();
        _trapPlacer = GetComponent<TrapPlacer>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        _movementComponent.MovementInput = context.ReadValue<Vector2>();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        _combatComponent.AttackInfrontOfCharacter();
    }

    public void PlaceTrap(InputAction.CallbackContext context)
    {
        _trapPlacer.PlaceTrap();
    }
}
