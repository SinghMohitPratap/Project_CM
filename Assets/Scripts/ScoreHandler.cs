using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ScoreHandler : MonoBehaviour,IReset
{
    public TMP_Text matchText;
    public TMP_Text turnText;
    public TMP_Text scoreValue;

    int matchvalue;
    int turnvalue;
 
    public void ChangeMacthTextValue(int value) 
    {
        matchvalue += value;
        GameManager.Instance.matchValue = matchvalue;
        matchText.text = matchvalue.ToString();
        CalculateScore();
    }
    public  void ChangeturntextValue(int value) 
    {
        turnvalue += value;
        GameManager.Instance.turnValue = turnvalue;
        turnText.text = turnvalue.ToString();
        CalculateScore();
    }
    private void OnEnable()
    {
        GameManager.GameReset += ResetValues;
    }
    private void OnDisable()
    {
   
        GameManager.GameReset -= ResetValues;
    }

    public void SetValues(int turn ,int match) 
    {
        matchvalue = match;
        GameManager.Instance.matchValue = matchvalue;
        matchText.text = matchvalue.ToString();
        turnvalue = turn;
        GameManager.Instance.turnValue = turnvalue;
        turnText.text = turnvalue.ToString();

    }

   public void ResetValues()
    { 
        matchvalue = 0;
        turnvalue = 0;
        matchText.text = matchvalue.ToString();
        turnText.text = turnvalue.ToString();
    }
    public void CalculateScore() 
    {
       int score = (matchvalue * 10) - (turnvalue * 2);
        scoreValue.text = score.ToString();

    }
}

