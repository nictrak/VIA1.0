using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Tank : MonoBehaviour
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
    [SerializeField]
    private GameObject pointer;

    private BasicMeleeState currentState;
    private GameObject player;
    private GridPosition playerGridPosition;
    private PlayerHealth playerHealth;
    private GridPosition gridPosition;
    private AIPath aStar;
    private int attackCooldownCounter;
    private int attackTimeCounter;
    private bool canAttack;
    private SpriteRenderer spriteRenderer;
    private GameObject returnPoint;
    private AIDestinationSetter destinationSetter;

    public enum BasicMeleeState
    {
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
        attackCooldownCounter = 0;
        attackTimeCounter = 0;
        canAttack = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        returnPoint = Instantiate(pointer);
        returnPoint.transform.position = transform.position;
        destinationSetter = GetComponent<AIDestinationSetter>();
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
    public bool IsMoveToReturn()
    {
        if(((Vector2)(returnPoint.transform.position - transform.position)).magnitude > 0.1)
        {
            return true;
        }
        return false;
    }
    private void StateMachineRunningPerFrame()
    {
        BasicMeleeState nextState = BasicMeleeState.Idle;
        if (currentState == BasicMeleeState.Meet)
        {
            if (aStar.canMove) aStar.canMove = false;
            //spriteRenderer.color = Color.cyan;
            // Check if aggro
            if (!IsPlayerInMeetRange()) nextState = BasicMeleeState.Idle;
            else if (IsPlayerInAggroRange()) nextState = BasicMeleeState.Aggro;
            else nextState = BasicMeleeState.Meet;
        }
        else if (currentState == BasicMeleeState.Aggro)
        {
            //spriteRenderer.color = Color.green;
            destinationSetter.target = player.transform;
            if (!aStar.canMove) aStar.canMove = true;
            if (!IsPlayerInMeetRange()) nextState = BasicMeleeState.Idle;
            else if (IsPlayerInAttackRange() && canAttack) nextState = BasicMeleeState.Attack;
            else nextState = BasicMeleeState.Aggro;
        }
        else if (currentState == BasicMeleeState.Attack)
        {
            //spriteRenderer.color = Color.red;
            if (aStar.canMove) aStar.canMove = false;
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
            if (IsMoveToReturn())
            {
                destinationSetter.target = returnPoint.transform;
                aStar.canMove = true;
            }
            else
            {
                destinationSetter.target = player.transform;
                aStar.canMove = false;
            }
            //spriteRenderer.color = Color.white;
            // Check if meet
            if (IsPlayerInMeetRange()) nextState = BasicMeleeState.Meet;
            else nextState = BasicMeleeState.Idle;
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
