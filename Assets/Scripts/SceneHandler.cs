using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    public static void Load(string sceneName){
        SceneManager.LoadScene(sceneName);
        unpause();
    }

    public static void unpause(){
        Time.timeScale = 1;
    }

    public static void Exit(){
        Application.Quit();
        Debug.Log("Game quit successfully.");
    }
}
