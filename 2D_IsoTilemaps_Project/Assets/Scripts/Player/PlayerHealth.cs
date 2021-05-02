using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int initialHealth;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private GameObject healthBarFade;
    [SerializeField]
    private GameObject bloodDamage;
    [SerializeField]
    private float shrinkSpeed = 0.0005f;
    [SerializeField]
    private float bloodDamageTime = 0.25f;

    private Image healthBar;
    private Image damageBar;
    private Image bloodEffect;
    private Color bloodColor;

    private float bloodDamageTimer = 0f;

    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = initialHealth;
        healthBar = healthBarFade.transform.Find("healthbar").GetComponent<Image>();
        damageBar = healthBarFade.transform.Find("damagebar").GetComponent<Image>();
        bloodEffect = bloodDamage.GetComponent<Image>();
        bloodColor = bloodEffect.color;
        bloodColor.a = 0f;
        bloodEffect.color = bloodColor;
        bloodDamage.active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar.fillAmount < damageBar.fillAmount){
            damageBar.fillAmount -= shrinkSpeed;
        } else if (healthBar.fillAmount > damageBar.fillAmount){
            damageBar.fillAmount = healthBar.fillAmount;
        }

        if ( bloodColor.a > 0 ){
            bloodDamageTimer -= Time.deltaTime;
            if ( bloodDamageTimer < 0 ) {
                bloodColor.a -= 5f * Time.deltaTime;
                bloodEffect.color = bloodColor;
            }
        }
    }

    private void updateHealthBar()
    {
        float percentage = (float)currentHealth / (float)maxHealth;
        healthBar.fillAmount = percentage;
    }

    public void DealDamage(int damage)
    {
        if (currentHealth >= 0) {
            currentHealth -= damage;
            if (damage > 0) {
                bloodDamageTimer = bloodDamageTime;
                bloodColor.a = 0.8f;
                bloodEffect.color = bloodColor;
            }
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            updateHealthBar();
        } else {
            currentHealth = 0;
            updateHealthBar();
        }

    }
}
