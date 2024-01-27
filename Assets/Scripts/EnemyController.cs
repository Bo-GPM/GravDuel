using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public GameObject objectToSpawn;
    public float spawnInterval;
    public float spawnIntervalIncrement;
    public float minimumSpawnInterval;
    public float spawnDistance;
    public float tempInvincibleTime;
    private float tempInvincibleTimeOriginal;
    private float remainingSpawnTime;

    [SerializeField] private int pointWorth = 10;
    // private int spawnedEmemiesCount = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Initialization of remainingSpawnTime 
        remainingSpawnTime = spawnInterval;
        tempInvincibleTimeOriginal = tempInvincibleTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Spawning Logic
        if (remainingSpawnTime < 0)
        {
            // Vector2 tempPos = playerLocation.position - transform.position;
            // float tempAngle = Vector2.Angle(transform.position, playerLocation.position);
            // Vector3 tempFinalPos = Quaternion.AngleAxis(tempAngle, Vector3.forward) * tempPos;
            // tempFinalPos = tempFinalPos.normalized * spawnDistance;
            LevelManager.instance.audioFiles[3].Play();
            LevelManager.instance.resetableGameObjects.Add(Instantiate(objectToSpawn, transform.position, quaternion.identity));
            tempInvincibleTime = tempInvincibleTimeOriginal; // Reset timer

            // Instantiate(objectToSpawn, transform.position, quaternion.identity);
            CalculateInterval();
        }
        else
        {
            remainingSpawnTime -= Time.deltaTime;
            
            //Invincible Time 
            tempInvincibleTime -= Time.deltaTime;
        }
        
        // Looking at BH
        Vector3 direction = LevelManager.instance.playerInstance.transform.position - transform.position;
        // Debug.Log(direction);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion tempQ = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = tempQ;
        
    }

    private void CalculateInterval()
    {
        if (spawnInterval <= minimumSpawnInterval)
        {
            spawnInterval = minimumSpawnInterval;
        }
        else
        {
            spawnInterval -= spawnIntervalIncrement;
        }

        remainingSpawnTime = spawnInterval;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (tempInvincibleTime < 0)
        {
            if (other.gameObject.CompareTag("Asteroid"))
            {
                LevelManager.instance.audioFiles[1].Play();
                LevelManager.instance.SearchGameObjectToRemoveFromList(this.gameObject);
                LevelManager.instance.AddToScore(pointWorth);
                Destroy(gameObject);
                
            }
        }
    }
}
