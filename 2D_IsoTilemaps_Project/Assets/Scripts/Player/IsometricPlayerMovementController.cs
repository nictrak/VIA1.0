                                                                                                                        using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsometricPlayerMovementController : MonoBehaviour
{

    public float movementSpeed = 1f;
	private double curTime = 0.0;
	public double comboTimer = 1.5;
    IsometricCharacterRenderer isoRenderer;
    public LayerMask enermyLayers;
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

    [SerializeField]
    private GameObject dashBar;
    private Image sphere1;
    private Image sphere2;

    //variable for attacking
    private IsometricCharacterRenderer.States currentAttackState;
    private IsometricCharacterRenderer.States animatedAttackState;
    private int attackCounter;
    [SerializeField]
    private List<int> attackFrames;
    private int maxAttackState;
    private bool isDash;
    [SerializeField]
    private int dashFrame;
    private int dashCounter;

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
        sphere1 = dashBar.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        sphere2 = dashBar.transform.GetChild(1).transform.GetChild(1).GetComponent<Image>();
        playerAttackHitbox = GetComponent<PlayerAttackHitbox>();
        currentAttackState = IsometricCharacterRenderer.States.none;
        animatedAttackState = IsometricCharacterRenderer.States.none;
        attackCounter = 0;
        maxAttackState = 3;
        dashCounter = 0;
        isDash = false;
    }
    
	
	void Update()
	{
		Vector2 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
		
		
        if (Input.GetKeyDown(KeyCode.X) && currentDashCharge > 0 && isEnable)
        {
            dashVector = inputVector * DashMultiplier;
            currentDashCharge -= 1;
            if (currentDashCharge == 0) sphere1.enabled = false;
            else if (currentDashCharge == 1) sphere2.enabled = false;
            isoRenderer.DashDirection(movement);
            isDash = true;
        }
        if (Input.GetKeyDown(KeyCode.Z) && (int)currentAttackState < maxAttackState)
        {
            currentAttackState += 1;
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
        if (isEnable) rbody.MovePosition(newPos);
        if(currentDashCharge < MaxDashCharge)
        {
            if(dashRechargeCounter >= DashRehargeFrame)
            {
                currentDashCharge++;
                if (currentDashCharge == 1) sphere1.enabled = true;
                else sphere2.enabled = true;
                dashRechargeCounter = 0;
            }
            else
            {
                dashRechargeCounter++;
            }
        }
        if (isDash)
        {
            if (dashCounter <= 0) isoRenderer.DashDirection(movement);
            if (dashCounter >= dashFrame)
            {
                isDash = false;
                dashCounter = 0;
            }
            else dashCounter++;
        }
        else
        {
            if (currentAttackState == IsometricCharacterRenderer.States.none)
            {
                isoRenderer.SetDirection(movement);
            }
            if (currentAttackState > animatedAttackState)
            {
                animatedAttackState += 1;
                isoRenderer.AttackDirection(inputVector, animatedAttackState);
                attackCounter = 0;
                playerAttackHitbox.HitboxDealDamage((int)animatedAttackState - 1);
            }
            else if (currentAttackState > 0)
            {
                if (attackCounter > attackFrames[(int)currentAttackState - 1])
                {
                    currentAttackState = IsometricCharacterRenderer.States.none;
                    animatedAttackState = IsometricCharacterRenderer.States.none;
                    attackCounter = 0;
                }
                else
                {
                    attackCounter++;
                }
            }
        }
        dashVector = new Vector2();
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
