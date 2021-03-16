using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int SpawnInterval;
    public GameObject Obj;
    private int counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (counter >= SpawnInterval)
        {
            GameObject spawned = Instantiate(Obj);
            Obj.transform.position = this.transform.position;
            counter = 0;
        }
        else counter++;
    }
}
