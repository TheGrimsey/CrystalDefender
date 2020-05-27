using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(CombatComponent))]
public class PlayerInput : MonoBehaviour
{
    MovementComponent _movementComponent;
    CombatComponent _combatComponent;

    // Start is called before the first frame update
    void Start()
    {
        _movementComponent = GetComponent<MovementComponent>();
        _combatComponent = GetComponent<CombatComponent>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        _movementComponent.MovementInput = context.ReadValue<Vector2>();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        _combatComponent.AttackInfrontOfCharacter();
    }
}
