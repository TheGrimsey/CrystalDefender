using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    GameKeeper _gameKeeper;

    [Header("Spawn Properties")]
    [SerializeField]
    List<GameObject> _enemiesToSpawn;

    [SerializeField]
    int _minMobsToSpawn = 3;

    [SerializeField]
    int _maxMobsToSpawn = 7;

    [SerializeField]
    int _spawnRadius = 5;

    [Header("Round Properties")]
    [SerializeField]
    float _baseTimeBetweenRounds = 30f;

    [SerializeField]
    float _preparationTime = 5f;

    [SerializeField]
    float _nextSpawnTime = 0f;

    void Start()
    {
        _gameKeeper = GameKeeper.Get();

        _nextSpawnTime = Time.time + _preparationTime;    
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= _nextSpawnTime)
        {
            _gameKeeper.IncrementRound();

            Spawn();

            _nextSpawnTime += _baseTimeBetweenRounds;
        }
    }

    private void Spawn()
    {
        Random.InitState(_gameKeeper.Round);

        int mobsToSpawn = Random.Range(_minMobsToSpawn, _maxMobsToSpawn);
        for(int i = 0; i < mobsToSpawn; i++)
        {
            Random.InitState(_gameKeeper.Round * (i + 1));

            GameObject enemyToSpawn = _enemiesToSpawn[Random.Range(0, _enemiesToSpawn.Count - 1)];

            Vector3 SpawnPosition = new Vector3(transform.position.x, transform.position.y + Random.Range(-_spawnRadius, _spawnRadius));

            Instantiate<GameObject>(enemyToSpawn, SpawnPosition, Quaternion.identity);
        }
    }
}
