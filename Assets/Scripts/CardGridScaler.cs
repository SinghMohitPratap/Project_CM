using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// //Script will be enable when user click on playbutton in Menu Page
/// It enabled through inspector play button On click event...
/// </summary>

public class CardGridScaler : MonoBehaviour,IReset   
{
    public RectTransform cardGrid;  
    public GameObject cardPrefab;


    //Configure Rows,Columns,Space and Ease Time, Game Manager is responsible for setting this values....
    [Header("Grid Configurator")]
    public int rows = 3;
    public int columns = 6;
    public float spacing = 20f;
    public float visibilityEaseTime = 3f;


    Vector2 screenSize;
    GameObject[,] cards_2D;

    private void OnEnable()
    {
        if ((rows * columns) % 2 == 0)
        {
            UpdateGrid();
            UpdateCardsData();
            screenSize = new Vector2(Screen.width, Screen.height);
            StartCoroutine(CheckForGridSize());
        }
        else
            Debug.LogError("Please make sure the multiple of rows and column are even.");

        GameManager.GameReset += ResetValues;
    }

    IEnumerator CheckForGridSize() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(0.1f);

            if ((Screen.width != screenSize.x) || (Screen.height != screenSize.y)) 
            {
                UpdateGrid();
                screenSize.x = Screen.width;
                screenSize.y = Screen.height;
            }
        }
    }
    void UpdateGrid()
    {
        GridLayoutGroup grid = cardGrid.GetComponent<GridLayoutGroup>();

        float totalSpacingX = spacing * (columns - 1);
        float totalSpacingY = spacing * (rows - 1);

        float cellWidth = (cardGrid.rect.width - totalSpacingX) / columns;
        float cellHeight = (cardGrid.rect.height - totalSpacingY) / rows;

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.spacing = new Vector2(spacing, spacing);
        grid.cellSize = new Vector2(cellWidth, cellHeight);

        GameManager.Instance.InvokeCardResizeEvent();

    }

    void UpdateCardsData() 
    {
       
        foreach (Transform child in cardGrid)
        {
            Destroy(child.gameObject);
            GameManager.Instance.CardsList.Clear();
        }

        int counter = 0;
        cards_2D = new GameObject[rows, columns];
        for (int i = 0; i < cards_2D.GetLength(0); i++)
        {
            for (int j = 0; j < cards_2D.GetLength(1); j++)
            {
                var temp = Instantiate(cardPrefab, cardGrid);
                Card card = temp.GetComponent<Card>();
                card.cardId = counter++;
                card.cardPos = new Vector2(i, j);
                card.easeTime = visibilityEaseTime;
                GameManager.Instance.CardsList.Add(temp);
                cards_2D[i, j] = temp;
              
            }
        }
        List<GameObject> cardListCopy = new List<GameObject>(GameManager.Instance.CardsList);
        List<CardsData> spriteListCopy = new List<CardsData>(GameManager.Instance.SpriteList);
        int cardCount = cardListCopy.Count;
        while (cardCount > 0)
        {

            int randomSprite = UnityEngine.Random.Range(0, spriteListCopy.Count);
            var tempSprite = spriteListCopy[randomSprite];
            for (int j = 0; j < 2; j++)
            {
                int randomCardValue = UnityEngine.Random.Range(0, cardListCopy.Count);
                var card = cardListCopy[randomCardValue];
                card.transform.GetChild(0).GetComponent<Image>().sprite = tempSprite.sprite;
               
                cardListCopy.Remove(card);
                cardCount--;
            }
            spriteListCopy.Remove(tempSprite);


        }
        cardListCopy.Clear();
        cardListCopy = null;
        spriteListCopy.Clear();
        spriteListCopy = null;
    }
   
    private void OnDisable()
    {
        GameManager.GameReset -= ResetValues;
    }
    public void ResetValues()
    {
       this.enabled = false;
    }
}
