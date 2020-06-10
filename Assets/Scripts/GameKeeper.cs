using UnityEngine;

public class GameKeeper : MonoBehaviour
{
    [SerializeField]
    GameObject _crystal;
    public GameObject Crystal => _crystal;

    int _round;
    public int Round => _round;
    public void IncrementRound() 
    { 
        _round++;

        if (OnRoundChanged != null)
            OnRoundChanged.Invoke();
    }
    public void ResetRound()
    {
        _round = 0;

        if (OnRoundChanged != null)
            OnRoundChanged.Invoke();
    }

    public delegate void OnRoundChangedDelegate();
    public event OnRoundChangedDelegate OnRoundChanged;

    public static GameKeeper Get()
    {
        return GameObject.FindGameObjectWithTag("GameKeeper").GetComponent<GameKeeper>();
    }
}
