using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(StatsComponent))]
[RequireComponent(typeof(CircleCollider2D))]
public class MovementComponent : MonoBehaviour
{
    [SerializeField]
    private LayerMask movementBlockingLayer;

    StatsComponent _statsComponent;
    CircleCollider2D _circleCollider2D;

    [SerializeField]
    Vector2 MovementInput;

    // Start is called before the first frame update
    void Start()
    {
        _statsComponent = GetComponent<StatsComponent>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // FixedUpdate is called on a fixed timer.
    void FixedUpdate()
    {        
        
        //Make sure the player wants to move before we do any expensive calculations.
        if (MovementInput != Vector2.zero)
        {
            //Amount to move this frame.
            float MovementAmount = _statsComponent.MovementSpeed * Time.fixedDeltaTime;

            Vector3 movementDistance = MovementInput * MovementAmount;

            //Try to move normally. If that fails try each axis individually (we do this so that the player still can move even if it is a tiny bit into the wall of one direction)
            if (!TryToMove(movementDistance))
            {
                //If that failed let's try to move only in the horizontal vector.
                movementDistance = new Vector3(MovementInput.x * MovementAmount, 0, 0);

                if (!TryToMove(movementDistance))
                {
                    //If that failed then just vertically.
                    movementDistance = new Vector3(0, MovementInput.y * MovementAmount, 0);

                    TryToMove(movementDistance);
                }
            }
        }
    }
    
    //Attempts to move the character by translation checking collision for that position on the movementBlockingLayer.
    bool TryToMove(Vector3 translation)
    {
        //Calculate how far we should move.

        //Raycast for movement.
        Collider2D hitCollider = Physics2D.OverlapCircle(gameObject.transform.position + translation, _circleCollider2D.radius, movementBlockingLayer.value);

        //Check so we didnt hit anything
        if (hitCollider == null)
        {
            //If we didnt then we can move.
            transform.Translate(translation, Space.World);

            return true;
        }

        return false;
    }

    //Called when player presses any of the move keys.
    public void Move(InputAction.CallbackContext context)
    {
        //Save input value to MovementInput.
        MovementInput = context.ReadValue<Vector2>();
    }
}
