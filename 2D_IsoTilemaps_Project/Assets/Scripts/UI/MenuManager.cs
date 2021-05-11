using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private string startPrototypeScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartPrototype()
    {
        SceneManager.LoadScene(startPrototypeScene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
