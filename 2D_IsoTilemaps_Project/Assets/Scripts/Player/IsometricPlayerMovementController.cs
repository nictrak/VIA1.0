                                                                                                                        using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{

    public float movementSpeed = 1f;
    public float attackRange = 1f;
	private IsometricCharacterRenderer.States attackState = IsometricCharacterRenderer.States.first;
	private double curTime = 0.0;
	public double comboTimer = 1.5;
	public double attackDelay = 0.2;
	private int comboCount = 1;
    IsometricCharacterRenderer isoRenderer;
    public Transform attackPoint ;
    public LayerMask enermyLayers;
    public int attackDamage = 40;
	private int i = 0;
    Rigidbody2D rbody;
    public float DashMultiplier;
    public int MaxDashCharge;
    public int DashRehargeFrame;
    [SerializeField]
    private int currentDashCharge;
    [SerializeField]
    private int dashRechargeCounter;
    private Vector2 dashVector;
    private bool isEnable;
    [SerializeField]
    private float SlowMultiplier = 0.5f;
    [SerializeField]
    private List<GameObject> slowTiles;
    private PlayerAttackHitbox playerAttackHitbox;
    public bool IsEnable { get => isEnable; set => isEnable = value; }

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
        dashVector = new Vector2();
        currentDashCharge = MaxDashCharge;
        dashRechargeCounter = 0;
        isEnable = true;
        slowTiles = new List<GameObject>();
        playerAttackHitbox = GetComponent<PlayerAttackHitbox>();
    }
    
	
	void Update()
	{
		Vector2 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
		
		if (Input.GetKeyDown(KeyCode.Z) && curTime + attackDelay < Time.time ){
            // CMDebug.TextPopupMouse("Attack !!!");
            playerAttackHitbox.HitboxDealDamage((int)attackState);
            isoRenderer.AttackDirection(movement, attackState);

            if (attackState == IsometricCharacterRenderer.States.first)
			{
				attackState = IsometricCharacterRenderer.States.second;
			}
			else if (attackState == IsometricCharacterRenderer.States.second && curTime + comboTimer > Time.time)
			{
				attackState = IsometricCharacterRenderer.States.third;
			}
			else if (attackState == IsometricCharacterRenderer.States.third && curTime + comboTimer > Time.time)
			{
				attackState = IsometricCharacterRenderer.States.first;
			}
			curTime = Time.time;
        }
				
		if (attackState != IsometricCharacterRenderer.States.first && curTime + comboTimer < Time.time)
		{
			Debug.Log("Reset !!!");
			attackState = IsometricCharacterRenderer.States.first;
		}
        if (Input.GetKeyDown(KeyCode.X) && currentDashCharge > 0 && isEnable)
        {
            dashVector = inputVector * DashMultiplier;
            currentDashCharge -= 1;
            isoRenderer.DashDirection(movement);
        }
        playerAttackHitbox.UpdateAllHitboxOffset(isoRenderer.LastDirection);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // var list = GetComponents(typeof(Component));
        // for(int i = 0; i < list.Length; i++)
        // {
        //     Debug.Log(list[i].ToString());
        // }



        Vector2 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime * (IsSlowTilesEmpty()?1f:SlowMultiplier)  + dashVector;
        dashVector = new Vector2();
        isoRenderer.SetDirection(movement);
        if(isEnable) rbody.MovePosition(newPos);
        if(currentDashCharge < MaxDashCharge)
        {
            if(dashRechargeCounter >= DashRehargeFrame)
            {
                currentDashCharge++;
                dashRechargeCounter = 0;
            }
            else
            {
                dashRechargeCounter++;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null){
            return ;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange/10);
    }
    private bool IsSlowTilesEmpty()
    {
        if(slowTiles.Count > 0)
        {
            return false;
        }
        return true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slow")
        {
            slowTiles.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slow")
        {
            slowTiles.Remove(collision.gameObject);
        }
    }
}
