using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BasicRange : MonoBehaviour
{
    [SerializeField]
    private float meetRange;
    [SerializeField]
    private float EscapeRange;
    [SerializeField]
    private int attackCooldownFrame;
    [SerializeField]
    private LinearBullet linearBullet;
    [SerializeField]
    private float bulletVelocity;
    [SerializeField]
    private int damage;

    private SlowDebuffState currentState;
    private GameObject player;
    private GridPosition playerGridPosition;
    private PlayerHealth playerHealth;
    private GridPosition gridPosition;
    private AIPath aStar;
    private int attackCooldownCounter;
    private int attackTimeCounter;
    private bool canAttack;
    private EnemyFleeController fleeController;
    private Animator animator;

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
        fleeController = GetComponent<EnemyFleeController>();
        animator = GetComponentInChildren<Animator>();
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
            if (!IsPlayerInMeetRange()) nextState = SlowDebuffState.Idle;
            else if (IsPlayerInEscapeRange()) nextState = SlowDebuffState.Escape;
            else nextState = SlowDebuffState.Meet;
        }
        else if (currentState == SlowDebuffState.Escape)
        {
            if (!IsPlayerInMeetRange()) nextState = SlowDebuffState.Idle;
            else if (!IsPlayerInEscapeRange()) nextState = SlowDebuffState.Meet;
            else nextState = SlowDebuffState.Escape;
        }
        else
        {
            // Check if meet
            if (IsPlayerInMeetRange()) nextState = SlowDebuffState.Meet;
            else nextState = SlowDebuffState.Idle;
        }
        changeState(nextState);
    }
    private void AttackCooldownPerFrame()
    {
        if (!canAttack)
        {
            if(attackCooldownCounter == 0)
            {
                animator.Play("basic_range_attack");
            }
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
    private void changeState(SlowDebuffState nextState)
    {
        if (nextState == SlowDebuffState.Meet)
        {
            fleeController.IsEnable = false;
            if (canAttack)
            {
                LinearBullet spawned = Instantiate<LinearBullet>(linearBullet);
                spawned.Setup(transform.position, player.transform.position, bulletVelocity, damage);
                canAttack = false;
            }
        }
        else if (nextState == SlowDebuffState.Escape)
        {
            fleeController.IsEnable = true;
            animator.Play("basic_range_idle");
        }
        else
        {
            fleeController.IsEnable = false;
            animator.Play("basic_range_idle");
        }
        currentState = nextState;
    }
}
