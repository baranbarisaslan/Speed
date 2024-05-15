using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Option : MonoBehaviour
{
   

    public void PlayMode()
    {
        LocalStorage.SaveData(LocalStorage.StorageValues.PlayMode, gameObject.name);
        LocalStorage.SaveData(LocalStorage.StorageValues.CurrentProgress, gameObject.name);
        NavigationManager.SetMainScene(NavigationManager.SceneName.PlayMode);
    }



    public void CurrentProgress()
    {
        string current = LocalStorage.GetData(LocalStorage.StorageValues.CurrentProgress);
        if(!string.IsNullOrEmpty(current))
        {
            LocalStorage.SaveData(LocalStorage.StorageValues.PlayMode, current);
            NavigationManager.SetMainScene(NavigationManager.SceneName.PlayMode);
        }
        else
        {
            Debug.Log("CURRENT IS NULL");
        }
    }

}
