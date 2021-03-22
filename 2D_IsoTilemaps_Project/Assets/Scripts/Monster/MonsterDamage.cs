using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    [SerializeField]
    private int collisionDamage;
    [SerializeField]
    private int DamageInterval;

    private bool isEnable;

    public int CollisionDamage { get => collisionDamage; set => collisionDamage = value; }

    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        isEnable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && isEnable)
        {
            Debug.Log(gameObject);
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (counter == 0)
            {
                playerHealth.DealDamage(CollisionDamage);                                                                       
                counter++;
            }
            else if(counter >= DamageInterval)
            {
                counter = 0;
            }
            else
            {
                counter++;
            }
        }
    }
}
