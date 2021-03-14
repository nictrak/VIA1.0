using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmArrow : MonoBehaviour
{
    public RythmSystem.ArrowDirection direction;
    private Vector3 velocityVector;
    private float xTerminateValue; 

    public Vector3 VelocityVector { get => velocityVector; set => velocityVector = value; }
    public float XTerminateValue { get => xTerminateValue; set => xTerminateValue = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        MovePerFrame();
        TerminateSelf();
    }
    private void MovePerFrame()
    {
        transform.position += velocityVector;
    }
    private void TerminateSelf()
    {
        if (transform.position.x > xTerminateValue) Destroy(this.gameObject);
    }
}
