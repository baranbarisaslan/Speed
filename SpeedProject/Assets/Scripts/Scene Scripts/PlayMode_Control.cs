using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static AppConstants;
using static Unity.VisualScripting.Metadata;
using Random = System.Random;

public class PlayMode_Control : MonoBehaviour
{
    string mode;
    int row;
    int col;
    int totalcount;
    GameObject grid;
    GridLayoutGroup gridLayoutGroup;
    GameObject card;
    List<GameObject> cards;

    public List<GameObject> flippedCards = new List<GameObject>();


    private List<string> planets;

    async void Start()
    {
        grid = GameObject.Find("Grid");
        gridLayoutGroup = grid.GetComponent<GridLayoutGroup>();
        card = Resources.Load<GameObject>(PrefabConstants.PrefabsPath + "Game_Card_Prefab");
        cards = new List<GameObject>();
        SetPlanets();

        mode = LocalStorage.GetData(LocalStorage.StorageValues.PlayMode);
        row = Convert.ToInt32(mode[0].ToString());
        col = Convert.ToInt32(mode[2].ToString());
        totalcount = row * col / 2;
        gridLayoutGroup.constraintCount = col;

        await SetLayout();
    }

    async Task SetLayout()
    {
        Sprite planet;
        for (int i = 0; i < totalcount; i++)
        {
            string planetname = GetRandomPlanet();
            planet = Resources.Load<Sprite>(SpriteConstants.SpritesPath + planetname);
            if (planet == null)
            {
                Debug.LogError("Failed to load sprite: " + planetname);
                continue;
            }

            // Create two cards for each planet
            for (int j = 0; j < 2; j++)
            {
                GameObject gamecard = Instantiate(card, grid.transform, false);
                gamecard.name = "Card_" + (i * 2 + j).ToString();
                GameObject child = gamecard.transform.Find("Game_Card").gameObject;
                child.transform.Find("Icon").GetComponent<Image>().sprite = planet;
                gamecard.GetComponent<Card_Option>().planet = planetname;
            }
        }

        // Shuffle the cards in the grid and reorder them to mix the queue
        List<GameObject> cards = new List<GameObject>();
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            cards.Add(grid.transform.GetChild(i).gameObject);
        }
        ShuffleList.Shuffle(cards);
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.SetSiblingIndex(i);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            await Task.Delay(150);
            cards[i].GetComponent<Card_Option>().GenerateFlip();
        }
        grid.GetComponent<CanvasGroup>().interactable = true;

    }


    public void SetPlanets()
    {
        planets = new List<string>();
        for (int i = 1; i <= 20; i++)
        {
            planets.Add($"Planet{i}");
        }
    }

    public string GetRandomPlanet()
    {
        if (planets.Count == 0)
        {
            throw new InvalidOperationException("No more planets available.");
        }

        Random random = new Random();
        int index = random.Next(planets.Count);
        string selectedPlanet = planets[index]; 
        planets.RemoveAt(index); 
        return selectedPlanet;
    }



    public void AddCardToList(GameObject card)
    {
        flippedCards.Add(card);

        while (flippedCards.Count >= 2)
        {
            ProcessCardPair(flippedCards[0], flippedCards[1]);
        }
    }

    private void ProcessCardPair(GameObject card1, GameObject card2)
    {
        flippedCards.Remove(card1);
        flippedCards.Remove(card2);
        if (card1.GetComponent<Card_Option>().planet == card2.GetComponent<Card_Option>().planet)
        {
            Success(card1, card2);
        }
        else
        {
            Failed(card1, card2);
        }
    }



    async void Success(GameObject card1, GameObject card2)
    {
        Debug.Log("THEY ARE THE SAME");
        await Task.Delay(500);

        CanvasGroup cg1 = card1.GetComponent<CanvasGroup>();
        cg1.interactable = false;
        StartCoroutine(PhysicalFunctionHelper.Fade(cg1, 1, 0, 0.5f));

        CanvasGroup cg2 = card2.GetComponent<CanvasGroup>();
        cg2.interactable = false;
        StartCoroutine(PhysicalFunctionHelper.Fade(cg2, 1, 0, 0.5f));
    }

    async void Failed(GameObject card1, GameObject card2)
    {
        Debug.Log("THEY ARE DIFFERENT");
        await Task.Delay(500);

        card1.GetComponent<Card_Option>().GenerateFlip();
        card2.GetComponent<Card_Option>().GenerateFlip();
    }
}
