using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class LevelControl : MonoBehaviour
{

    public string nextScene;
    public string currentScene;
    public int enemycount = 0;
    public int playercount = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(currentScene);
    }


}
