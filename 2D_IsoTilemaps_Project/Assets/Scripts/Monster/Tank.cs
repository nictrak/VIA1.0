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
    private GameObject pointerPrefab;

    private TankState currentState;
    private GameObject player;
    private GridPosition playerGridPosition;
    private PlayerHealth playerHealth;
    private GridPosition gridPosition;
    private AIPath aStar;
    private int attackCooldownCounter;
    private int attackTimeCounter;
    private bool canAttack;
    private SpriteRenderer spriteRenderer;
    private GameObject campPointer;
    private MonsterCharge monsterCharge;

    public enum TankState
    {
        Idle,
        Meet,
        Aggro,
        Attack,
        Back
    }
    // Start is called before the first frame update
    void Start()
    {
        currentState = TankState.Idle;
        player = GameObject.FindGameObjectWithTag("Player");
        playerGridPosition = player.GetComponent<GridPosition>();
        playerHealth = player.GetComponent<PlayerHealth>();
        gridPosition = GetComponent<GridPosition>();
        aStar = GetComponent<AIPath>();
        attackCooldownCounter = 0;
        attackTimeCounter = 0;
        canAttack = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        monsterCharge = GetComponent<MonsterCharge>();
    }
    private void FixedUpdate()
    {
        StateMachineRunningPerFrame();
        AttackCooldownPerFrame();
    }
    // Update is called once per frame
    void Update()
    {
        
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
    private void StateMachineRunningPerFrame()
    {
        TankState nextState = TankState.Idle;
        if (currentState == TankState.Meet)
        {
            if (aStar.canMove) aStar.canMove = false;
            spriteRenderer.color = Color.cyan;
            // Check if aggro
            if (!IsPlayerInMeetRange()) nextState = TankState.Idle;
            else if (IsPlayerInAggroRange()) nextState = TankState.Aggro;
            else nextState = TankState.Meet;
        }
        else if (currentState == TankState.Aggro)
        {
            spriteRenderer.color = Color.green;
            if (!aStar.canMove) aStar.canMove = true;
            if (!IsPlayerInMeetRange()) nextState = TankState.Idle;
            else if (IsPlayerInAttackRange() && canAttack) nextState = TankState.Attack;
            else nextState = TankState.Aggro;
        }
        else if (currentState == TankState.Attack)
        {
            spriteRenderer.color = Color.red;
            if (aStar.canMove) aStar.canMove = false;
            if (attackTimeCounter >= attckTimeFrame)
            {
                canAttack = false;
                attackTimeCounter = 0;
                nextState = TankState.Aggro;
            }
            else 
            {
                if(attackTimeCounter == 0)
                {
                    monsterCharge.Charge(player.transform.position);
                }
                attackTimeCounter++;
                nextState = TankState.Attack;
            }
        }
        else
        {
            if (aStar.canMove) aStar.canMove = false;
            spriteRenderer.color = Color.white;
            // Check if meet
            if (IsPlayerInMeetRange()) nextState = TankState.Meet;
            else nextState = TankState.Idle;
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
