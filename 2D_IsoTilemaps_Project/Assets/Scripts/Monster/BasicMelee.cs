using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class BasicMelee : MonoBehaviour
{
    [SerializeField]
    private float meetRange;
    [SerializeField]
    private float aggroRange;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private int attackCooldownFrame;
    [SerializeField]
    private int attckTimeFrame;
    [SerializeField]
    private int attackDamage;

    private BasicMeleeState currentState;
    private GameObject player;
    private GridPosition playerGridPosition;
    private PlayerHealth playerHealth;
    private GridPosition gridPosition;
    private AIPath aStar;
    private int attackCooldownCounter;
    private int attackTimeCounter;
    private bool canAttack;
    private Animator animator;
    
    public enum BasicMeleeState{
        Idle,
        Meet,
        Aggro,
        Attack
    }
    // Start is called before the first frame update
    void Start()
    {
        currentState = BasicMeleeState.Idle;
        player = GameObject.FindGameObjectWithTag("Player");
        playerGridPosition = player.GetComponent<GridPosition>();
        playerHealth = player.GetComponent<PlayerHealth>();
        gridPosition = GetComponent<GridPosition>();
        aStar = GetComponent<AIPath>();
        animator = GetComponentInChildren<Animator>();
        attackCooldownCounter = 0;
        attackTimeCounter = 0;
        canAttack = true;
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
        if(gridPosition.CalculateMagnitudeToOther(playerGridPosition) <= meetRange)
        {
            return true;
        }
        return false;
    }
    public bool IsPlayerInAggroRange()
    {
        if (gridPosition.CalculateMagnitudeToOther(playerGridPosition) <= aggroRange)
        {
            return true;
        }
        return false;
    }
    public bool IsPlayerInAttackRange()
    {
        {
            if (gridPosition.CalculateMagnitudeToOther(playerGridPosition) <= attackRange)
            {
                return true;
            }
            return false;
        }
    }
    private void StateMachineRunningPerFrame()
    {
        BasicMeleeState nextState = BasicMeleeState.Idle;
        if (currentState == BasicMeleeState.Meet)
        {
            // Check if aggro
            if (!IsPlayerInMeetRange()) nextState = BasicMeleeState.Idle;
            else if (IsPlayerInAggroRange()) nextState = BasicMeleeState.Aggro;
            else nextState = BasicMeleeState.Meet;
        }
        else if (currentState == BasicMeleeState.Aggro)
        {
            if (!IsPlayerInMeetRange()) nextState = BasicMeleeState.Idle;
            else if (IsPlayerInAttackRange() && canAttack) nextState = BasicMeleeState.Attack;
            else nextState = BasicMeleeState.Aggro;
        }
        else if (currentState == BasicMeleeState.Attack)
        {
            if (attackTimeCounter >= attckTimeFrame)
            {
                canAttack = false;
                if (IsPlayerInAttackRange()) playerHealth.DealDamage(attackDamage);
                attackTimeCounter = 0;
                nextState = BasicMeleeState.Aggro;
            }
            else
            {
                attackTimeCounter++;
                nextState = BasicMeleeState.Attack;
            }
        }
        else
        {
            // Check if meet
            if (IsPlayerInMeetRange()) nextState = BasicMeleeState.Meet;
            else nextState = BasicMeleeState.Idle;
        }
        changeState(nextState);
    }
    private void AttackCooldownPerFrame()
    {
        if (!canAttack)
        {
            if(attackCooldownCounter >= attackCooldownFrame)
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
    private void changeState(BasicMeleeState nextState)
    {
        if (nextState == BasicMeleeState.Meet)
        {
            if (aStar.canMove) aStar.canMove = false;
            animator.Play("basic_melee_idle");
        }
        else if (nextState == BasicMeleeState.Aggro)
        {
            if (!aStar.canMove) aStar.canMove = true;
            animator.Play("basic_melee_move");
        }
        else if (nextState == BasicMeleeState.Attack)
        {
            if (aStar.canMove) aStar.canMove = false;
            animator.Play("basic_melee_attack");
        }
        else
        {
            if (aStar.canMove) aStar.canMove = false;
            animator.Play("basic_melee_idle");
        }
        currentState = nextState;
    }
}
