using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Menu_Option : MonoBehaviour
{
   
    public async void Play()
    {
        CanvasGroup cg = GameObject.Find("Modes").GetComponent<CanvasGroup>();
        cg.interactable = true;


        CanvasGroup mode1 = GameObject.Find("2x2").GetComponent <CanvasGroup>();
        StartCoroutine(PhysicalFunctionHelper.Fade(mode1, 0, 1, 0.5f));
        await Task.Delay(100);

        CanvasGroup mode2 = GameObject.Find("2x3").GetComponent<CanvasGroup>();
        StartCoroutine(PhysicalFunctionHelper.Fade(mode2, 0, 1, 0.5f));
        await Task.Delay(100);

        CanvasGroup mode3 = GameObject.Find("2x4").GetComponent<CanvasGroup>();
        StartCoroutine(PhysicalFunctionHelper.Fade(mode3, 0, 1, 0.5f));
        await Task.Delay(100);

        CanvasGroup mode4 = GameObject.Find("3x4").GetComponent<CanvasGroup>();
        StartCoroutine(PhysicalFunctionHelper.Fade(mode4, 0, 1, 0.5f));
        await Task.Delay(100);

        CanvasGroup mode5 = GameObject.Find("4x4").GetComponent<CanvasGroup>();
        StartCoroutine(PhysicalFunctionHelper.Fade(mode5, 0, 1, 0.5f));
        await Task.Delay(100);

        CanvasGroup mode6 = GameObject.Find("4x5").GetComponent<CanvasGroup>();
        StartCoroutine(PhysicalFunctionHelper.Fade(mode6, 0, 1, 0.5f));
        await Task.Delay(100);

        CanvasGroup mode7 = GameObject.Find("4x6").GetComponent<CanvasGroup>();
        StartCoroutine(PhysicalFunctionHelper.Fade(mode7, 0, 1, 0.5f));
        await Task.Delay(100);

        CanvasGroup mode8 = GameObject.Find("5x6").GetComponent<CanvasGroup>();
        StartCoroutine(PhysicalFunctionHelper.Fade(mode8, 0, 1, 0.5f));
        await Task.Delay(100);

        CanvasGroup mode9 = GameObject.Find("6x6").GetComponent<CanvasGroup>();
        StartCoroutine(PhysicalFunctionHelper.Fade(mode9, 0, 1, 0.5f));
        await Task.Delay(100);

        CanvasGroup mode10 = GameObject.Find("6x7").GetComponent<CanvasGroup>();
        StartCoroutine(PhysicalFunctionHelper.Fade(mode10, 0, 1, 0.5f));
    }


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
