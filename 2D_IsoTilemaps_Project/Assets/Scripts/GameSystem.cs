using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    [SerializeField]
    private GridLayout grid;

    public static GridLayout GridSystem;
    // Start is called before the first frame update
    void Start()
    {
        GridSystem = grid;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
