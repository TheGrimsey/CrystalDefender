using UnityEngine;
using UnityEngine.SceneManagement;

public class GameKeeper : MonoBehaviour
{
    [SerializeField]
    GameObject _crystal;
    public GameObject Crystal => _crystal;

    public delegate void OnRoundChangedDelegate();
    public event OnRoundChangedDelegate OnRoundChanged;

    [SerializeField]
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

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static GameKeeper Get()
    {
        return GameObject.FindGameObjectWithTag("GameKeeper").GetComponent<GameKeeper>();
    }
}
