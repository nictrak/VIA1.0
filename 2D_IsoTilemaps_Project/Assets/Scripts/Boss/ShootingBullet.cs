using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBullet : MonoBehaviour
{
    //Serialized Inspector
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private List<GameObject> bulletSpawners;
    [SerializeField]
    private List<Vector2> bulletVelocityVectors;
    
    // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SpawnBullets()
    {
        int i;
        GameObject spawner;
        Vector2 velocityVector;
        GameObject spawned;
        for(i = 0; i < bulletSpawners.Count; i++)
        {
            // setup
            spawner = bulletSpawners[i];
            velocityVector = bulletVelocityVectors[i];
            // Instantiate
            spawned = Instantiate(bulletPrefab);
            // set position 
            spawned.transform.position = spawner.transform.position;
            // set velocity
            spawned.GetComponent<BossBullet>().VelocityVector = velocityVector;
        }
    }
}
