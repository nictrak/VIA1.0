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
    [SerializeField]
    private int deathFrame;
    [SerializeField]
    private int hurtFrame;
    [SerializeField]
    private float knockVelocity;
    public AudioSource attacksfx;

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
    private MonsterHealth monsterHealth;
    private int stateCounter;
    private Rigidbody2D rgbody;
    
    public enum BasicMeleeState{
        Idle,
        Meet,
        Aggro,
        Attack,
        Hurt,
        Death
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
        stateCounter = 0;
        monsterHealth = GetComponent<MonsterHealth>();
        attacksfx.Pause();
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
    private Vector2 CalKnockVelocityVector()
    {
        Vector2 direction = transform.position - player.transform.position;
        Vector2 result = direction.normalized * knockVelocity;
        return result;
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
                if (IsPlayerInAttackRange() && playerHealth != null) playerHealth.DealDamage(attackDamage);
                attackTimeCounter = 0;
                attacksfx.Play();
                nextState = BasicMeleeState.Aggro;
            }
            else
            {
                attackTimeCounter++;
                nextState = BasicMeleeState.Attack;
            }
        }
        else if (currentState == BasicMeleeState.Death)
        {
            if(stateCounter >= deathFrame)
            {
                nextState = BasicMeleeState.Death;
                monsterHealth.Die();
            }
            else
            {
                stateCounter++;
                nextState = BasicMeleeState.Death;
            }
        }
        else if (currentState == BasicMeleeState.Hurt)
        {
            
            if (stateCounter >= hurtFrame)
            {
                nextState = BasicMeleeState.Meet;
            }
            else
            {
                stateCounter++;
                nextState = BasicMeleeState.Hurt;
            }
        }
        else
        {
            // Check if meet
            if (IsPlayerInMeetRange()) nextState = BasicMeleeState.Meet;
            else nextState = BasicMeleeState.Idle;
        }
        if(currentState != BasicMeleeState.Hurt && monsterHealth.IsHurt && currentState != BasicMeleeState.Death)
        {
            nextState = BasicMeleeState.Hurt;
        }
        if(currentState != BasicMeleeState.Death && monsterHealth.IsDie)
        {
            nextState = BasicMeleeState.Death;
        }
        if(nextState != currentState) changeState(nextState);
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
        else if (nextState == BasicMeleeState.Death)
        {
            stateCounter = 0;
            if (aStar.canMove) aStar.canMove = false;
            animator.Play("dead");
        }
        else if (nextState == BasicMeleeState.Hurt)
        {
            stateCounter = 0;
            attackCooldownCounter = 0;
            if (aStar.canMove) aStar.canMove = false;
            rgbody.velocity = CalKnockVelocityVector();
            animator.Play("hurt");
        }
        else
        {
            if (aStar.canMove) aStar.canMove = false;
            animator.Play("basic_melee_idle");
        }
        currentState = nextState;
    }
}
