﻿                                                                                                                        using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private PlayerHealth playerHealth;

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
    private PlayerAttackController playerAttackController;
    private bool isDash;
    [SerializeField]
    private int dashFrame;
    private int dashCounter;
    private bool isRenderDeath;
    [SerializeField]
    private int deathFrame;
    private int deathCounter;


    [SerializeField]
    private float moveSpeedWhenAttackMultiplier;

    [SerializeField]
    private AudioSource dash;


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
        if(dashBar == null)
        {
            dashBar = GameObject.FindGameObjectWithTag("HUD").transform.GetChild(2).gameObject;
        }
        sphere1 = dashBar.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        sphere2 = dashBar.transform.GetChild(1).transform.GetChild(1).GetComponent<Image>();
        playerAttackHitbox = GetComponent<PlayerAttackHitbox>();
        currentAttackState = IsometricCharacterRenderer.States.none;
        animatedAttackState = IsometricCharacterRenderer.States.none;
        attackCounter = 0;
        maxAttackState = 3;
        dashCounter = 0;
        isDash = false;
        playerAttackController = GetComponent<PlayerAttackController>();
        playerHealth = GetComponent<PlayerHealth>();
        isRenderDeath = false;
        deathCounter = 0;
    }
    
	
	void Update()
	{
		Vector2 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
		
        if (Input.GetKeyDown(KeyCode.Space) && currentDashCharge > 0 && isEnable)
        {
            dashVector = inputVector * DashMultiplier;
            currentDashCharge -= 1;
            if (currentDashCharge == 0) sphere1.enabled = false;
            else if (currentDashCharge == 1) sphere2.enabled = false;
            isDash = true;
	        dash.Play();
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

        if (playerHealth.IsDie)
        {
            if (!isRenderDeath)
            {
                isoRenderer.AnimateViaDeath();
                isRenderDeath = true;
            }
            if(deathCounter >= deathFrame)
            {
                SceneManager.LoadScene("MenuScene");
            }
            else
            {
                deathCounter++;
            }
        }
        else
        {
            Vector2 currentPos = rbody.position;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
            inputVector = Vector2.ClampMagnitude(inputVector, 1);
            Vector2 movement = inputVector * movementSpeed;
            Vector2 newPos = currentPos + movement * Time.fixedDeltaTime * (IsSlowTilesEmpty() ? 1f : SlowMultiplier) *
                (playerAttackController.IsAttack2() ? moveSpeedWhenAttackMultiplier : 1f) + dashVector;
            if (isEnable) rbody.MovePosition(newPos);
            if (currentDashCharge < MaxDashCharge)
            {
                if (dashRechargeCounter >= DashRehargeFrame)
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

                if (dashCounter <= 0)
                {
                    Debug.Log("Dash");
                    isoRenderer.DashDirection(movement);
                }
                if (dashCounter >= dashFrame)
                {
                    isDash = false;
                    dashCounter = 0;
                }
                else dashCounter++;
                Debug.Log(dashCounter);
            }
            else
            {
                playerAttackController.AttackPerFrame2(inputVector, movement, isoRenderer);
            }
            dashVector = new Vector2();
        }
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
