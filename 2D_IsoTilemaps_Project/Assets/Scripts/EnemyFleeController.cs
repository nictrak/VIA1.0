using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeController : MonoBehaviour
{

    public Transform target;
    public float speed = 200f;
    public float maxSpread = 0.2f;
    public bool IsEnable = false;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEnable)
        {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                180,
                transform.eulerAngles.z
            );
            float xSpread = Random.Range(-maxSpread, maxSpread);
            float ySpread = Random.Range(-maxSpread, maxSpread);
            Vector2 spreadVector = new Vector2(xSpread, ySpread);
            Vector2 direction = (transform.position - target.position).normalized;
            Vector2 force = (direction + spreadVector).normalized * speed * Time.deltaTime;
            rb.AddForce(force);
        }
        else
        {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                0,
                transform.eulerAngles.z
            );
        }
    }
}
