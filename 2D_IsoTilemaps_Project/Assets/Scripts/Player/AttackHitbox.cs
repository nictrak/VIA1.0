using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemies;
    [SerializeField]
    private float knockVelocity = 0.25f;
    [SerializeField]
    private int knockFrame = 1;

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
            if(enemies[i].GetComponent<MonsterKnocked>() != null) {
                enemies[i].GetComponent<MonsterKnocked>().Knocked(transform.position, knockVelocity, knockFrame);
                print("Work");
            }
            enemies[i].GetComponent<MonsterHealth>().TakeDamage(damage);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            enemies.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemies.Remove(collision.gameObject);
        }
    }
}
