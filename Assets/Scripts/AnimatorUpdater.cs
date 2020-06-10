using UnityEngine;

/*
 * ANIMATION UPDATER
 * Updates animation values based on StatsComponent
 */
[RequireComponent(typeof(StatsComponent))]
public class AnimatorUpdater : MonoBehaviour
{
    //CACHED StatsComponent that we pull values from.
    StatsComponent _statsComponent;

    //Animator to update values on.
    [SerializeField]
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        //Cache StatsComponent.
        _statsComponent = GetComponent<StatsComponent>();
    }

    // LateUpdate is called once per frame after all normal Update()
    void LateUpdate()
    {
        //Check so we have an animator.
        if(_animator != null)
        {
            //Set animator values.
            _animator.SetInteger("X", (int)_statsComponent.FaceDirection.x);
            _animator.SetInteger("Y", (int)_statsComponent.FaceDirection.y);
            _animator.SetBool("IsMoving", _statsComponent.IsMoving);
        }
    }
}
