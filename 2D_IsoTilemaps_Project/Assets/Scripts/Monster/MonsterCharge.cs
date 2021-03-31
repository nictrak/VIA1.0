using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCharge : MonoBehaviour
{
    [SerializeField]
    private float chargeVelocity;
    [SerializeField]
    private int chargeFrame;
    [SerializeField]
    private float knockVelocity;
    [SerializeField]
    private int knockFrame;
    [SerializeField]
    private int damage;

    private bool isCharge;
    private int chargeCounter;
    private Vector2 chargeVector;
    private Rigidbody2D rbody;
    private bool isKnock;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        ChargePerFrame();
    }
    public void Charge(Vector2 targetPosition)
    {
        //setup
        isCharge = true;
        isKnock = false;
        chargeCounter = 0;
        chargeVector = (targetPosition - rbody.position).normalized * chargeVelocity;
    }
    private void ChargePerFrame()
    {
        if (isCharge)
        {
            if (chargeCounter >= chargeFrame)
            {
                isCharge = false;
            }
            else
            {
                Vector2 newPos = rbody.position + chargeVector;
                rbody.MovePosition(newPos);
                chargeCounter++;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(isCharge && !isKnock)
            {
                collision.gameObject.GetComponent<PlayerKnocked>().Knocked(rbody.position, knockVelocity, knockFrame);
                collision.gameObject.GetComponent<PlayerHealth>().DealDamage(damage);
                isKnock = true;
            }
        }
    }
}
