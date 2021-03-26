using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOnDestroy : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        GameObject spawned =  Instantiate(gameObject);
        spawned.transform.position = transform.position;
    }
}
