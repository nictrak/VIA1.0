using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public GameObject prefab;
    public GameObject healthBar;

	private bool attacked = false;
	private float attackedTime = 0f;
    private float scaleY;
    private float maxScaleX;
	private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
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
		if(attacked && Time.time > attackedTime + 0.25f ) spriteRenderer.color = Color.white;
    }
    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage");
        currentHealth -= damage;
		
		spriteRenderer.color = Color.blue;
		attacked = true;
		attackedTime = Time.time;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enermy died!");
        if(prefab != null) Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void updateHealthBar()
    {
        float scaleX = (float)currentHealth / (float)maxHealth * maxScaleX;
        healthBar.transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}
