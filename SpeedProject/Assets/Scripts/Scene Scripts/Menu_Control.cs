using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Control : MonoBehaviour
{
    string previous;

    void Start()
    {
        previous = LocalStorage.GetData(LocalStorage.StorageValues.CurrentProgress);
        if(!string.IsNullOrEmpty(previous))
        {
            GameObject.Find("Continue").GetComponent<Button>().interactable = true;
        }
    }
}
