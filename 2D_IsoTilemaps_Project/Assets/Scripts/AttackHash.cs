using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHash : MonoBehaviour
{
    [SerializeField]
    private List<string> keys;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getKeyIndex(string word)
    {
        int res = keys.IndexOf(word);
        if(res == -1)
        {
            res = 0;
        }
        return res;
    }
}
