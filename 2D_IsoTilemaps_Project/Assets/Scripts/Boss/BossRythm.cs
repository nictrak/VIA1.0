using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRythm : MonoBehaviour
{
    [SerializeField]
    private float triggerHealth;

    private MonsterHealth monsterHealth;
    private bool isAlreadyTrigger;
    // Start is called before the first frame update
    void Start()
    {
        monsterHealth = GetComponent<MonsterHealth>();
        isAlreadyTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!isAlreadyTrigger && monsterHealth.CurrentHealth <= triggerHealth)
        {
            SceneManager.LoadScene("Scene_Rythm");
            isAlreadyTrigger = true;
        }*/
    }
    private void OnDestroy()
    {
        SceneManager.LoadScene("Scene_Rythm");
    }

}
