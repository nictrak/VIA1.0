using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    private GridLayout gridLayout;
    private Vector2 position;

    public Vector2 Position { get => position;}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }
    private void UpdatePosition()
    {
        if(gridLayout == null)
        {
            gridLayout = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridLayout>();
        }
        Vector3 rawPosition = gridLayout.WorldToCell(transform.position);
        position = new Vector2(rawPosition.x, rawPosition.y);
    }
    public float CalculateMagnitudeToOther(GridPosition other)
    {
        return (position - other.position).magnitude;
    }
}
