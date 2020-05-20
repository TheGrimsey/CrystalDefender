using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameKeeper : MonoBehaviour
{
    [SerializeField]
    GameObject _crystal;
    public GameObject Crystal => _crystal;

    // Start is called before the first frame update
    void Start()
    {
    }

    public static GameKeeper Get()
    {
        return GameObject.FindGameObjectWithTag("GameKeeper").GetComponent<GameKeeper>();
    }
}
