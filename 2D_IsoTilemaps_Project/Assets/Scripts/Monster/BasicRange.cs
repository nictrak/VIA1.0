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
    [SerializeField]
    private int deathFrame;
    [SerializeField]
    private int hurtFrame;
    [SerializeField]
    private float knockVelocity;

    private BasicRangeState currentState;
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
    private MonsterHealth monsterHealth;
    private int stateCounter;
    private Rigidbody2D rgbody;

    public enum BasicRangeState
    {
        Idle,
        Meet,
        Escape,
        Hurt,
        Death
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = BasicRangeState.Idle;
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
        monsterHealth = GetComponent<MonsterHealth>();
        stateCounter = 0;
        rgbody = GetComponent<Rigidbody2D>();
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
    private Vector2 CalKnockVelocityVector()
    {
        Vector2 direction = transform.position - player.transform.position;
        Vector2 result = direction.normalized * knockVelocity;
        return result;
    }

    private void StateMachineRunningPerFrame()
    {
        BasicRangeState nextState = BasicRangeState.Idle;
        if (currentState == BasicRangeState.Meet)
        {
            if (canAttack)
            {
                LinearBullet spawned = Instantiate<LinearBullet>(linearBullet);
                spawned.Setup(transform.position, player.transform.position, bulletVelocity, damage);
                canAttack = false;
            }
            if (!IsPlayerInMeetRange()) nextState = BasicRangeState.Idle;
            else if (IsPlayerInEscapeRange()) nextState = BasicRangeState.Escape;
            else nextState = BasicRangeState.Meet;
        }
        else if (currentState == BasicRangeState.Escape)
        {
            if (!IsPlayerInMeetRange()) nextState = BasicRangeState.Idle;
            else if (!IsPlayerInEscapeRange()) nextState = BasicRangeState.Meet;
            else nextState = BasicRangeState.Escape;
        }
        else if (currentState == BasicRangeState.Death)
        {
            if (stateCounter >= deathFrame)
            {
                nextState = BasicRangeState.Death;
                monsterHealth.Die();
            }
            else
            {
                stateCounter++;
                nextState = BasicRangeState.Death;
            }
        }
        else if (currentState == BasicRangeState.Hurt)
        {
            if (stateCounter >= hurtFrame)
            {
                nextState = BasicRangeState.Meet;
            }
            else
            {
                stateCounter++;
                nextState = BasicRangeState.Hurt;
            }
        }
        else
        {
            // Check if meet
            if (IsPlayerInMeetRange()) nextState = BasicRangeState.Meet;
            else nextState = BasicRangeState.Idle;
        }
        if (currentState != BasicRangeState.Hurt && monsterHealth.IsHurt)
        {
            nextState = BasicRangeState.Hurt;
        }
        if (currentState != BasicRangeState.Death && monsterHealth.IsDie)
        {
            nextState = BasicRangeState.Death;
        }
        if(nextState != currentState) changeState(nextState);
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
    private void changeState(BasicRangeState nextState)
    {
        if (nextState == BasicRangeState.Meet)
        {
            fleeController.IsEnable = false;
        }
        else if (nextState == BasicRangeState.Escape)
        {
            fleeController.IsEnable = true;
            animator.Play("basic_range_move");
        }
        else if (nextState == BasicRangeState.Death)
        {
            fleeController.IsEnable = false;
            stateCounter = 0;
            animator.Play("basic_range_dead");
        }
        else if (nextState == BasicRangeState.Hurt)
        {
            fleeController.IsEnable = false;
            stateCounter = 0;
            attackCooldownCounter = 0;
            rgbody.velocity = CalKnockVelocityVector();
            animator.Play("basic_range_hurt");
        }
        else
        {
            fleeController.IsEnable = false;
            animator.Play("basic_range_idle");
        }
        currentState = nextState;
    }
}
