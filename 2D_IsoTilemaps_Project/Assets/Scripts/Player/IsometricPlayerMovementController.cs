                                                                                                                        using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private PlayerAttackController playerAttackController;
    private bool isDash;
    [SerializeField]
    private int dashFrame;
    private int dashCounter;
    [SerializeField]
    private float moveSpeedWhenAttackMultiplier;


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
        dashCounter = 0;
        isDash = false;
        playerAttackController = GetComponent<PlayerAttackController>();
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
            isoRenderer.DashDirection(movement);
            isDash = true;
        }
        playerAttackController.KeyAttack2(isoRenderer);

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
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime * (IsSlowTilesEmpty()?1f:SlowMultiplier) * 
            (playerAttackController.IsAttack2()?moveSpeedWhenAttackMultiplier:1f)  + dashVector;
        if (isEnable) rbody.MovePosition(newPos);
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
            playerAttackController.AttackPerFrame2(inputVector, movement, isoRenderer);
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
