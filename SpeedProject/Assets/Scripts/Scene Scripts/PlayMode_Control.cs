using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AppConstants;

public class PlayMode_Control : MonoBehaviour
{
    string mode;
    int row;
    int col;
    int totalcount;
    GameObject grid;
    GridLayoutGroup gridLayoutGroup;
    GameObject card;
    void Start()
    {
        grid = GameObject.Find("Grid");
        gridLayoutGroup = grid.GetComponent<GridLayoutGroup>();
        card = Resources.Load<GameObject>(PrefabConstants.PrefabsPath + "Game_Card_Prefab");

        mode = LocalStorage.GetData(LocalStorage.StorageValues.PlayMode);
        row = Convert.ToInt32(mode[0].ToString());
        col = Convert.ToInt32(mode[2].ToString());
        totalcount = row * col;
        gridLayoutGroup.constraintCount = col;

        SetLayout();
    }



    void SetLayout()
    {
        for(int i = 0; i < totalcount; i++)
        {
            GameObject gamecard = Instantiate(card,grid.transform,false);
        }
    }
}
