using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    private int collisionDamage;
    [SerializeField]
    private int DamageInterval = 100;

    private bool isEnable;

    public int CollisionDamage { get => collisionDamage; set => collisionDamage = value; }
    public bool IsEnable { get => isEnable; set => isEnable = value; }

    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        isEnable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isEnable)
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            PlayerKnocked playerKnocked = collision.gameObject.GetComponent<PlayerKnocked>();
            if (counter == 0)
            {
                playerHealth.DealDamage(CollisionDamage);
                counter++;
            }
            else if (counter >= DamageInterval)
            {
                counter = 0;
            }
            else
            {
                counter++;
            }
        }
    }
    public void Setup(Vector2 position, int damage)
    {
        transform.position = position;
        collisionDamage = damage;
    }
}
