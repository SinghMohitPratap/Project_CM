using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public interface IReset
{
    void ResetValues();    
}

public enum eDifficulty {Easy=0,Medium=1,Hard=2 }


[System.Serializable]
public class CardsData
{
    public string name;
    public Sprite sprite;
}

[System.Serializable]
public class GameData
{

    public List<CardData> cardDataList;

    public int matchCount;
    public int turnCount;
    public int rows;
    public int columns;
    public int gameCounter;
}

[System.Serializable]
public class CardData
{
    public string name;
    public bool isHide;
}

public class GameManager : MonoBehaviour
{
    //Singleton...
    private static GameManager instance;
    public static GameManager Instance 
    {
        get { return instance; }
        private set { }   
    }
    private GameManager() {  }

    //Events for Screen Resize and Game Reset...
    public static event Action resizeCardsEvent;
    public static event Action GameReset;

    [Header("Difficulty State")]
    public eDifficulty currentDifficulty;

    [Header("Componenet Ref")]
    public GameObject CardsContainer; 
    public CardGridScaler cardGridScaler;
    public ScoreHandler scoreHandler;
    public GameObject playAgainPanel;

    [Header("Cards and Sprites Data")]
    public List<CardsData> SpriteList;
    public Vector2[] gridLayout;
    public List<GameObject> CardsList;

