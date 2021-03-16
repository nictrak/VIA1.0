using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{

    public float movementSpeed = 1f;
    public float attackRange = 1f;
	private bool comboing = false;
	private bool comboing2 = false;
	private double curTime = 0.0;
	public double comboTimer = 1.5;
	public double attackDelay = 0.2;
	private int comboCount = 1;
    IsometricCharacterRenderer isoRenderer;
    public Transform attackPoint ;
    public LayerMask enermyLayers;
    public int attackDamage = 40;
    Rigidbody2D rbody;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
    }
    
    void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                 Debug.Log("enemy");
                // collision.gameObject.SendMessage("ApplyDamage", 10);
            }

            if (collision.gameObject.tag == "Heal")
            {
                 Debug.Log("heal");
                // // collision.gameObject.SendMessage("ApplyDamage", 10);
                // Destroy(collision.gameObject);
                // Destroy(collision.gameObject);
            }
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
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        isoRenderer.SetDirection(movement);
        rbody.MovePosition(newPos);	
		
		if (Input.GetKey(KeyCode.Z) && curTime + attackDelay < Time.time ){
            // CMDebug.TextPopupMouse("Attack !!!");
			if (!comboing && !comboing2)
			{
				comboing = true;	
				comboCount = 1;
			}
			else if (comboing && !comboing2 && curTime + comboTimer > Time.time)
			{
				comboing = false;
				comboing2 = true;
				comboCount = 2;
			}
			else if (!comboing && comboing2 && curTime + comboTimer > Time.time)
			{
				comboing = false;
				comboing2 = false;
				comboCount = 3;
			}
			curTime = Time.time;
            isoRenderer.AttackDirection(movement, comboCount);
            Debug.Log("Attack !!!");
            Collider2D[] hitEnermies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enermyLayers);
            foreach(Collider2D enermy in hitEnermies ){
                // Debug.Log("hit"+attackDamage);
                Debug.Log("hit"+enermy.ToString());
				Debug.Log(enermy.GetComponent<MonsterMove>());
                enermy.GetComponent<MonsterHealth>().TakeDamage(attackDamage);
            }
        }
		
		if (( comboing || comboing2 ) && curTime + comboTimer < Time.time)
		{
			Debug.Log("Reset !!!");
			comboing = false;
			comboing2 = false;
		}
		
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null){
            return ;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange/10);
    }
}
