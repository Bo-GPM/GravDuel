using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    // Stuff to spawn
    public GameObject enemies;
    // Range of spawnArea
    public int spawnRangeX;
    public int spawnRangeY;
    // Time to spawn
    public float leastTimeToSpawn;
    public float mostTimeToSpawn;
    public float initTimeToSpawn;
    private float _remainingTimeToSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        _remainingTimeToSpawn = GenerateSpawnTime() + initTimeToSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        // A timer controlling when to spawn
        if (_remainingTimeToSpawn < 0)
        {
            SpawnEnemies();
            _remainingTimeToSpawn = GenerateSpawnTime();
        }
        else
        {
            _remainingTimeToSpawn -= Time.deltaTime;
        }
        
    }

    private float GenerateSpawnTime()
    {
        float tempTime = Random.Range(leastTimeToSpawn, mostTimeToSpawn);
        return tempTime;
    }

    private void SpawnEnemies()
    {
        float spawnPosX = Random.Range(-spawnRangeX, spawnRangeX) + transform.position.x;
        float spawnPosY = Random.Range(-spawnRangeY, spawnRangeY) + transform.position.y;

        Vector2 spawnPos = new Vector2(spawnPosX, spawnPosY);

        LevelManager.instance.audioFiles[4].Play();
        // Instantiate(enemies, spawnPos, quaternion.identity);
        LevelManager.instance.resetableGameObjects.Add(Instantiate(enemies, spawnPos, quaternion.identity)); 
    }
}
