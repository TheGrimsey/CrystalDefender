using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * GAME KEEPER
 * Holds data relevant to multiple parts of the game.
 */
public class GameKeeper : MonoBehaviour
{
    //Our crystal.
    [SerializeField]
    GameObject _crystal;
    public GameObject Crystal => _crystal;

    //Current round.
    [SerializeField]
    int _round;
    public int Round => _round;

    public delegate void OnRoundChangedDelegate();
    public event OnRoundChangedDelegate OnRoundChanged;

    //Increments the round and invokes OnRoundChangedEvent
    public void IncrementRound() 
    { 
        _round++;

        if (OnRoundChanged != null)
            OnRoundChanged.Invoke();
    }

    //Restarts the game.
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Returns the active GameKeeper.
    public static GameKeeper Get()
    {
        return GameObject.FindGameObjectWithTag("GameKeeper").GetComponent<GameKeeper>();
    }
}
