using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimeLimit : MonoBehaviour
{
    [SerializeField]
    private int lifeTime;
    private int lifeCounter;
    // Start is called before the first frame update
    void Start()
    {
        lifeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        TerminateAtEndPerFrame();
    }
    private void TerminateAtEndPerFrame()
    {
        if (lifeCounter >= lifeTime)
        {
            Destroy(gameObject);
        }
        else lifeCounter++;
    }
}
