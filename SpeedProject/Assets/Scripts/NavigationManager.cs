using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class NavigationManager: MonoBehaviour
{ 

    public enum SceneName
    {
       Menu,
       PlayMode_1
    }

   
    public static void SetMainScene(SceneName nextScene)
    {
        SceneManager.LoadScene(nextScene.ToString());
    }


    public static SceneName GetCurrentScene()
    {
        string name =  SceneManager.GetActiveScene().name;
        return FindScene(name);
    }

    public static SceneName FindScene(string name)
    {
        foreach (SceneName scene in Enum.GetValues(typeof(SceneName)))
        {
            if (scene.ToString().Equals(name))
            {
                return scene;
            }
        }

        return SceneName.Menu;
    }


}
