using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private List<GameObject> enemies;
    // Start is called before the first frame update
    void Start()
    {

        enemies = new List<GameObject>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DamageAll(int damage)
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<MonsterHealth>().TakeDamage(damage);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "enemy")
        {
            enemies.Add(collision.gameObject);
        }
    }
}
