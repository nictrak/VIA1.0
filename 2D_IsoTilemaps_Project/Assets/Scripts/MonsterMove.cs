﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Transform player;//set target from inspector instead of looking in Update
    public float speed = 3f;
   
    public Transform prefab;
    // Start is called before the first frame update
    void Start()
    {
        
        currentHealth = maxHealth;
        Debug.Log(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage");
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enermy died!");
        Destroy(gameObject);
        Instantiate(prefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //rotate to look at the player
         transform.LookAt(player.position);
         transform.Rotate(new Vector3(0,-90,0),Space.Self);//correcting the original rotation
         
         
         //move towards the player
         if (Vector3.Distance(transform.position,player.position)>1f){//move if distance from target is greater than 1
             transform.Translate(new Vector3(speed* Time.deltaTime,0,0) );
         }
    }
}
