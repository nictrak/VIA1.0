using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeController : MonoBehaviour
{

    public Transform target;
    public float speed = 200f;
    public float aggroRange = 3f;
    public float maxSpread = 0.2f;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= aggroRange)
        {
            float xSpread = Random.RandomRange(-maxSpread, maxSpread);
            float ySpread = Random.RandomRange(-maxSpread, maxSpread);
            Vector2 spreadVector = new Vector2(xSpread, ySpread);

            Vector2 direction = (transform.position - target.position).normalized;
            Vector2 force = (direction+spreadVector).normalized * speed * Time.deltaTime;
            rb.AddForce(force);
        }
    }
}
