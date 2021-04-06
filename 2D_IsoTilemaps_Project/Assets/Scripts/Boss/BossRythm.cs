using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRythm : MonoBehaviour
{
    [SerializeField]
    private float triggerHealth;

    private MonsterHealth monsterHealth;
    private Boss boss;
    private GameObject player;
    private RythmSystem rythmSystem;
    private bool isAlreadyTrigger;
    // Start is called before the first frame update
    void Start()
    {
        monsterHealth = GetComponent<MonsterHealth>();
        boss = GetComponent<Boss>();
        player = GameObject.FindGameObjectWithTag("Player");
        rythmSystem = GameObject.FindGameObjectWithTag("Rythm").GetComponent<RythmSystem>();
        isAlreadyTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlreadyTrigger && monsterHealth.CurrentHealth <= triggerHealth)
        {
            DisablePlayer();
            player.transform.position = rythmSystem.CenterPoint.transform.position;
            isAlreadyTrigger = true;
        }
    }
    private void DisablePlayer()
    {
        player.GetComponent<IsometricPlayerMovementController>().enabled = false;
    }
}
