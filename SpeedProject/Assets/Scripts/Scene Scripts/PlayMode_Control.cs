using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    string highest;
    int score;
    int high;

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
        LocalStorage.SaveData(LocalStorage.StorageValues.Streak, "1");

        mode = LocalStorage.GetData(LocalStorage.StorageValues.PlayMode);
        Debug.Log(mode);
        Calculate();

        totalcount = row * col / 2;
        gridLayoutGroup.constraintCount = col;
        if(col >= 6)
        {
            gridLayoutGroup.cellSize = new Vector2(150f, 150f);
        }
        else if (col >= 4)
        {
            gridLayoutGroup.cellSize = new Vector2(200f, 200f);
        }
        else
        {
            gridLayoutGroup.cellSize = new Vector2(250f, 250f);
        }

        highest = LocalStorage.GetData(LocalStorage.StorageValues.HighestScore);
        if(!string.IsNullOrEmpty(highest))
        {
            GameObject.Find("Highest").GetComponent<TMP_Text>().text += " " + highest;
        }

        await SetLayout();
    }

    async Task SetLayout()
    {
        for (int i = 0; i < totalcount; i++)
        {
            Sprite planet_sprite = GetRandomPlanet();
            if (planet_sprite == null)
            {
                Debug.LogError("Failed to load sprite: " + planet_sprite.name);
                continue;
            }

            // Create two cards for each planet
            for (int j = 0; j < 2; j++)
            {
                GameObject gamecard = Instantiate(card, grid.transform, false);
                GameObject child = gamecard.transform.Find("Game_Card").gameObject;
                child.transform.Find("Icon").GetComponent<Image>().sprite = planet_sprite;
                gamecard.GetComponent<Card_Option>().planet = planet_sprite.name;
                gamecard.name = "Card_" + (i * 2 + j).ToString();
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

        await Task.Delay(250 * totalcount);
        for (int i = 0; i < cards.Count; i++)
        {
            await Task.Delay(100);
            cards[i].GetComponent<Card_Option>().GenerateFlip();
        }
        grid.GetComponent<CanvasGroup>().interactable = true;

    }


    public void SetPlanets()
    {
        planets = new List<string>();
        for (int i = 1; i <= 40; i++)
        {
            planets.Add($"Planet{i}");
        }
    }

    public Sprite GetRandomPlanet()
    {
        if (planets.Count == 0)
        {
            throw new InvalidOperationException("No more planets available.");
        }

        Random random = new Random();
        int index = random.Next(planets.Count);
        string selectedPlanet = planets[index]; 
        planets.RemoveAt(index);
        Sprite planet = Resources.Load<Sprite>(SpriteConstants.SpritesPath + selectedPlanet);
        return planet;
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
        await Task.Delay(500);
        AudioClip clip = Resources.Load<AudioClip>(AudioConstants.AudioPath + "audio_correct");
        PlayAudio.Play(clip);

        CanvasGroup cg1 = card1.GetComponent<CanvasGroup>();
        cg1.interactable = false;
        StartCoroutine(PhysicalFunctionHelper.Fade(cg1, 1, 0, 0.5f));

        CanvasGroup cg2 = card2.GetComponent<CanvasGroup>();
        cg2.interactable = false;
        StartCoroutine(PhysicalFunctionHelper.Fade(cg2, 1, 0, 0.5f));
        AddScore();
        totalcount--;
        if(totalcount == 0)
        {
            GameOver();
        }

    }

    async void Failed(GameObject card1, GameObject card2)
    {
        await Task.Delay(500);
        AudioClip clip = Resources.Load<AudioClip>(AudioConstants.AudioPath + "audio_wrong");
        PlayAudio.Play(clip);
        card1.GetComponent<Card_Option>().GenerateFlip();
        card2.GetComponent<Card_Option>().GenerateFlip();
        LocalStorage.SaveData(LocalStorage.StorageValues.Streak, "1");

    }


    ///Calculates the added score and the streak bonus
    private int CalculateScoreMultiplier(int streak)
    {
        if (streak > 2)
            return 10 * row * (streak - 1);
        return 10 * row;
    }

    void AddScore()
    {
        int streak = Convert.ToInt32(LocalStorage.GetData(LocalStorage.StorageValues.Streak));
        int currentScore = Convert.ToInt32(GameObject.Find("Score").GetComponent<TMP_Text>().text);
        int scoreToAdd = CalculateScoreMultiplier(streak);

        if(streak > 2)
        {
            GameObject.Find("Bonus").GetComponent<TMP_Text>().text = $"BONUS x{streak} {scoreToAdd} POINT!";
            ShowPopup();
        }
       

        int newScore = currentScore + scoreToAdd;
        GameObject.Find("Score").GetComponent<TMP_Text>().text = newScore.ToString();
        LocalStorage.SaveData(LocalStorage.StorageValues.Streak, (++streak).ToString());
    }


    async void ShowPopup()
    {
        GameObject.Find("BonusPopup").GetComponent<Animator>().SetBool("Show", true);
        await Task.Delay(100);
        GameObject.Find("BonusPopup").GetComponent<Animator>().SetBool("Show", false);

    }

    async void GameOver()
    {
        string oldheighest = LocalStorage.GetData(LocalStorage.StorageValues.HighestScore);
        AudioClip clip = Resources.Load<AudioClip>(AudioConstants.AudioPath + "audio_over");
        PlayAudio.Play(clip);

        if (!string.IsNullOrEmpty(oldheighest))
            high = Convert.ToInt32(oldheighest);
        else
            high = 0;

        if (score > high)
        {
            LocalStorage.SaveData(LocalStorage.StorageValues.HighestScore, score.ToString(), true);
        }
        LocalStorage.SaveData(LocalStorage.StorageValues.Streak, "1", true);

        await Task.Delay(1250);
        NextLevel();
    }


    void Calculate()
    {
        if (mode == "2x2")
        {
            row = 2;
            col = 2;
        }
        else if (mode == "2x3")
        {
            row = 2;
            col = 3;
        }
        else if (mode == "2x4")
        {
            row = 2;
            col = 4;
        }
        else if (mode == "3x4")
        {
            row = 3;
            col = 4;
        }
        else if (mode == "3x6")
        {
            row = 3;
            col = 6;
        }
        else if (mode == "4x4")
        {
            row = 4;
            col = 4;
        }
        else if (mode == "4x5")
        {
            row = 4;
            col = 5;
        }
        else if (mode == "4x6")
        {
            row = 4;
            col = 6;
        }
        else if (mode == "5x6")
        {
            row = 5;
            col = 6;
        }
        else if (mode == "6x6")
        {
            row = 6;
            col = 6;
        }   
        else if (mode == "6x7")
        {
            row = 6;
            col = 7;
        }
    }

    void NextLevel()
    {
        List<string> modes = new List<string> { "2x2", "2x3", "2x4", "3x4", "4x4", "4x5", "4x6", "5x6", "6x6", "6x7" };
        int currentModeIndex = modes.IndexOf(mode);

        if (currentModeIndex < modes.Count - 1)
        {
            string nextMode = modes[currentModeIndex + 1];
            LocalStorage.SaveData(LocalStorage.StorageValues.PlayMode, nextMode);
            LocalStorage.SaveData(LocalStorage.StorageValues.CurrentProgress, nextMode);
            NavigationManager.SetMainScene(NavigationManager.SceneName.PlayMode);
        }
        else
        {
            NavigationManager.SetMainScene(NavigationManager.SceneName.Menu);
        }
    }
}
