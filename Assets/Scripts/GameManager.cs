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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetPlayerPrefsData();
            openCardsArr = new Card[2];
            ChangeOrientationForMobile();
        }
        else 
        {
            Destroy(gameObject);
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
            if (openCardsArr[0].cardName.Equals(openCardsArr[1].cardName))
            {
                scoreHandler.ChangeMacthTextValue(1);
                scoreHandler.ChangeturntextValue(1);            
                StartCoroutine(DisableCards(openCardsArr));
                Gamecounter += 2;
            }
            else 
            {
                scoreHandler.ChangeturntextValue(1);
                StartCoroutine(HideCardsAgain(openCardsArr));
            }
            CardsOpen = 0;
        }      
    }


    IEnumerator HideCardsAgain(  Card[] openCardsArr) 
    {
        yield return new WaitForSeconds(1);
        openCardsArr[0].StartCoroutine("FlipCard");
        openCardsArr[1].StartCoroutine("FlipCard");
        ChangeCardsBtnActiveState(true);
    }
  
    IEnumerator DisableCards(  Card[] openCardsArr) 
    {
        yield return new WaitForSeconds(1);
        openCardsArr[0].gameObject.transform.GetChild(0).gameObject.SetActive(false);
        openCardsArr[1].gameObject.transform.GetChild(0).gameObject.SetActive(false);
        openCardsArr[0].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        openCardsArr[1].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        ChangeCardsBtnActiveState(true);
        if (CardsList.Count == Gamecounter) 
        {
           playAgainPanel.SetActive(true);  
        }
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
        ResetGameplay();
        cardGridScaler.enabled = true;
    }
    public void OnClickHomeBtn() 
    {
        ResetGameplay();
    }
}

