using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int initialHealth;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private GameObject healthBar;

    private float scaleY;
    private float maxScaleX;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = initialHealth;
        scaleY = healthBar.transform.localScale.y;
        maxScaleX = healthBar.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void updateHealthBar()
    {
        float scaleX = (float)currentHealth / (float)maxHealth * maxScaleX;
        healthBar.transform.localScale = new Vector3(scaleX, scaleY, 1);
    }

    public void DealDamage(int damage)
    {
        if (currentHealth >= 0) {
            currentHealth -= damage;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            updateHealthBar();
        } else {
            currentHealth = 0;
            updateHealthBar();
        }

    }
}
