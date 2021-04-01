using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearBullet : MonoBehaviour
{

    private float velocity;
    private Vector2 direction;
    private int damage;
    private Rigidbody2D rbody;


    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MovePerFrame();
    }

    private void MovePerFrame()
    {
        Vector2 newPos = rbody.position + direction * velocity;
        rbody.MovePosition(newPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().DealDamage(damage);
            Destroy(gameObject);
        }
    }
    public void Setup(Vector2 start, Vector2 target, float velocity,int damage)
    {
        transform.position = start;
        direction = (target - start).normalized;
        this.damage = damage;
        this.velocity = velocity;
    }
}
