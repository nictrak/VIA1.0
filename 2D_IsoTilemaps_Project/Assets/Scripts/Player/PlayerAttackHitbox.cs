using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    [SerializeField]
    private List<AttackHitbox> hitboxes;
    [SerializeField]
    private List<float> distances;
    [SerializeField]
    private List<int> damages;

    private List<Vector2> basedOffset;
    private Vector2 up = new Vector2(0, 1);
    private Vector2 down = new Vector2(0, -1);
    private Vector2 left = new Vector2(-1, 0);
    private Vector2 right = new Vector2(1, 0);
    // Start is called before the first frame update
    void Start()
    {
        basedOffset = new List<Vector2>();
        SetupBasedOffset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupBasedOffset()
    {
        for (int i = 0; i < hitboxes.Count; i++)
        {
            basedOffset.Add(hitboxes[i].GetComponent<CircleCollider2D>().offset);
        }
    }
    private void UpdateSingleHitboxOffset(int index, Vector2 direction)
    {
        Vector2 newOffset = basedOffset[index] + distances[index] * direction;
        Debug.Log(newOffset);
        hitboxes[index].GetComponent<Collider2D>().offset = newOffset;
    }
    public void UpdateAllHitboxOffset(int direction)
    {
        Vector2 directionVec = IntDirectionToVector(direction);
        for (int i = 0; i < hitboxes.Count; i++)
        {
            UpdateSingleHitboxOffset(i, directionVec);
        }
    }
    public Vector2 IntDirectionToVector(int direction)
    {
        Vector2 result = new Vector2();
        if(direction == 0)
        {
            result = up;
        }else if (direction == 1)
        {
            result = (up + left).normalized;
        }
        else if (direction == 2)
        {
            result = left;
        }
        else if (direction == 3)
        {
            result = (down + left).normalized;
        }
        else if (direction == 4)
        {
            result = down;
        }
        else if (direction == 5)
        {
            result = (down + right).normalized;
        }
        else if (direction == 6)
        {
            result = right;
        }
        else if (direction == 7)
        {
            result = (up + right).normalized;
        }
        return result;
    }
    public void HitboxDealDamage(int index)
    {
        hitboxes[index].DamageAll(damages[index]);
    }
}
