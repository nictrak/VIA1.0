using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MonsterKnocked : MonoBehaviour
{
    private float knockVelocity;
    private bool isKnock;
    private Vector2 knockVector;
    private int knockFrame;
    private int knockCounter;
    private Rigidbody2D rbody;
    private EnemyAIController movementController;
    //private AIPath aStar;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        movementController = GetComponent<EnemyAIController>();
        //aStar = GetComponent<AIPath>();;
        knockCounter = 0;
        isKnock = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        KnockedPerFrame();
    }
    public void Knocked(Vector2 enemyPosition, float knockVelocity, int knockFrame)
    {
        if (!isKnock)
        {
            Debug.Log("Player knocked");
            isKnock = true;
            movementController.enabled = false;
            //aStar.enabled = false;
            this.knockVelocity = knockVelocity;
            knockVector = CalKnockVector(enemyPosition);
            this.knockFrame = knockFrame;
            knockCounter = 0;
        }
    }
    public Vector2 CalKnockVector(Vector2 enemyPosition)
    {
        Vector2 result = new Vector2();
        Vector2 playerPosition = rbody.position;
        Vector2 direction = (playerPosition - enemyPosition).normalized;
        result = direction * knockVelocity;
        Debug.Log(result);
        return result;
    }
    private void KnockedPerFrame()
    {
        if (isKnock)
        {
            if(knockCounter >= knockFrame)
            {
                isKnock = false;
                movementController.enabled = true;
                //aStar.enabled = true;
                knockCounter = 0;
            }
            else
            {
                Vector2 newPos = rbody.position + knockVector;
                rbody.MovePosition(newPos);
                knockCounter++;
            }
        }
    }
}
