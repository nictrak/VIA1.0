using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Vector2 velocityVector;
    private Rigidbody2D rigidbody;

    public Vector2 VelocityVector { get => velocityVector; set => velocityVector = value; }

    // Start is called before the first frame update
    void Start()
    {
        //Setup rigidbody
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        MovePerFrame();
    }
    // Move this object per frame
    private void MovePerFrame()
    {
        rigidbody.MovePosition(rigidbody.position + VelocityVector);
    }
}
