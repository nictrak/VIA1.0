using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WarpPoint : MonoBehaviour
{
    public string ToScene;
    public string SelectedTag = "Enemy"  ;
    private int count = 0 ;
    private  GameObject[] objects;
    public int leaveEnemy = 0 ;
    // Start is called before the first frame update
    void Start()
    {
        if (SelectedTag == "") SelectedTag = "Enemy";
        GameObject[] objects = GameObject.FindGameObjectsWithTag(SelectedTag);
    }

    // Update is called once per frame
    void Update()
    {
        if( count > 100) {
            objects = GameObject.FindGameObjectsWithTag(SelectedTag);
            count = 0 ;
        }else {
            count += 1;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" )
        {
            if (leaveEnemy >= objects.Length ){
                SceneManager.LoadScene(ToScene);
            }
        }
    }
}
