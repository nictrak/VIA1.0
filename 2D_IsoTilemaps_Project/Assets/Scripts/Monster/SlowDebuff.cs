using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SlowDebuff : MonoBehaviour
{
    [SerializeField]
    private float meetRange;
    [SerializeField]
    private float EscapeRange;
    [SerializeField]
    private int attackCooldownFrame;
    [SerializeField]
    private ProjectileMovement projectileBullet;

    private SlowDebuffState currentState;
    private GameObject player;
    private GridPosition playerGridPosition;
    private PlayerHealth playerHealth;
    private GridPosition gridPosition;
    private AIPath aStar;
    private int attackCooldownCounter;
    private int attackTimeCounter;
    private bool canAttack;
    private SpriteRenderer spriteRenderer;
    private EnemyFleeController fleeController;

    public enum SlowDebuffState
    {
        Idle,
        Meet,
        Escape
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = SlowDebuffState.Idle;
        player = GameObject.FindGameObjectWithTag("Player");
        playerGridPosition = player.GetComponent<GridPosition>();
        playerHealth = player.GetComponent<PlayerHealth>();
        gridPosition = GetComponent<GridPosition>();
        aStar = GetComponent<AIPath>();
        attackCooldownCounter = 0;
        attackTimeCounter = 0;
        canAttack = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        fleeController = GetComponent<EnemyFleeController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        StateMachineRunningPerFrame();
        AttackCooldownPerFrame();
    }
    public bool IsPlayerInMeetRange()
    {
        if (gridPosition.CalculateMagnitudeToOther(playerGridPosition) <= meetRange)
        {
            return true;
        }
        return false;
    }
    public bool IsPlayerInEscapeRange()
    {
        if (gridPosition.CalculateMagnitudeToOther(playerGridPosition) <= EscapeRange)
        {
            return true;
        }
        return false;
    }

    private void StateMachineRunningPerFrame()
    {
        SlowDebuffState nextState = SlowDebuffState.Idle;
        if (currentState == SlowDebuffState.Meet)
        {
            fleeController.IsEnable = false;
            spriteRenderer.color = Color.red;
            if (canAttack)
            {
                ProjectileMovement spawned = Instantiate<ProjectileMovement>(projectileBullet);
                spawned.Setup(transform.position, player.transform.position, 0.1f);
                canAttack = false;
            }
            if (!IsPlayerInMeetRange()) nextState = SlowDebuffState.Idle;
            else if (IsPlayerInEscapeRange()) nextState = SlowDebuffState.Escape;
            else nextState = SlowDebuffState.Meet;
        }
        else if(currentState == SlowDebuffState.Escape)
        {
            fleeController.IsEnable = true;
            spriteRenderer.color = Color.cyan;
            if (!IsPlayerInMeetRange()) nextState = SlowDebuffState.Idle;
            else if (!IsPlayerInEscapeRange()) nextState = SlowDebuffState.Meet;
            else nextState = SlowDebuffState.Escape;
        }
        else
        {
            fleeController.IsEnable = false;
            spriteRenderer.color = Color.white;
            // Check if meet
            if (IsPlayerInMeetRange()) nextState = SlowDebuffState.Meet;
            else nextState = SlowDebuffState.Idle;
        }
        currentState = nextState;
    }
    private void AttackCooldownPerFrame()
    {
        if (!canAttack)
        {
            if (attackCooldownCounter >= attackCooldownFrame)
            {
                canAttack = true;
                attackCooldownCounter = 0;
            }
            else
            {
                attackCooldownCounter++;
            }
        }
    }
}
