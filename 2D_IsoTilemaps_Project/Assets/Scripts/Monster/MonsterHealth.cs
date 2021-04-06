using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public GameObject prefab;
    public GameObject healthBar;

    private float scaleY;
    private float maxScaleX;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        if(healthBar != null)
        {
            scaleY = healthBar.transform.localScale.y;
            maxScaleX = healthBar.transform.localScale.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(healthBar != null) updateHealthBar();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if(prefab != null) Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void updateHealthBar()
    {
        float scaleX = (float)currentHealth / (float)maxHealth * maxScaleX;
        healthBar.transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}
