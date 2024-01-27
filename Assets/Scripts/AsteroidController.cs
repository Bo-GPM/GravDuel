using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public Rigidbody2D myselfRB;
    public float invincibleTime;

    public float attractionForce = 5.0f;
    [SerializeField] private int pointWorth = 1;


    // Update is called once per frame
    void Update()
    {
        myselfRB.AddForce(CalculateForce(LevelManager.instance.playerInstance), ForceMode2D.Impulse);

        // Invincible Time Update
        invincibleTime -= Time.deltaTime;
    }

    private Vector3 CalculateForce(GameObject attractor)
    {
        // Calculate scale and direction of black hole pulling force 
        Vector3 difference = attractor.transform.position - transform.position;
        float distance = difference.magnitude;
        float forceScale = attractionForce / (Mathf.Pow(distance, 1.3f));
        Vector3 nomForceVector3 = difference.normalized;
        Vector3 forceVector3 = forceScale * nomForceVector3;

        // Debug.Log(forceVector3);
        // Debug.Log(difference);
        // Debug.Log(distance);
        // Debug.Log(nomForceVector3);
        // Debug.Log(forceScale);

        return forceVector3; 
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if (other.gameObject.tag == "Enemy")
        {
            if (invincibleTime <= 0)
            {
                LevelManager.instance.audioFiles[1].Play();
                LevelManager.instance.SearchGameObjectToRemoveFromList(this.gameObject);
                LevelManager.instance.AddToScore(pointWorth);
                Destroy(gameObject);
            }
        }
    }
}
