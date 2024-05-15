using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Option : MonoBehaviour
{
   

    public void PlayMode_1()
    {
        LocalStorage.SaveData(LocalStorage.StorageValues.PlayMode, "2x2");
        LocalStorage.SaveData(LocalStorage.StorageValues.CurrentProgress, "2x2");
        NavigationManager.SetMainScene(NavigationManager.SceneName.PlayMode_1);
    }

}
