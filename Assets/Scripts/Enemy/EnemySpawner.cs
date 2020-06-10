using System.Collections.Generic;
using UnityEngine;

/*
 * ENEMY SPAWNER
 * Handles spawning randomized enemy waves at fixed intervals.
 */
public class EnemySpawner : MonoBehaviour
{
    //CACHED GameKeeper
    GameKeeper _gameKeeper;

    [Header("Spawn Properties")]
    //List of Enemy prefabs to spawn.
    [SerializeField]
    List<GameObject> _enemiesToSpawn;

    //Minimum amount of enemies to spawn each time.
    [SerializeField]
    int _minMobsToSpawn = 3;

    //Maximum amount of enemies to spawn each time.
    [SerializeField]
    int _maxMobsToSpawn = 7;

    //The distance above or below the spawner on the Y axis that we can spawn enemies.
    [SerializeField]
    int _spawnVerticalHalfLength = 6;

    [Header("Round Properties")]
    //Time between each spawn.
    [SerializeField]
    float _baseTimeBetweenRounds = 30f;

    //Time given at the start of the game for a player to prepare.
    [SerializeField]
    float _preparationTime = 5f;

    //Time when we will spawn the next round of enemies.
    [SerializeField]
    float _nextSpawnTime = 0f;

    void Start()
    {
        _gameKeeper = GameKeeper.Get();

        //Set the first spawntime to now + preperation time to give the player a warm up period.
        _nextSpawnTime = Time.time + _preparationTime;    
    }

    // Update is called once per frame
    void Update()
    {
        //Check if it is time to spawn.
        if(Time.time >= _nextSpawnTime)
        {
            //Update round.
            _gameKeeper.IncrementRound();

            //Spawn enemies.
            Spawn();

            //Set time for next spawn.
            _nextSpawnTime += _baseTimeBetweenRounds;
        }
    }

    private void Spawn()
    {
        //Seed random to current round for consistent spawns.
        Random.InitState(_gameKeeper.Round);

        //Randomize amount of mobs to spawn. (Using the seed we will get the same amount in the same order at all times)
        int mobsToSpawn = Random.Range(_minMobsToSpawn, _maxMobsToSpawn);
        for(int i = 0; i < mobsToSpawn; i++)
        {
            //Seed random with round * i + 1. This makes it so we get the same enemy prefab each replay but can have different ones within the same round.
            Random.InitState(_gameKeeper.Round * (i + 1));

            //Select prefab to spawn.
            GameObject enemyToSpawn = _enemiesToSpawn[Random.Range(0, _enemiesToSpawn.Count - 1)];

            //Randomize spawn position's Y.
            Vector3 SpawnPosition = new Vector3(transform.position.x, transform.position.y + Random.Range(-_spawnVerticalHalfLength, _spawnVerticalHalfLength));

            //Spawn enemy.
            Instantiate<GameObject>(enemyToSpawn, SpawnPosition, Quaternion.identity);
        }
    }
}