    private void Awake()
    {
       //DataPersistence.DeleteData();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSaveData();
            SetPlayerPrefsData();
            openCardsArr = new Card[2];
            ChangeOrientationForMobile();
        }
        else 
        {
            Destroy(gameObject);
        }  
    }

    public bool isDataExist = false;
    public void LoadSaveData()
    {
       gameData = DataPersistence.LoadDataFromDisk();
        if (gameData != null)
        {
            if (gameData.cardDataList.Count > 0)
            {
                //Data Exist..
                cardGridScaler.rows = gameData.rows;
                cardGridScaler.columns = gameData.columns;
                scoreHandler.SetValues(gameData.turnCount, gameData.matchCount);
                Gamecounter = gameData.gameCounter;
                // cardGridScaler.enabled = true;
                isDataExist = true;
            }
            else
            {
                isDataExist = false;
            }


        }
        else 
        {
            scoreHandler.SetValues(0, 0);
        }
    }


    public void SetCardPrefabValueFromDisk() 
    {

        Dictionary<string,Sprite> spriteContainer = new Dictionary<string,Sprite>();
        for (int i = 0; i < SpriteList.Count; i++)
        {
            spriteContainer.Add(SpriteList[i].name, SpriteList[i].sprite);
        }

        for (int i = 0; i < CardsList.Count; i++)
        {
            if (gameData != null)
            {
                if (gameData.cardDataList[i].isHide)
                {
                    CardsList[i].GetComponent<Card>().isHide = true;
                    CardsList[i].transform.GetChild(0).gameObject.SetActive(false);
                    CardsList[i].transform.GetChild(1).gameObject.SetActive(false);
                }
                else 
                {
                    if (spriteContainer.ContainsKey(gameData.cardDataList[i].name))
                    {

                        CardsList[i].transform.GetChild(0).GetComponent<Image>().sprite = spriteContainer[gameData.cardDataList[i].name];
                    }
                    else
                    {
                        Debug.Log("Key not found: " + gameData.cardDataList[i].name);
                    }                                                   
                }

            }

        }
    }

    private void SetPlayerPrefsData()
    {
        if (PlayerPrefs.HasKey("Difficulty"))
        {
            int diffValue = PlayerPrefs.GetInt("Difficulty");
            ChangeDifficultyState(diffValue);
        }
        else
        {
            PlayerPrefs.SetInt("Difficulty", 0);
            ChangeDifficultyState(0);
        }
    }


    public void InvokeCardResizeEvent() 
    {
        resizeCardsEvent?.Invoke();
    }

    private void ChangeDifficultyState(int diiValue) 
    {
        switch (diiValue)
        {
            case (int)eDifficulty.Easy:
                {
                    currentDifficulty = eDifficulty.Easy;
                    cardGridScaler.rows =(int)gridLayout[0].x;
                    cardGridScaler.columns = (int)gridLayout[0].y;
                    cardGridScaler.visibilityEaseTime = 3f;
                    PlayerPrefs.SetInt("Difficulty", 0);
                    break;
                }
            case (int)eDifficulty.Medium:
                {
                    currentDifficulty = eDifficulty.Medium;
                    cardGridScaler.rows = (int)gridLayout[1].x;
                    cardGridScaler.columns = (int)gridLayout[1].y;
                    cardGridScaler.visibilityEaseTime = 2.5f;
                    PlayerPrefs.SetInt("Difficulty", 1);
                    break;
                }
            case (int)eDifficulty.Hard:
                {
                    currentDifficulty = eDifficulty.Hard;
                    cardGridScaler.rows = (int)gridLayout[2].x;
                    cardGridScaler.columns = (int)gridLayout[2].y;
                    cardGridScaler.visibilityEaseTime = 2f;
                    PlayerPrefs.SetInt("Difficulty", 2);
                    break;
                }
            default:
                break;
        }
       
    }



    public void ResetDataIfDifficultyChanged() 
    {
        scoreHandler.SetValues(0, 0);
        ResetGameplay();
        gameData = null;
        isDataExist = false;

    }

    private void ChangeOrientationForMobile() 
    {
#if UNITY_ANDROID || UNITY_IOS

        Screen.orientation = ScreenOrientation.LandscapeLeft;
#endif
    }

    private void ResetGameplay() 
    {
        CardsList.Clear();
        Gamecounter = 0;
        CardsOpen = 0;
        matchValue = 0;
        turnValue = 0;
        GameReset?.Invoke();     
    }

    
    //Scoring System....
    int CardsOpen;
    Card[] openCardsArr;
    int Gamecounter = 0;

    public void AddCards(Card cardRef) 
    {
        openCardsArr[CardsOpen] = cardRef;
        CardsOpen++;
        if (CardsOpen == 2) 
        {
            ChangeCardsBtnActiveState(false);
            if (openCardsArr[0].cardName.Equals(openCardsArr[1].cardName)) // cards got matched...
            {
                scoreHandler.ChangeMacthTextValue(1);
                scoreHandler.ChangeturntextValue(1);            
                StartCoroutine(DisableCards(openCardsArr));
                RemoveCardsFromGridArray(openCardsArr);
                Gamecounter += 2;
            }
            else // cards do not matched...
            {
                scoreHandler.ChangeturntextValue(1);
                StartCoroutine(HideCardsAgain(openCardsArr));
            }
            CardsOpen = 0;
        }      
    }


    void RemoveCardsFromGridArray(Card[] openCards) 
    {
        Vector2 pos1 = openCards[0].cardPos;
        Vector2 pos2 = openCards[1].cardPos;

        //cardGridScaler.cards_2D[(int)pos1.x, (int)pos1.y] = null;
       // cardGridScaler.cards_2D[(int)pos2.x, (int)pos2.y] = null;
    }

    IEnumerator HideCardsAgain(Card[] openCardsArr) 
    {
        yield return new WaitForSeconds(1);
        openCardsArr[0].StartCoroutine("FlipCard");
        openCardsArr[1].StartCoroutine("FlipCard");
        ChangeCardsBtnActiveState(true);
    }
  
    IEnumerator DisableCards(Card[] openCardsArr) 
    {
        yield return new WaitForSeconds(1);
        
        openCardsArr[0].gameObject.transform.GetChild(0).gameObject.SetActive(false);
        openCardsArr[1].gameObject.transform.GetChild(0).gameObject.SetActive(false);
        openCardsArr[0].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        openCardsArr[1].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        openCardsArr[0].isHide = true;
        openCardsArr[1].isHide = true;
        ChangeCardsBtnActiveState(true);
        if (CardsList.Count == Gamecounter) 
        {
           playAgainPanel.SetActive(true);
            GameOver();
        }
    }

    void GameOver() 
    {
        cardGridScaler.ResetCardsTwoDArr();
        GameManager.Instance.isDataExist = false;
        gameData = null;
        ResetGameplay();
    }

    void ChangeCardsBtnActiveState(bool value) 
    {
        foreach (var card in CardsList) 
        {
         card.transform.GetChild(1).GetComponent<Button>().enabled = value;
        }   
    }


    /// Methods called from btn click event from outside...
    public void PlayAgain() 
    {
        gameData = null;
        ResetGameplay();
        cardGridScaler.enabled = true;
    }
    public void OnClickHomeBtn() 
    {
        //ResetGameplay();
    }


    internal GameData gameData;
    internal int matchValue;
    internal int turnValue;

    private void OnApplicationPause(bool pause)
    {
        if (pause&& cardGridScaler.cards_2D!=null) 
        {
         
            if (gameData == null)
                gameData = new GameData();
            gameData.cardDataList?.Clear();
            gameData.cardDataList = new List<CardData>();
            for (int i = 0; i < CardsList.Count; i++)
            {
                CardData cardData = new CardData();
                cardData.name = CardsList[i].transform.GetChild(0).GetComponent<Image>().sprite.name;
                cardData.isHide = CardsList[i].GetComponent<Card>().isHide;            
                gameData.cardDataList.Add(cardData);
            }

            gameData.matchCount = matchValue;
            gameData.turnCount = turnValue;
            gameData.gameCounter = Gamecounter;
            gameData.rows = cardGridScaler.cards_2D.GetLength(0);
            gameData.columns = cardGridScaler.cards_2D.GetLength(1);
            string data = JsonUtility.ToJson(gameData);

            DataPersistence.SaveDataToDisk(data);

        }
    }
    private void OnApplicationQuit()
    {
        if (cardGridScaler.cards_2D != null)
        {
           
            if (gameData == null)
                gameData = new GameData();          
            gameData.rows = cardGridScaler.cards_2D.GetLength(0);
            gameData.columns = cardGridScaler.cards_2D.GetLength(1);
            gameData.matchCount = matchValue;
            gameData.turnCount = turnValue;
            gameData.gameCounter = Gamecounter;
            gameData.cardDataList?.Clear();
            gameData.cardDataList = new List<CardData>();

            for (int i = 0; i < CardsList.Count; i++)
            {
                CardData cardData = new CardData();
                cardData.name = CardsList[i].transform.GetChild(0).GetComponent<Image>().sprite.name;
                cardData.isHide = CardsList[i].GetComponent<Card>().isHide;              
                gameData.cardDataList.Add(cardData);
            }
            string data = JsonUtility.ToJson(gameData);

            DataPersistence.SaveDataToDisk(data);
        }
    }
}

