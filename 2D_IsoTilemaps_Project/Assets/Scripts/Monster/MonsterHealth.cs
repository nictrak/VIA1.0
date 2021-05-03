using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField]
    private GameObject onHitEffectPrefab;
    [SerializeField]
    private float randomRange;

    public int maxHealth = 100;
    private int currentHealth;

    public GameObject prefab;
    public GameObject healthBar;
    public int hurtFram = 1;

	private bool attacked = false;
	private float attackedTime = 0f;
    private float scaleY;
    private float maxScaleX;
    private bool isDie;
    private bool isHurt;
    private int hurtCounter;
    
    [SerializeField]
    private AudioSource monster_hurt;
    [SerializeField]
    private AudioSource monster_die;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public bool IsDie { get => isDie; set => isDie = value; }
    public bool IsHurt { get => isHurt; set => isHurt = value; }

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        currentHealth = maxHealth;
        if(healthBar != null)
        {
            scaleY = healthBar.transform.localScale.y;
            maxScaleX = healthBar.transform.localScale.x;
        }
        isDie = false;
        isHurt = false;
        hurtCounter = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if(healthBar != null) updateHealthBar();
		if(attacked && Time.time > attackedTime + 0.25f ) spriteRenderer.color = Color.white;
    }
    private void FixedUpdate()
    {
        if (isHurt)
        {
            monster_hurt.Play();
            if (hurtCounter >= hurtFram)
            {
                hurtCounter = 0;
                isHurt = false;
            }
            else
            {
                hurtCounter++;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
		spriteRenderer.color = new Color(1, 0, 0, 0.75f);
        attacked = true;
        monster_hurt.Play();
        attackedTime = Time.time;
        isHurt = true;
        SpawnEffect();
        if (currentHealth <= 0)
        {
            isDie = true;
            monster_die.Play();
        }
    }

    public void Die()
    {
        if(prefab != null) Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void updateHealthBar()
    {
        float scaleX = (float)currentHealth / (float)maxHealth * maxScaleX;
        healthBar.transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
    private void SpawnEffect()
    {
        float randomX = transform.position.x + Random.Range(0f, randomRange);
        float randomY = transform.position.y + Random.Range(0f, randomRange);
        Vector3 spawnedPosition = new Vector3(randomX, randomY, 0);
        GameObject spawned = Instantiate(onHitEffectPrefab);
        spawned.transform.position = spawnedPosition;
    }
}
