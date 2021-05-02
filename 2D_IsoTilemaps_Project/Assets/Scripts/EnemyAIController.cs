using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAIController : MonoBehaviour
{

    public Transform targetToMove;
    public Transform targetToTrigger;
    public float speed = 200f;
    public float nextWaypointDistance = .5f;
    public float aggroRange = 3f;
    public bool isAddForce = false;
    private SpriteRenderer spriteRenderer;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (targetToMove == null) targetToMove = playerTransform;
        if (targetToTrigger == null) targetToTrigger = playerTransform;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void UpdatePath()
    {
        if(seeker.IsDone() && Vector3.Distance(targetToTrigger.position, transform.position) <= aggroRange )
        {
            seeker.StartPath(rb.position, targetToMove.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null) return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } else 
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if(isAddForce) rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        if (targetToMove.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
}
