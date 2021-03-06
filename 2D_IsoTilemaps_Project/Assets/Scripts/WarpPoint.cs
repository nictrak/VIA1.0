using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WarpPoint : MonoBehaviour
{
    public string ToScene;
    public string SelectedTag  ;
    private int count = 0 ;
    private  GameObject[] objects;
    public int leaveEnemy = 0 ;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(SelectedTag);
    }

    // Update is called once per frame
    void Update()
    {
        if( count > 1000) {
            objects = GameObject.FindGameObjectsWithTag(SelectedTag);
            count = 0 ;
        }else {
            count += 1;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(ToScene);
        if(collision.gameObject.tag == "Player" )
        {
            if (leaveEnemy >= objects.Length ){
                SceneManager.LoadScene(ToScene);
            }
        }
    }
}
