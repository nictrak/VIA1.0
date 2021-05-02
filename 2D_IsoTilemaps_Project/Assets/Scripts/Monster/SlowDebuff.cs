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
    [SerializeField]
    private int deathFrame;
    [SerializeField]
    private int hurtFrame;

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
    private Animator animator;
    private MonsterHealth monsterHealth;
    private int stateCounter;

    public enum SlowDebuffState
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
        animator = GetComponentInChildren<Animator>();
        monsterHealth = GetComponent<MonsterHealth>();
        stateCounter = 0;
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
            //spriteRenderer.color = Color.red;
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
            //spriteRenderer.color = Color.cyan;
            if (!IsPlayerInMeetRange()) nextState = SlowDebuffState.Idle;
            else if (!IsPlayerInEscapeRange()) nextState = SlowDebuffState.Meet;
            else nextState = SlowDebuffState.Escape;
        }
        else if (currentState == SlowDebuffState.Death)
        {
            if (stateCounter >= deathFrame)
            {
                nextState = SlowDebuffState.Death;
                monsterHealth.Die();
            }
            else
            {
                stateCounter++;
                nextState = SlowDebuffState.Death;
            }
        }
        else if (currentState == SlowDebuffState.Hurt)
        {
            if (stateCounter >= hurtFrame)
            {
                nextState = SlowDebuffState.Meet;
            }
            else
            {
                stateCounter++;
                nextState = SlowDebuffState.Hurt;
            }
        }
        else
        {
            // Check if meet
            if (IsPlayerInMeetRange()) nextState = SlowDebuffState.Meet;
            else nextState = SlowDebuffState.Idle;
        }
        if (currentState != SlowDebuffState.Hurt && monsterHealth.IsHurt)
        {
            nextState = SlowDebuffState.Hurt;
        }
        if (currentState != SlowDebuffState.Death && monsterHealth.IsDie)
        {
            nextState = SlowDebuffState.Death;
        }
        if (nextState != currentState) changeState(nextState);
    }
    private void AttackCooldownPerFrame()
    {
        if (!canAttack)
        {
            if (attackCooldownCounter == 0)
            {
                animator.Play("attack");
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
        }
        else if (nextState == SlowDebuffState.Escape)
        {
            fleeController.IsEnable = true;
            animator.Play("debuff move");
        }
        else if (nextState == SlowDebuffState.Death)
        {
            fleeController.IsEnable = false;
            stateCounter = 0;
            animator.Play("dead");
        }
        else if (nextState == SlowDebuffState.Hurt)
        {
            fleeController.IsEnable = false;
            stateCounter = 0;
            attackCooldownCounter = 0;
            animator.Play("hurt");
        }
        else
        {
            fleeController.IsEnable = false;
            animator.Play("debuff idle");
        }
        currentState = nextState;
    }
}
