using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCannon : MonoBehaviour
{
    [SerializeField]
    private ProjectileMovement spawnPrefab;
    [SerializeField]
    private int spawnFrame;
    [SerializeField]
    private GameObject target;

    private int spawnCounter;
    // Start is called before the first frame update
    void Start()
    {
        spawnCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        SpawnPerFrame();
    }
    private void SpawnPerFrame()
    {
        if(spawnCounter >= spawnFrame)
        {
            ProjectileMovement spawned = Instantiate<ProjectileMovement>(spawnPrefab);
            spawned.Setup(transform.position, target.transform.position, 0.1f);
            spawnCounter = 0;
        }
        else
        {
            spawnCounter++;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }
}
