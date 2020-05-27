using UnityEngine;

[RequireComponent(typeof(StatsComponent))]
public class AnimatorUpdater : MonoBehaviour
{
    StatsComponent _statsComponent;

    [SerializeField]
    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _statsComponent = GetComponent<StatsComponent>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(_animator != null)
        {
            _animator.SetInteger("X", (int)_statsComponent.FaceDirection.x);
            _animator.SetInteger("Y", (int)_statsComponent.FaceDirection.y);
            _animator.SetBool("IsMoving", _statsComponent.IsMoving);
        }
    }
}
