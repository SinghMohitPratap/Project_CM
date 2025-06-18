
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour 
{
   
    public int cardId;
    public Vector2 cardPos;
    public string cardName;

    internal float easeTime;
    bool cardHide;
    bool isFlipped ;
    public Vector2[] card_fg_size = new Vector2[4];
    CardGridScaler cardGridScaler;
  
    public bool isHide=false;
    private void Awake()
    {
        isFlipped = true;
        var cardScalerArr = GameObject.FindObjectsByType<CardGridScaler>( FindObjectsSortMode.None);
        cardGridScaler = cardScalerArr[0];
        easeTime = 3f;
    }

    private void OnEnable()
    {
        GameManager.resizeCardsEvent += ScaleCharacterCards;
    }
    private void OnDisable()
    {
        GameManager.resizeCardsEvent -= ScaleCharacterCards; ;
    }
    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        ScaleCharacterCards();
        yield return new WaitForSeconds(easeTime);
        StartCoroutine(FlipCard());
        cardName = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite.name;
    }

    public void ScaleCharacterCards() 
    {
        print("Resizing");
        int factorX = (int)GetComponent<RectTransform>().sizeDelta.x;
        factorX = ToLowerPowerOfTwo(factorX);
        int factorY = (int)GetComponent<RectTransform>().sizeDelta.y;
        factorY = ToLowerPowerOfTwo(factorY);
        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(factorX, factorY);
    }

    int ToLowerPowerOfTwo(int value)
    {
        if (value < 1) return 0;

        int power = 1;
        while (power * 2 <= value)
        {
            power *= 2;
        }
        return power;
    }

    public void OnClickCardBG() 
    {
        StartCoroutine(FlipCard());   
    }
    
    public  IEnumerator FlipCard() 
    {       
        isFlipped = !isFlipped;
        if (isFlipped)
        {           
            while (transform.GetChild(1).localEulerAngles.y < 89.9f)
            {
                yield return null;
                transform.GetChild(1).localRotation = Quaternion.AngleAxis(90, Vector3.up);
                GameManager.Instance.AddCards(this);
            }
        }
        else 
        {       
            while (transform.GetChild(1).localEulerAngles.y > 0f)
            {
                yield return null;
                transform.GetChild(1).localRotation = Quaternion.AngleAxis(0, Vector3.up);
            }
        }
       
    }

    public void PlayCardSound() 
    {      
       SoundManager.Instance.PlayFLipCardSound();
    }
   
}
