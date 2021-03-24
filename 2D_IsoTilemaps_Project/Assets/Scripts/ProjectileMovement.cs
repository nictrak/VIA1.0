using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float terminateThreshold;

    private Vector2 endPosition;
    private float currentLavitateVelocity;
    private Vector2 planeVector;
    private float lavitateValue;
    private Rigidbody2D rbody;
    private bool isAlreadySetup;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        TerminateAtEndPerFrame();
        MovePerFrame();
        UpdateVelocityPerFrame();
    }
    private void Setup(Vector2 startPosition, Vector2 endPosition, float startLavitateVelocity, float planeVelocity)
    {
        this.endPosition = endPosition;
        this.currentLavitateVelocity = startLavitateVelocity;
        planeVector = CalculatePlaneVector(startPosition, endPosition, planeVelocity);
        lavitateValue = 0;
        isAlreadySetup = true;
    }
    private Vector2 CalculatePlaneVector(Vector2 startPosition, Vector2 endPosition, float planeVelocity)
    {
        Vector2 planeVector = (endPosition - startPosition).normalized * planeVelocity;
        return planeVector;
    }
    private void MovePerFrame()
    {

        Vector2 lavitateVector = new Vector2(0, currentLavitateVelocity);
        Vector2 newPos = rbody.position + lavitateVector + planeVector;
        rbody.MovePosition(newPos);
    }
    private void UpdateVelocityPerFrame()
    {
        currentLavitateVelocity -= gravity;
    }
    private void TerminateAtEndPerFrame()
    {
        float distance = (rbody.position - endPosition).magnitude;
        if(distance < terminateThreshold)
        {
            Destroy(gameObject);
        }
    }
}
