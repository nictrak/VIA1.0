using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    private GameObject player;//set target from inspector instead of looking in Update
    public float speed = 3f;
    public float expectedDistance;
  
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {
        //rotate to look at the player
        transform.LookAt(player.transform.position);
        transform.Rotate(new Vector3(0,-90,0),Space.Self);//correcting the original rotation

        float distance = Vector3.Distance(transform.position, player.transform.position);
         //move towards the player
         if (distance > expectedDistance){//move if distance from target is greater than 1
             transform.Translate(new Vector3(speed* Time.deltaTime,0,0));
         }
    }
}
