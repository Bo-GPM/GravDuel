using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;
    private float horInput;
    private float verInput;
    public UnityEvent collidedWithBH;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");
        
        Vector2 tempTrans = new Vector2();
        tempTrans.x = horInput * moveSpeed * Time.deltaTime;
        tempTrans.y = verInput * moveSpeed * Time.deltaTime;
        
        transform.Translate(tempTrans);
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        collidedWithBH.Invoke();
        Debug.Log("GameOver");
        LevelManager.instance.PlayerFailed();
    }
}
